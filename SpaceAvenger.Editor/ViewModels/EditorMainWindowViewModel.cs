using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.Spaceships;
using SpaceAvenger.Editor.ViewModels.SpaceshipsParts.Base;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Settings;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class EditorMainWindowViewModel : ViewModelBase
    {
        #region Fields
        private GameObject m_root;
        private GameViewHost m_gameViewHost;

        private bool m_rootEnabled;
        private bool m_ShowGizmos;
        private bool m_ShowBorders;

        private double m_posX;
        private double m_posY;
        private double m_rot;
        private double m_ScaleX;
        private double m_ScaleY;
        private ObservableCollection<ShipModule> m_ShipModules;
        private ObservableCollection<string> m_resourceNames;
        private IResourceLoader m_ResourceLoader;
        private string m_SelectedRoot;
        #endregion

        #region Properties
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

        public ObservableCollection<ShipModule> ShipModules
        { get => m_ShipModules; set => m_ShipModules = value; }

        public ObservableCollection<string> ResourceNames
        { get => m_resourceNames; set => m_resourceNames = value; }

        public string SelectedRoot
        {
            get => m_SelectedRoot;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    m_root.GetComponent<Sprite>(nameof(Sprite)).Load(m_ResourceLoader.ResourceDictionary, value);
                }
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
            m_ShipModules = new ObservableCollection<ShipModule>();
            m_resourceNames = new ObservableCollection<string>();
            m_SelectedRoot = string.Empty;
            m_gameViewHost = new GameViewHost();
            m_root = new SpaceshipMock();
            m_rootEnabled = true;
            m_ShowBorders = true;
            m_ShowGizmos = true;

            GESettings.DrawGizmo = true;
            GESettings.DrawBorders = true;

            OnAddModuleButtonPressed = new Command(
                OnAddModuleButtonPressedExecute,
                CanOnAddModuleButtonPressedExecute
                );

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            GameView.ClearVisuals();
            if (m_root != null)
            {
                m_root.Update();
                m_root.Render(GameView);
            }
        }
        #endregion

        #region Methods
        #region On Add Module Button Pressed
        private bool CanOnAddModuleButtonPressedExecute(object p) => true;

        private void OnAddModuleButtonPressedExecute(object p)
        {
            var module = new ShipModule(ResourceNames);
            ShipModules.Add(module);
        }
        #endregion
        #endregion
    }
}
