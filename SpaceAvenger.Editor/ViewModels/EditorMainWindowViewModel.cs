using Newtonsoft.Json.Linq;
using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.Spaceships;
using SpaceAvenger.Editor.ViewModels.SpaceshipsParts.Base;
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

namespace SpaceAvenger.Editor.ViewModels
{
    internal class EditorMainWindowViewModel : ValidationViewModel
    {
        #region Fields
        private GameObject m_root;
        private GameViewHost m_gameViewHost;
        private TreeItemViewModel m_SelectedItem;

        private bool m_ShowGizmos;
        private bool m_ShowBorders;

        private double m_posX;
        private double m_posY;
        private double m_rot;
        private double m_ScaleX;
        private double m_ScaleY;
        private double m_CenterPositionX;
        private double m_CenterPositionY;
        private ObservableCollection<ChildObjectViewModel> m_ShipModules;
        private ObservableCollection<string> m_resourceNames;
        private IResourceLoader m_ResourceLoader;
        #endregion

        #region Properties

        public TreeItemViewModel SelectedItem 
        {
            get=> m_SelectedItem;
            set=> Set(ref m_SelectedItem, value);
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
                UpdatePositionX(GetCurrentGameObject(), (float)PositionX);
            }
        }

        public double PositionY
        {
            get => m_posY;
            set 
            { 
                Set(ref m_posY, value);
                UpdatePositionY(GetCurrentGameObject(), (float)PositionY);
            }
        }

        public double Rot
        {
            get => m_rot;
            set 
            { 
                Set(ref m_rot, value);
                UpdateRotation(GetCurrentGameObject(), (float)Rot);
            }
        }

        public double ScaleX
        {
            get => m_ScaleX;
            set 
            { 
                Set(ref m_ScaleX, value);
                UpdateScaleX(GetCurrentGameObject(), (float)ScaleX);
            }
        }

        public double ScaleY
        {
            get => m_ScaleY;
            set 
            { 
                Set(ref m_ScaleY, value);
                UpdateScaleY(GetCurrentGameObject(), (float)ScaleY);
            }
        }

        public double CenterPositionX 
        {
            get=>m_CenterPositionX;
            set 
            {
                Set(ref m_CenterPositionX, value);
                UpdateCenterPositionX(GetCurrentGameObject(), (float)CenterPositionX);
            }
        }

        public double CenterPositionY 
        {
            get=> m_CenterPositionY;
            set 
            {
                Set(ref m_CenterPositionY, value);
                UpdateCenterPositionY(GetCurrentGameObject(), (float)CenterPositionY);
            }
        }

        public ObservableCollection<ChildObjectViewModel> Children
        { get => m_ShipModules; set => m_ShipModules = value; }

        public ObservableCollection<string> ResourceNames
        { get => m_resourceNames; set => m_resourceNames = value; }

               
        #endregion

        

        #region Commands
        public ICommand OnAddGameObjectButtonPressed { get; }
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
            m_ShipModules = new ObservableCollection<ChildObjectViewModel>();
            m_resourceNames = new ObservableCollection<string>();
            m_gameViewHost = new GameViewHost();
            m_gameViewHost.StartGame();
            m_ShowBorders = true;
            m_ShowGizmos = true;
            m_SelectedItem = new TreeItemViewModel();

            m_ScaleX = 1f;
            m_ScaleY = 1f;

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
        private bool CanOnAddGameObjectButtonPressedExecute(object p) => m_root != null;

        private void OnAddGameObjectButtonPressedExecute(object p)
        {
           
        }
        #endregion
        private void LoadCurrentGameObjProperties(IGameObject obj)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);

                PositionX = t.Position.X;
                PositionY = t.Position.Y;
                Rot = t.Rotation;
                ScaleX = t.Scale.Width;
                ScaleY = t.Scale.Height;
                CenterPositionX = t.CenterPosition.X;
                CenterPositionY = t.CenterPosition.Y;
            }
        }

        private void UpdatePositionX(IGameObject obj, float x)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);
                float y = t.Position.Y;
                t.Position = new Vector2(x, y);
            }
        }

        private void UpdatePositionY(IGameObject obj, float y)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);
                float x = t.Position.X;
                t.Position = new Vector2(x, y);
            }
        }

        private void UpdateRotation(IGameObject obj, float rotation)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);
                t.Rotation = rotation;
            }
            
        }

        private void UpdateScaleX(IGameObject obj, float x)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);
                float y = t.Scale.Height;
                t.Scale = new SizeF(x, y);
            }
        }

        private void UpdateScaleY(IGameObject obj, float y)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);
                float x = t.Scale.Width;
                t.Scale = new SizeF(x, y);
            }
        }

        private void UpdateCenterPositionX(IGameObject obj, float x)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);
                float y = t.CenterPosition.Y;
                t.CenterPosition = new Vector2(x, y);
            }
        }

        private void UpdateCenterPositionY(IGameObject obj, float y)
        {
            if (obj != null)
            {
                var t = obj.GetComponent<TransformComponent>(true);
                float x = t.CenterPosition.X;
                t.CenterPosition = new Vector2(x, y);
            }
        }

        private IGameObject GetCurrentGameObject()
        {
            return null;
        }

        #endregion
    }
}
