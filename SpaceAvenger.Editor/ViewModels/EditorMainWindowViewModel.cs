using Newtonsoft.Json.Linq;
using SpaceAvenger.Editor.Services.Base;
using SpaceAvenger.Editor.Spaceships;
using SpaceAvenger.Editor.ViewModels.SpaceshipsParts.Base;
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

        private bool m_rootEnabled;
        private bool m_ShowGizmos;
        private bool m_ShowBorders;
        private bool m_RootSelected;
        private string m_RootName;
        private bool m_ChooseRootImageEnabled;
        private bool m_SelectRootEnabled;
        private int m_SelectedChildIndex;

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
        private string m_SelectedRoot;
        #endregion

        #region Properties
        
        public int SelectedChildIndex 
        { 
            get=> m_SelectedChildIndex;
            set 
            {
                Set(ref m_SelectedChildIndex, value);
                if (SelectedChildIndex >= 0)
                {                    
                    LoadCurrentGameObjProperties(GetCurrentGameObject());
                    RootSelected = false;
                }
            }
        }

        public bool SelectRootEnabled 
        {
            get=> m_SelectRootEnabled;
            set=> Set(ref m_SelectRootEnabled, value); 
        }

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

        public string SelectedRoot
        {
            get => m_SelectedRoot;
            set 
            {
                Set(ref m_SelectedRoot, value);

                if (m_root == null && !string.IsNullOrEmpty(SelectedRoot))
                {
                    m_root = new ModuleMock(RootName);
                    m_root.GetComponent<Sprite>(true).Load(m_ResourceLoader.ResourceDictionary, SelectedRoot);
                    m_gameViewHost.World.Add(m_root);
                    SelectRootEnabled = true;
                    RootSelected = true;
                }
                else
                {
                    m_root.GetComponent<Sprite>(true).Load(m_ResourceLoader.ResourceDictionary, SelectedRoot);
                    SelectRootEnabled = true;
                    RootSelected = true;
                }
            }
        }

        public bool RootSelected
        {
            get => m_RootSelected;
            set 
            { 
                Set(ref m_RootSelected, value);
                if (m_RootSelected)
                {
                    SelectedChildIndex = -1;
                    LoadCurrentGameObjProperties(m_root);
                }
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
                            SelectRootEnabled = true;
                        }
                        else
                        {
                            ChooseRootImageEnabled = false;
                            SelectRootEnabled = false;
                        }
                    break;
                }

                return error;
            }
        }
        #endregion

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
            m_ShipModules = new ObservableCollection<ChildObjectViewModel>();
            m_resourceNames = new ObservableCollection<string>();
            m_SelectedRoot = string.Empty;
            m_gameViewHost = new GameViewHost();
            m_gameViewHost.StartGame();
            m_rootEnabled = true;
            m_ShowBorders = true;
            m_ShowGizmos = true;
            m_RootName = string.Empty;
            m_SelectRootEnabled = false;
            m_ChooseRootImageEnabled = false;
            m_SelectedChildIndex = -1;

            m_ScaleX = 1f;
            m_ScaleY = 1f;

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
        private bool CanOnAddModuleButtonPressedExecute(object p) => m_root != null;

        private void OnAddModuleButtonPressedExecute(object p)
        {
            string name = $"Module{Children.Count + 1}";
            var child = new ModuleMock(name);
            var module = new ChildObjectViewModel(Children.Count + 1, name, m_ResourceLoader,
                child, ResourceNames);
            m_root.AddChild(child);
            Children.Add(module);
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
            IGameObject obj = null;
            if (SelectedChildIndex >= 0)
            {
                obj = Children[SelectedChildIndex].GameObject;
            }
            else if (RootSelected && m_root is not null)
            {
                obj = m_root;
            }
            return obj;
        }

        #endregion
    }
}
