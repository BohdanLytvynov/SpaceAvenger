using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.Spaceships;
using SpaceAvenger.Editor.ViewModels.SpaceshipsParts.Base;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO.Pipelines;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.Helpers;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Settings;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class EditorMainWindowViewModel : ValidationViewModel
    {
        #region Fields
        private GameObject m_root;
        private Dictionary<int, IGameObject> m_childrenMap;
        private GameViewHost m_gameViewHost;

        private bool m_rootEnabled;
        private bool m_ShowGizmos;
        private bool m_ShowBorders;
        private bool m_RootSelected;
        private string m_RootName;
        private bool m_ChooseRootImageEnabled;

        private double m_posX;
        private double m_posY;
        private double m_rot;
        private double m_ScaleX;
        private double m_ScaleY;
        private ObservableCollection<ShipModuleViewModel> m_ShipModules;
        private ObservableCollection<string> m_resourceNames;
        private IResourceLoader m_ResourceLoader;
        private string m_SelectedRoot;
        #endregion

        #region Properties

        public bool ChooseRootImageEnabled 
        { 
            get=> m_ChooseRootImageEnabled; 
            set => Set(ref m_ChooseRootImageEnabled, value);
        }

        public string RootName 
        {
            get=> m_RootName; 
            set=> Set(ref m_RootName, value);
        }

        public bool RootEnabled
        {
            get => m_rootEnabled;
            set
            {
                Set(ref m_rootEnabled, value);
                m_root.Enabled = value;
            }
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

        public double PositionX
        {
            get => m_posX;
            set
            {
                Set(ref m_posX, value);
                var y = m_root.Position.Y;
                m_root.Position = new Vector2((float)value, y);
            }
        }

        public double PositionY
        {
            get => m_posY;
            set
            {
                Set(ref m_posY, value);
                var x = m_root.Position.X;
                m_root.Position = new Vector2(x, (float)value);
            }
        }

        public double Rot
        {
            get => m_rot;
            set
            {
                Set(ref m_rot, value);
                m_root.Rotation = value;
            }
        }

        public double ScaleX
        {
            get => m_ScaleX;
            set 
            {
                Set(ref m_ScaleX, value);
                var h = m_root.Scale.Height;
                m_root.Scale = new SizeF((float)value, h);
            }
        }

        public double ScaleY
        {
            get => m_ScaleY;
            set 
            {
                Set(ref m_ScaleY, value);
                var w = m_root.Scale.Width;
                m_root.Scale = new SizeF(w, (float)value);
            }
        }
        #endregion

        #region IDataErrorInfo
        public override string this[string columnName]
        {
            get
            { 
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(RootName):
                        if (!ValidationHelper.TextIsEmpty(RootName, out error))
                        {
                            ChooseRootImageEnabled = true;

                            if(m_root != null && !m_root.Name.Equals(RootName))
                                m_root.Name = RootName;
                        }
                        else
                        {
                            ChooseRootImageEnabled = false;
                        }
                        break;
                }

                return error;
            }
        }
        #endregion

        public ObservableCollection<ShipModuleViewModel> ShipModules
        { get => m_ShipModules; set => m_ShipModules = value; }

        public ObservableCollection<string> ResourceNames
        { get => m_resourceNames; set => m_resourceNames = value; }

        public string SelectedRoot
        {
            get => m_SelectedRoot;
            set
            {
                if (m_root == null && !string.IsNullOrEmpty(value))
                {
                    m_root = new ModuleMock(RootName);
                    m_root.GetComponent<Sprite>(nameof(Sprite)).Load(m_ResourceLoader.ResourceDictionary, value);
                    m_gameViewHost.World.Add(m_root);
                }
                else
                {
                    m_root.GetComponent<Sprite>(nameof(Sprite)).Load(m_ResourceLoader.ResourceDictionary, value);
                }
            }
        }

        public bool RootSelected 
        {
            get => m_RootSelected;
            set 
            {
                Set(ref m_RootSelected, value);
                if (!m_RootSelected)
                    return;

                PositionX = m_root.Position.X;
                PositionY = m_root.Position.Y;
                Rot = m_root.Rotation;
                ScaleX = m_root.Scale.Width;
                ScaleY = m_root.Scale.Height;
            }
        }

        #region Commands
        public ICommand OnAddModuleButtonPressed { get; }
        #endregion

        #region Ctor
        public EditorMainWindowViewModel(IResourceLoader resourceLoader) : this()
        {
            m_ResourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));

            foreach (var resName in m_ResourceLoader.LoadAll())
                ResourceNames.Add(resName);
        }

        public EditorMainWindowViewModel()
        {
            m_childrenMap = new Dictionary<int, IGameObject>();
            m_ShipModules = new ObservableCollection<ShipModuleViewModel>();
            m_resourceNames = new ObservableCollection<string>();
            m_SelectedRoot = string.Empty;
            m_gameViewHost = new GameViewHost();
            m_gameViewHost.StartGame();
            m_rootEnabled = true;
            m_ShowBorders = true;
            m_ShowGizmos = true;
            m_RootName = string.Empty;

            GESettings.DrawGizmo = true;
            GESettings.DrawBorders = true;

            OnAddModuleButtonPressed = new Command(
                OnAddModuleButtonPressedExecute,
                CanOnAddModuleButtonPressedExecute
                );
        }

        #endregion 

        #region Methods
        #region On Add Module Button Pressed
        private bool CanOnAddModuleButtonPressedExecute(object p) => true;

        private void OnAddModuleButtonPressedExecute(object p)
        {
            var child = new ModuleMock("Module");
            m_childrenMap.Add(child.Id, child);
            var module = new ShipModuleViewModel(child, ResourceNames);
            m_root.AddChild(child);
            ShipModules.Add(module);
        }
        #endregion
        #endregion
    }
}
