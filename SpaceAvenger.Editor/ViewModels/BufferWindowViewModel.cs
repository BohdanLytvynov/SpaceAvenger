using SpaceAvenger.Editor.MessageBus;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.ViewModels.TreeItems;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.MessageBus.Base;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.Factories.Base;

namespace SpaceAvenger.Editor.ViewModels
{
    internal enum SelectedTab
    { 
        GameObjects = 0, 
        Components = 1,
    }

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
        private int m_selectedGameObjectIndex;
        private int m_selectedComponentIndex;
        private SelectedTab m_SelectedTab;
        #endregion

        #region Properties

        public string Title 
        { get => m_title; set => Set(ref m_title, value); }

        public ObservableCollection<TreeItemViewModel> GameObjectBuffer 
        { get => m_gameObjectBuffer; set => m_gameObjectBuffer = value; }

        public ObservableCollection<ComponentViewModel> ComponentBuffer 
        { get => m_ComponentsBuffer; set => m_ComponentsBuffer = value; }

        public int SelectedGameObjectIndex 
        { get => m_selectedGameObjectIndex; set => Set(ref m_selectedGameObjectIndex, value); }

        public int SelectedComponentIndex 
        { get => m_selectedComponentIndex; set => Set(ref m_selectedComponentIndex, value); }
        #endregion

        #region Commands

        public ICommand OnGameObjectsTabPressed { get; }
        public ICommand OnComponentsTabPressed { get; }
        public ICommand OnSelectButtonPressed { get; }
        public ICommand OnRemoveButtonPressed { get; }
        public ICommand OnClearButtonPressed { get; }
        public ICommand OnCancelButtonPressed { get; }

        #endregion

        #region Ctor
        public BufferWindowViewModel(IMessageBus messageBus) : this()
        {
            m_messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

            #region Subscribe

            Subscriptions.Add(m_messageBus.RegisterHandler<CopyToBufferMessage, IGameEngineEntity>(OnCopyToBuffer));

            #endregion
        }

        public BufferWindowViewModel()
        {
            #region Init Fields
            m_title = "Editor Buffer";
            m_gameObjectBuffer = new ObservableCollection<TreeItemViewModel>();
            m_ComponentsBuffer = new ObservableCollection<ComponentViewModel>();
            m_selectedGameObjectIndex = -1;
            m_selectedComponentIndex = -1;
            #endregion

            #region Init Commands

            OnGameObjectsTabPressed = new Command
                (
                    OnGameObjectTabButtonPressedExecute,
                    CanOnGameObjectTabButtonPressedExecute
                );

            OnComponentsTabPressed = new Command
                (
                    OnComponentsTabButtonPressedExecute,
                    CanOnComponentsTabButtonPressedExecute
                );

            OnSelectButtonPressed = new Command
                (
                    OnSelectButtonPressedExecute,
                    CanOnSelectButtonPressedExecute
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

        #region Subscriptions

        private void OnCopyToBuffer(CopyToBufferMessage copyToBufferMessage)
        { 
            if(copyToBufferMessage == null) return;
            if(copyToBufferMessage.Content == null) return;
            
        }

        #endregion

        #region On GameObjects Tab Button Pressed

        private bool CanOnGameObjectTabButtonPressedExecute(object p) => true;

        private void OnGameObjectTabButtonPressedExecute(object p)
        { 
            m_SelectedTab = SelectedTab.GameObjects;
        }

        #endregion

        #region On Components Tab Pressed

        private bool CanOnComponentsTabButtonPressedExecute(object p) => true;

        private void OnComponentsTabButtonPressedExecute(object p)
        {
            m_SelectedTab = SelectedTab.Components;
        }

        #endregion

        #region On Select Button Pressed

        private bool CanOnSelectButtonPressedExecute(object p)
        {
            switch (m_SelectedTab)
            {
                case SelectedTab.GameObjects:
                    return m_selectedGameObjectIndex >= 0;
                case SelectedTab.Components:
                    return m_selectedComponentIndex >= 0;
            }

            return false;
        }

        private void OnSelectButtonPressedExecute(object p)
        {
            switch (m_SelectedTab)
            {
                case SelectedTab.GameObjects:
                    
                    break;

                case SelectedTab.Components:

                    break;
            }
        }

        #endregion

        #region On Remove Button Pressed

        private bool CanOnRemoveButtonPressedExecute(object p) => m_selectedGameObjectIndex >= 0;

        private void OnRemoveButtonPressedExecute(object p)
        {
            GameObjectBuffer.RemoveAt(SelectedGameObjectIndex);
        }

        #endregion

        #region On Cancel Button Pressed
        private bool CanOnCancelButtonPressedExecute(object p) => true;

        private void OnCancelButtonPressedExecute(object p)
        {
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
            m_gameObjectBuffer.Clear();
        }

        #endregion

        #endregion
    }
}
