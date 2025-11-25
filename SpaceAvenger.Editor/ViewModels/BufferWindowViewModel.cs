using SpaceAvenger.Editor.MessageBus;
using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.ViewModels.TreeItems;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Component.Base;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class BufferWindowViewModel : SubscriptableViewModel
    {
        #region Events
        public event Action OnCancelWindow;
        #endregion

        #region Fields

        private string m_title;
        private ObservableCollection<TreeItemViewModel> m_gameObjectBuffer;
        private ObservableCollection<ComponentViewModel> m_ComponentsBuffer;
        private IMessageBus m_messageBus;
        private int m_selectedComponentIndex;
        private int m_SelectedTabIndex;
        private TreeItemViewModel m_selectedItem;
        #endregion

        #region Properties
        public int SelectedTabIndex
        { get => m_SelectedTabIndex; set => Set(ref m_SelectedTabIndex, value); }

        public string Title 
        { get => m_title; set => Set(ref m_title, value); }

        public ObservableCollection<TreeItemViewModel> GameObjectBuffer 
        { get => m_gameObjectBuffer; set => m_gameObjectBuffer = value; }

        public ObservableCollection<ComponentViewModel> ComponentBuffer 
        { get => m_ComponentsBuffer; set => m_ComponentsBuffer = value; }

        public int SelectedComponentIndex 
        { get => m_selectedComponentIndex; set => Set(ref m_selectedComponentIndex, value); }
        #endregion

        #region Commands

        public ICommand OnPasteButtonPressed { get; }
        public ICommand OnRemoveButtonPressed { get; }
        public ICommand OnClearButtonPressed { get; }
        public ICommand OnCancelButtonPressed { get; }

        #endregion

        #region Ctor
        public BufferWindowViewModel(IMessageBus messageBus) : this()
        {
            m_messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

            #region Subscribe

            Subscriptions.Add(
                m_messageBus.RegisterHandler<CopyToGameObjectBufferMessage, TreeItemViewModel>
                (OnCopyToGameObjectBuffer));
            Subscriptions.Add(
                m_messageBus.RegisterHandler<CopyToComponentsBufferMessage, ComponentViewModel>
                (OnCopyToComponentsBuffer));
            #endregion
        }

        public BufferWindowViewModel()
        {
            #region Init Fields
            m_title = "Editor Buffer";
            m_gameObjectBuffer = new ObservableCollection<TreeItemViewModel>();
            m_ComponentsBuffer = new ObservableCollection<ComponentViewModel>();
            m_selectedComponentIndex = -1;
            m_selectedItem = new TreeItemViewModel(-1, null);
            #endregion

            #region Init Commands

            OnPasteButtonPressed = new Command
                (
                    OnPasteButtonPressedExecute,
                    CanOnPasteButtonPressedExecute
                );

            OnRemoveButtonPressed = new Command
                (
                    OnRemoveButtonPressedExecute,
                    CanOnRemoveButtonPressedExecute
                );

            OnClearButtonPressed = new Command
                (
                    OnClearButtonPressedExecute,
                    CanOnClearButtonPressedExecute
                );

            OnCancelButtonPressed = new Command
                (
                    OnCancelButtonPressedExecute,
                    CanOnCancelButtonPressedExecute
                );

            #endregion
        }
        #endregion

        #region Methods

        private void RemoveObjectFromTreeRec(TreeItemViewModel item, ObservableCollection<TreeItemViewModel> src, bool removed)
        {
            if (removed)
                return;

            foreach (TreeItemViewModel itemViewModel in src)
            {
                if (removed)
                    break;

                if (itemViewModel.Id == item.Id)
                {
                    Unsubscribe(itemViewModel);
                    src.Remove(itemViewModel);
                    removed = true;
                    break;
                }

                RemoveObjectFromTreeRec(item, itemViewModel.Children, removed);
            }
        }

        #region Subscriptions

        private void OnCopyToGameObjectBuffer(CopyToGameObjectBufferMessage copyToBufferMessage)
        { 
            if(copyToBufferMessage == null) return;
            var content = copyToBufferMessage.Content;
            if (content == null) return;

            Subscribe(content);

            GameObjectBuffer.Add(content);
        }

        private void OnCopyToComponentsBuffer(CopyToComponentsBufferMessage msg)
        {
            if (msg == null) return;
            var content = msg.Content;
            if (content == null) return;

            ComponentBuffer.Add(content);
        }

        #endregion

        private void Subscribe(TreeItemViewModel treeItemViewModel)
        {
            if (treeItemViewModel == null)
                return;

            treeItemViewModel.ItemSelected += TreeItemViewModel_ItemSelected;

            foreach (var item in treeItemViewModel.Children)
            {
                Subscribe(item);
            }
        }

        private void TreeItemViewModel_ItemSelected(int id)
        {
            m_selectedItem = null;

            if (id >= 0)
            {
                TreeItemViewModel.FindInCollection(id, GameObjectBuffer, ref m_selectedItem);

                TreeItemViewModel.UnselectAll(GameObjectBuffer);
                m_selectedItem.RaiseEvent = false;
                m_selectedItem.Selected = true;
                m_selectedItem.RaiseEvent = true;
            }
            else
            {
                TreeItemViewModel.UnselectAll(GameObjectBuffer);
            }
        }

        private void Unsubscribe(TreeItemViewModel treeItemViewModel)
        {
            if (treeItemViewModel == null)
                return;

            treeItemViewModel.ItemSelected -= TreeItemViewModel_ItemSelected;

            foreach (var item in treeItemViewModel.Children)
            {
                Unsubscribe(item);
            }
        }

        #region On Select Button Pressed

        private bool CanOnPasteButtonPressedExecute(object p)
        {
            switch (m_SelectedTabIndex)
            {
                case 0:
                    return m_selectedItem != null && m_selectedItem.ShowNumber >= 0;
                case 1:
                    return m_selectedComponentIndex >= 0;
            }

            return false;
        }

        private void OnPasteButtonPressedExecute(object p)
        {
            switch (m_SelectedTabIndex)
            {
                case 0:

                    m_messageBus.Send<PasteFromGameObjectBufferMessage, IGameObjectMock>(
                        new PasteFromGameObjectBufferMessage((IGameObjectMock)m_selectedItem.GameObject.Clone()));

                    break;

                case 1:

                    m_messageBus.Send<PasteFromComponentsBufferMessage, IGEComponent>
                        (new PasteFromComponentsBufferMessage(ComponentBuffer[SelectedComponentIndex].Component));

                    break;
            }
        }

        #endregion

        #region On Remove Button Pressed

        private bool CanOnRemoveButtonPressedExecute(object p) => m_selectedItem != null && m_selectedItem.ShowNumber >= 0;

        private void OnRemoveButtonPressedExecute(object p)
        {
            RemoveObjectFromTreeRec(m_selectedItem, GameObjectBuffer, false);
        }

        #endregion

        #region On Cancel Button Pressed
        private bool CanOnCancelButtonPressedExecute(object p) => true;

        private void OnCancelButtonPressedExecute(object p)
        {
            Clear();
            OnCancelWindow?.Invoke();
        }
        #endregion

        #region On Clear Button Pressed

        private bool CanOnClearButtonPressedExecute(object p) => true;

        private void OnClearButtonPressedExecute(object p)
        { 
            GameObjectBuffer.Clear();
            ComponentBuffer.Clear();
        }

        #endregion

        #region External

        public void Clear()
        {
            foreach (var item in m_gameObjectBuffer)
            {
                Unsubscribe(item);
            }

            m_gameObjectBuffer.Clear();
            m_ComponentsBuffer.Clear();
        }

        #endregion

        #endregion
    }
}
