using Newtonsoft.Json.Linq;
using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.TreeItems;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Data;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Settings;
using WPFGameEngine.WPF.GE.Component.Base;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using System.Linq.Expressions;
using SpaceAvenger.Editor.ViewModels.Components.Transform;
using WPFGameEngine.WPF.GE.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using SpaceAvenger.Editor.ViewModels.Components.Animation;
using SpaceAvenger.Editor.ViewModels.Components.Animator;
using System.Xaml;
using SpaceAvenger.Editor.ViewModels.Components.Sprite;
using System.Windows.Documents;
using System.Windows.Media;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class EditorMainWindowViewModel : ValidationViewModel
    {
        #region Fields
        private GameViewHost m_gameViewHost;
        private TreeItemViewModel? m_SelectedItem;

        private bool m_ShowGizmos;
        private bool m_ShowBorders;
        private bool m_enabled;
        private string m_objName;
        private double m_ZIndex;
        
        private IResourceLoader m_ResourceLoader;

        ObservableCollection<TreeItemViewModel> m_Items;
        ObservableCollection<ComponentViewModel> m_Componnts;
        #endregion

        #region Properties

        public double ZIndex 
        {
            get=> m_ZIndex;
            set 
            {
                Set(ref m_ZIndex, value);
                UpdateZIndex((int)ZIndex);
            }
        }

        public bool Enabled 
        {
            get=> m_enabled;
            set 
            {
                Set(ref m_enabled, value);
                UpdateEnabled(value);
            }
        }

        public string ObjName
        {
            get=> m_objName;
            set=> Set(ref m_objName, value);
        }

        public ObservableCollection<TreeItemViewModel> Items 
        {
            get=> m_Items;
            set=> m_Items = value;
        }

        public ObservableCollection<ComponentViewModel> Components
        {
            get=> m_Componnts;
            set=> m_Componnts = value;
        }

        public bool ShowGizmos
        {
            get => m_ShowGizmos;
            set
            {
                Set(ref m_ShowGizmos, value);
                GESettings.DrawGizmo = value;
            }
        }

        public bool ShowBorders
        {
            get => m_ShowBorders;
            set
            {
                Set(ref m_ShowBorders, value);
                GESettings.DrawBorders = value;
            }
        }

        public GameViewHost GameView
        { get => m_gameViewHost; set => Set(ref m_gameViewHost, value); }

        #endregion
        
        #region Commands
        public ICommand OnAddGameObjectButtonPressed { get; }

        public ICommand OnDeleteGameObjectButtonPressed { get; }
        #endregion

        #region Ctor
        public EditorMainWindowViewModel(IResourceLoader resourceLoader) : this()
        {
            m_ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
        }

        public EditorMainWindowViewModel()
        {
            m_gameViewHost = new GameViewHost();
            m_gameViewHost.StartGame();
            m_ShowBorders = true;
            m_ShowGizmos = true;
            m_SelectedItem = null;
            m_Items = new ObservableCollection<TreeItemViewModel>();
            m_Componnts = new ObservableCollection<ComponentViewModel>();
            m_objName = string.Empty;

            GESettings.DrawGizmo = true;
            GESettings.DrawBorders = true;

            OnAddGameObjectButtonPressed = new Command(
                OnAddGameObjectButtonPressedExecute,
                CanOnAddGameObjectButtonPressedExecute
                );

            OnDeleteGameObjectButtonPressed = new Command(
                OnDeleteObjectButtonPressedExecute,
                CanOnDeleteObjectButtonPressedExecute
                );
        }

        #endregion 

        #region Methods
        #region On Add Module Button Pressed
        private bool CanOnAddGameObjectButtonPressedExecute(object p) => true;

        private void OnAddGameObjectButtonPressedExecute(object p)
        {
            IGameObject obj = new GameObjectMock();
            obj.RegisterComponent(new Sprite(m_ResourceLoader.Load<ImageSource>("Empty")));

            if (m_SelectedItem == null)
            {
                TreeItemViewModel itemViewModel = new(Items.Count + 1, obj);
                itemViewModel.ItemSelected += ItemViewModel_ItemSelected;
                Items.Add(itemViewModel);
                m_gameViewHost.World.Add(obj);
            }
            else
            {
                TreeItemViewModel itemViewModel = new(m_SelectedItem.Children.Count + 1, obj);
                itemViewModel.ItemSelected += ItemViewModel_ItemSelected;                
                m_SelectedItem.Children.Add(itemViewModel);
                m_SelectedItem.GameObject.AddChild(obj);
            }

        }

        private void ItemViewModel_ItemSelected(TreeItemViewModel item)
        {
            m_SelectedItem = item;
            if (m_SelectedItem == null)
            {
                Components.Clear();
                return;
            }
            UpdateGameObjectProperties();
            UpdateComponents();
        }

        #endregion

        #region On Delete Object ButtonPressed

        private bool CanOnDeleteObjectButtonPressedExecute(object p) => m_SelectedItem != null;

        private void OnDeleteObjectButtonPressedExecute(object p)
        {
            if (m_SelectedItem != null)
            {
                RemoveFromWorld(m_SelectedItem.GameObject);
                RemoveObjectRec(m_SelectedItem, Items, false);
                Components.Clear();
                m_SelectedItem = null;
            }
            
        }

        #endregion

        private void RemoveObjectRec(TreeItemViewModel item, ObservableCollection<TreeItemViewModel> src, bool removed)
        {
            if (removed)
                return;

            foreach (TreeItemViewModel itemViewModel in src)
            {
                if (removed)
                    break;

                if (itemViewModel.Id == item.Id)
                { 
                    src.Remove(itemViewModel);
                    removed = true;
                    break;
                }

                RemoveObjectRec(item, itemViewModel.Children, removed);
            }
        }

        private void UpdateGameObjectProperties()
        {
            if (m_SelectedItem == null && m_SelectedItem.GameObject == null)
                return;

            Enabled = m_SelectedItem.GameObject.Enabled;
            ObjName = m_SelectedItem.GameObject.Name;
            ZIndex = m_SelectedItem.GameObject.ZIndex;
        }

        private void UpdateComponents()
        {
            if (m_SelectedItem == null && m_SelectedItem.GameObject == null)
                return;

            Components.Clear();
            var currentComponents = m_SelectedItem?.GameObject?.GetComponents() ?? null;

            if (currentComponents == null)
                return;

            foreach (var component in currentComponents)
            {
                ComponentViewModel c = null;
                switch (component.Name)
                {
                    case nameof(TransformComponent):
                        c = new TransformComponentViewModel(m_SelectedItem.GameObject);
                        break;
                    case nameof(Animation):
                        c = new AnimationComponentViewModel(m_SelectedItem.GameObject);
                        break;
                    case nameof(Animator):
                        c = new AnimatorComponentViewModel(m_SelectedItem.GameObject);
                        break;
                    case nameof(Sprite):
                        c = new SpriteComponentViewModel(m_SelectedItem.GameObject);
                        break;
                }

                Components.Add(c);
            }
        }

        private void RemoveFromWorld(IGameObject item)
        {
            if (item != null)
            {
                GameObject.RemoveObject((x) => x.Id == item.Id, m_gameViewHost.World, true);
            }
        }

        private void UpdateEnabled(bool value)
        {
            if (m_SelectedItem != null && m_SelectedItem.GameObject != null)
            { 
                m_SelectedItem.GameObject.Enabled = value;
            }
        }

        private void UpdateZIndex(int value)
        {
            if (m_SelectedItem != null && m_SelectedItem.GameObject != null)
            {
                m_SelectedItem.GameObject.ZIndex = value;
            }
        }

        #endregion
    }
}
