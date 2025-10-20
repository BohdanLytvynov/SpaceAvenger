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

namespace SpaceAvenger.Editor.ViewModels
{
    internal class EditorMainWindowViewModel : ValidationViewModel
    {
        #region Fields
        private GameObject m_root;
        private GameViewHost m_gameViewHost;
        private TreeItemViewModel? m_SelectedItem;

        private bool m_ShowGizmos;
        private bool m_ShowBorders;

        
        private IResourceLoader m_ResourceLoader;

        ObservableCollection<TreeItemViewModel> m_Items;
        ObservableCollection<ComponentViewModel> m_Componnts;
        #endregion

        #region Properties

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

            GESettings.DrawGizmo = true;
            GESettings.DrawBorders = true;

            OnAddGameObjectButtonPressed = new Command(
                OnAddGameObjectButtonPressedExecute,
                CanOnAddGameObjectButtonPressedExecute
                );
        }

        #endregion 

        #region Methods
        #region On Add Module Button Pressed
        private bool CanOnAddGameObjectButtonPressedExecute(object p) => true;

        private void OnAddGameObjectButtonPressedExecute(object p)
        {
            IGameObject obj = new GameObjectMock();
            TreeItemViewModel itemViewModel = new(Items.Count + 1, obj);
            itemViewModel.ItemSelected += ItemViewModel_ItemSelected;
            Items.Add(itemViewModel);

        }

        private void ItemViewModel_ItemSelected(TreeItemViewModel item)
        {
            m_SelectedItem = item;
        }

        #endregion

        private void UpdateGameObjectProperties()
        { 
            
        }

        private void UpdateComponents()
        { 
            Components.Clear();
        }

        #endregion
    }
}
