using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.TreeItems;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModelBaseLibDotNetCore.Commands;
using ViewModelBaseLibDotNetCore.VM;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Settings;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.ViewModels.Components.Transform;
using WPFGameEngine.WPF.GE.Component.Animators;
using SpaceAvenger.Editor.ViewModels.Components.Animations;
using SpaceAvenger.Editor.ViewModels.Components.Animators;
using SpaceAvenger.Editor.ViewModels.Components.Sprites;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Exceptions;
using System.Windows;
using WPFGameEngine.Timers.Base;
using WPFGameEngine.Extensions;
using SpaceAvenger.Editor.ViewModels.Options;
using WPFGameEngine.Enums;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.FactoryWrapper.Base;
using Microsoft.Win32;
using ViewModelBaseLibDotNetCore.Helpers;
using WPFGameEngine.WPF.GE.Serialization.GameObjects;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using SpaceAvenger.Editor.ViewModels.Components.RelativeTransforms;
using System.Drawing;
using System.Numerics;
using SpaceAvenger.Editor.ViewModels.Prefabs;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using SpaceAvenger.Services.WpfGameViewHost;

namespace SpaceAvenger.Editor.ViewModels
{
    internal class EditorMainWindowViewModel : ValidationViewModel
    {
        #region Fields
        private string m_title;
        private WpfGameObjectViewHost m_gameViewHost;
        private TreeItemViewModel? m_SelectedItem;

        private bool m_ShowGizmos;
        private bool m_ShowBorders;
        private bool m_enabled;
        private string m_objName;
        private double m_ZIndex;
        private int m_SelectedComponentIndex;
        private OptionsViewModel m_SelectedComponent;
        private PrefabViewModel m_SelectedPrefab;
        private ObservableCollection<TreeItemViewModel> m_Items;
        private ObservableCollection<ComponentViewModel> m_Components;
        private ObservableCollection<OptionsViewModel> m_ComponentsToAdd;
        private ObservableCollection<PrefabViewModel> m_prefabs;
        private List<GameObjectDto> m_Dtos;
        private IAssemblyLoader m_assemblyLoader;
        private IFactoryWrapper m_factoryWrapper;
        private IGameTimer m_gameTimer;
        private IGameObjectExporter m_gameObjectExporter;
        private IGameObjectImporter m_gameObjectImporter;
        private string m_pathToExport;

        #endregion

        #region Properties
        public string PathToExport 
        { get => m_pathToExport; set => Set(ref m_pathToExport, value); }

        public string Title
        { get => m_title; set => Set(ref m_title, value); }

        public OptionsViewModel SelectedComponent
        {
            get => m_SelectedComponent;
            set => Set(ref m_SelectedComponent, value);
        }

        public int SelectedComponentIndex
        {
            get => m_SelectedComponentIndex;
            set => Set(ref m_SelectedComponentIndex, value);
        }

        public double ZIndex
        {
            get => m_ZIndex;
            set
            {
                Set(ref m_ZIndex, value);
                UpdateZIndex((int)ZIndex);
            }
        }

        public bool Enabled
        {
            get => m_enabled;
            set
            {
                Set(ref m_enabled, value);
                UpdateEnabled(value);
            }
        }

        public string ObjName
        {
            get => m_objName;
            set => Set(ref m_objName, value);
        }

        public ObservableCollection<TreeItemViewModel> Items
        {
            get => m_Items;
            set => m_Items = value;
        }

        public ObservableCollection<ComponentViewModel> Components
        {
            get => m_Components;
            set => m_Components = value;
        }

        public ObservableCollection<OptionsViewModel> ComponentsToAdd
        {
            get => m_ComponentsToAdd;
            set => m_ComponentsToAdd = value;
        }

        public ObservableCollection<PrefabViewModel> Prefabs 
        {
            get => m_prefabs;
            set => m_prefabs = value;
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

        public WpfGameObjectViewHost GameView
        { get => m_gameViewHost; set => Set(ref m_gameViewHost, value); }

        public PrefabViewModel SelectedPrefab 
        { get => m_SelectedPrefab; set => Set(ref m_SelectedPrefab, value); }
        #endregion

        #region Commands
        public ICommand OnAddGameObjectButtonPressed { get; }

        public ICommand OnDeleteGameObjectButtonPressed { get; }

        public ICommand OnAddComponentButtonPressed { get; }

        public ICommand OnDeleteComponentButtonPressed { get; }

        public ICommand OnExportButtonPressed { get; }

        public ICommand OnOpenDialogButtonPressed { get; }

        public ICommand OnRefreshPrefabsButtonPressed { get; }
        public ICommand OnLoadPrefabButtonPressed { get; }
        public ICommand OnRemovePrefabButtonPressed { get; }
        #endregion

        #region IData Error Info
        public override string this[string columnName]
        {
            get 
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(PathToExport):
                        SetValidArrayValue(0, !ValidationHelper.TextIsEmpty(PathToExport, out error));
                        break;
                }

                return error;
            }
        }
        #endregion

        #region Ctor
        public EditorMainWindowViewModel(
            IFactoryWrapper factoryWrapper,
            IGameTimer gameTimer,
            IAssemblyLoader assemblyLoader,
            IGameObjectExporter gameObjectExporter,
            IGameObjectImporter gameObjectImporter
            ) : this()
        {
            m_factoryWrapper = factoryWrapper ?? throw new ArgumentNullException(nameof(factoryWrapper));
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_gameObjectExporter = gameObjectExporter ?? throw new ArgumentNullException(nameof(gameObjectExporter));
            m_gameObjectImporter = gameObjectImporter ?? throw new ArgumentNullException(nameof(gameObjectImporter));

            m_Dtos = new List<GameObjectDto>();

            #region Get All Components From GE
            m_ComponentsToAdd = new ObservableCollection<OptionsViewModel>();
            var geAssembly = m_assemblyLoader["WPFGameEngine"];

            foreach (var type in geAssembly.GetTypes())
            {
                var attr = type.GetAttribute<VisibleInEditor>();
                if (attr != null && attr.GetValue<GEObjectType>("GameObjectType") == GEObjectType.Component)
                {
                    m_ComponentsToAdd.Add(new OptionsViewModel(
                        attr.GetValue<string>("DisplayName"),
                        attr.GetValue<string>("FactoryName")));
                }
            }

            #endregion

            m_gameTimer = gameTimer ?? throw new ArgumentNullException(nameof(gameTimer));
            m_gameViewHost = new WpfGameObjectViewHost(m_gameTimer);
            m_gameViewHost.StartGame();
        }

        public EditorMainWindowViewModel()
        {
            InitValidArray(1);
            m_title = "Game Editor";
            m_ShowBorders = true;
            m_ShowGizmos = true;
            m_SelectedItem = null;
            m_Items = new ObservableCollection<TreeItemViewModel>();
            m_Components = new ObservableCollection<ComponentViewModel>();
            m_prefabs = new ObservableCollection<PrefabViewModel>();
            m_objName = string.Empty;
            m_SelectedComponentIndex = -1;
            m_SelectedComponent = new OptionsViewModel();
            m_pathToExport = string.Empty;
            m_SelectedPrefab = new PrefabViewModel();

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

            OnAddComponentButtonPressed = new Command(
                OnAddComponentButtonPressedExecute,
                CanOnAddComponentButtonPressedExecute
                );

            OnDeleteComponentButtonPressed = new Command(
                OnDeleteComponentButtonPressedExecute,
                CanOnDeleteComponentButtonPressedExecute);

            OnExportButtonPressed = new Command(
                OnExportButtonPressedExecute,
                CanOnExportButtonPressedExecute);

            OnOpenDialogButtonPressed = new Command(
                OnOpenDialogButtonPressedExecute,
                CanOnOpenDialogButtonPressedExecute);

            OnRefreshPrefabsButtonPressed = new Command(
                OnRefreshPrefabsButtonPressedExecute,
                CanOnRefreshPrefabsButtonPressedExecute);

            OnLoadPrefabButtonPressed = new Command(
                OnLoadPrefabButtonPressedExecute,
                CanOnLoadPrefabButtonPressedExecute);

            OnRemovePrefabButtonPressed = new Command(
                OnRemovePrefabButtonPressedExecute,
                CanOnRemovePrefabButtonPressedExecute);
        }

        #endregion 

        private void RefreshPrefabs()
        {
            Prefabs.Clear();
            m_Dtos.Clear();
            m_gameObjectImporter.PathToFolder = PathToExport;
            foreach (var item in m_gameObjectImporter.ImportObjects())
            {
                Prefabs.Add(new PrefabViewModel(Prefabs.Count + 1) { PrefabName = item.ObjectName });
                m_Dtos.Add(item);
            }
        }

        private TreeItemViewModel LoadObjectToTree(IGameObject gameObject, int count = 1)
        {
            if (gameObject == null)
                return null;
            
            TreeItemViewModel treeItem = new TreeItemViewModel(count, gameObject);
            treeItem.ItemSelected += ItemViewModel_ItemSelected;
            foreach (var item in gameObject.Children)
            {
                var ch = LoadObjectToTree(item, count);
                treeItem.Children.Add(ch);
                count++;
            }

            return treeItem;
        }

        private IGameObject LoadPrefabGameObjectRec(GameObjectDto dto)
        {
            if (dto == null)
                return null;

            IGameObject prefab = new GameObjectMock();
            prefab.Enabled = dto.Enabled;
            prefab.ZIndex = dto.ZIndex;
            prefab.ObjectName = dto.ObjectName;
            prefab.UniqueName = dto.UniqueName;

            foreach (var c in dto.Components)
            {
                prefab.RegisterComponent(c.ToObject(m_factoryWrapper));
            }

            foreach (var ch in dto.Children)
            {
                var child = LoadPrefabGameObjectRec(ch);
                prefab.AddChild(child);
            }

            return prefab;
        }

        #region Methods
        #region On Add Object Button Pressed
        private bool CanOnAddGameObjectButtonPressedExecute(object p) => true;

        private void OnAddGameObjectButtonPressedExecute(object p)
        {
            IGameObject obj = new GameObjectMock();

            if (m_SelectedItem == null)
            {
                TreeItemViewModel itemViewModel = new(Items.Count + 1, obj);
                itemViewModel.ItemSelected += ItemViewModel_ItemSelected;
                Items.Add(itemViewModel);
                var t = m_factoryWrapper.CreateObject<TransformComponent>();
                t.Scale = new SizeF(1f, 1f);
                t.CenterPosition = new Vector2(0.5f, 0.5f);
                obj.RegisterComponent(t);
                m_gameViewHost.AddObject(obj);
            }
            else
            {
                TreeItemViewModel itemViewModel = new(m_SelectedItem.Children.Count + 1, obj);
                itemViewModel.ItemSelected += ItemViewModel_ItemSelected;
                m_SelectedItem.Children.Add(itemViewModel);
                var t = m_factoryWrapper.CreateObject<RelativeTransformComponent>();
                t.Scale = new SizeF(1f, 1f);
                t.CenterPosition = new Vector2(0.5f, 0.5f);
                obj.RegisterComponent(t);
                m_SelectedItem.GameObject.AddChild(obj);
            }

            var sprite = m_factoryWrapper.CreateObject<Sprite>();
            sprite.Load("Empty");
            obj.RegisterComponent(sprite);
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

        private void Unselect(TreeItemViewModel node)
        {
            if (node == null)
                return;

            foreach (var child in node.Children)
            { 
                child.RaiseEvent = false;
                child.Selected = false;
                child.RaiseEvent = true;
                Unselect(child);
            }
        }

        private void UnselectAll(ObservableCollection<TreeItemViewModel> tree)
        {
            foreach (TreeItemViewModel item in tree)
            {
                item.RaiseEvent = false;
                item.Selected = false;
                item.RaiseEvent = true;
                Unselect(item);
            }
        }

        #endregion

        #region On Delete Object ButtonPressed

        private bool CanOnDeleteObjectButtonPressedExecute(object p) => m_SelectedItem != null;

        private void OnDeleteObjectButtonPressedExecute(object p)
        {
            if (m_SelectedItem != null)
            {
                RemoveFromWorld(m_SelectedItem.GameObject);
                m_SelectedItem.ItemSelected -= ItemViewModel_ItemSelected;
                RemoveObjectFromTreeRec(m_SelectedItem, Items, false);
                Components.Clear();
                m_SelectedItem = null;
            }

        }

        #endregion

        #region On Add Component Button Pressed
        private bool CanOnAddComponentButtonPressedExecute(object p) =>
            m_SelectedItem != null && SelectedComponent != null 
            && !string.IsNullOrEmpty(SelectedComponent.FactoryName);

        private void OnAddComponentButtonPressedExecute(object p)
        {
            try
            {
                var component = (IGEComponent)m_factoryWrapper.CreateObject(SelectedComponent.FactoryName);
                m_SelectedItem.GameObject.RegisterComponent(component);
                Components.Add(CreateComponentViewModel(component));
            }
            catch (ComponentAlreadyRegisteredException ex)
            {
                MessageBox.Show(ex.Message, m_title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region On Delete Component Button Pressed

        private bool CanOnDeleteComponentButtonPressedExecute(object p) =>
            m_SelectedItem != null && SelectedComponentIndex >= 0;

        private void OnDeleteComponentButtonPressedExecute(object p)
        {
            var componentViewModel = Components[SelectedComponentIndex];
            componentViewModel.GameObject.UnregisterComponent(componentViewModel.ComponentName);
            Components.RemoveAt(SelectedComponentIndex);
            SelectedComponentIndex = -1;
        }

        #endregion

        #region On Export Button Pressed Execute
        private bool CanOnExportButtonPressedExecute(object p) => Items.Count > 0 && GetValidArrayValue(0);

        private void OnExportButtonPressedExecute(object p)
        {
            int count = 0;
            Exception ex = null;
            foreach (var item in GameView.World)
            {
                if (item.IsExported)
                {
                    m_gameObjectExporter.Export(item, PathToExport, ex);
                    count++;
                }

                if (ex != null)
                    break;
            }

            if(count == 0)
                return;

            if (ex == null)
                MessageBox.Show("Export successful!", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show($"Export failed! Error: {ex.Message}", Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region On Open Dialog Button Pressed
        private bool CanOnOpenDialogButtonPressedExecute(object p) => true;

        private void OnOpenDialogButtonPressedExecute(object p)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            if (!string.IsNullOrEmpty(PathToExport))
            { 
                dialog.DefaultDirectory = PathToExport;
            }
            if (dialog.ShowDialog() ?? false)
            { 
                PathToExport = dialog.FolderName;
            }
        }
        #endregion

        #region On Refresh Prefabs Button Pressed
        private bool CanOnRefreshPrefabsButtonPressedExecute(object p) =>
            !string.IsNullOrEmpty(PathToExport);

        private void OnRefreshPrefabsButtonPressedExecute(object p)
        {
            RefreshPrefabs();
        }
        #endregion

        #region On Load Prefab Button Pressed
        private bool CanOnLoadPrefabButtonPressedExecute(object p) =>
            SelectedPrefab != null && SelectedPrefab.ShowNumber > 0;

        private void OnLoadPrefabButtonPressedExecute(object p)
        {
            GameView.ClearWorld();
            Items.Clear();
            if (SelectedPrefab != null)
            {
                var dto = m_Dtos
                    .Where(d => d.ObjectName.Equals(SelectedPrefab.PrefabName))
                    .FirstOrDefault();

                if (dto != null)
                {
                    IGameObject obj = LoadPrefabGameObjectRec(dto);
                    GameView.AddObject(obj);
                    var treeItem = LoadObjectToTree(obj);
                    Items.Add(treeItem);
                }
                else
                {
                    MessageBox.Show($"Prefab <{SelectedPrefab.PrefabName}> was not found!",
                        Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region On Remove Prefab Button Pressed
        private bool CanOnRemovePrefabButtonPressedExecute(object p) =>
            SelectedPrefab != null && SelectedPrefab.ShowNumber > 0;

        private void OnRemovePrefabButtonPressedExecute(object p)
        {
            Prefabs.Remove(SelectedPrefab);
            GameObject.RemoveObject(obj => obj.ObjectName.Equals(SelectedPrefab.PrefabName), 
                GameView.World, true);
            SelectedPrefab = new PrefabViewModel();
        }
        #endregion

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
                    itemViewModel.ItemSelected -= ItemViewModel_ItemSelected;
                    src.Remove(itemViewModel);
                    removed = true;
                    break;
                }

                RemoveObjectFromTreeRec(item, itemViewModel.Children, removed);
            }
        }

        private void UpdateGameObjectProperties()
        {
            if (m_SelectedItem == null && m_SelectedItem.GameObject == null)
                return;

            Enabled = m_SelectedItem.GameObject.Enabled;
            ObjName = m_SelectedItem.GameObject.ObjectName;
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
                Components.Add(CreateComponentViewModel(component));
            }
        }

        private void RemoveFromWorld(IGameObject item)
        {
            m_gameViewHost.RemoveObject(item);
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

        private ComponentViewModel CreateComponentViewModel(IGEComponent component)
        {
            ComponentViewModel c = null;
            switch (component.ComponentName)
            {
                case nameof(TransformComponent):
                    c = new TransformComponentViewModel(m_SelectedItem.GameObject);
                    break;
                case nameof(Animation):
                    c = new AnimationComponentViewModel(m_SelectedItem.GameObject,
                        m_factoryWrapper,
                        m_assemblyLoader);
                    break;
                case nameof(Animator):
                    c = new AnimatorComponentViewModel(m_SelectedItem.GameObject,
                        m_assemblyLoader, m_factoryWrapper);
                    break;
                case nameof(Sprite):
                    c = new SpriteComponentViewModel(m_SelectedItem.GameObject, m_factoryWrapper.ResourceLoader);
                    break;
                case nameof(RelativeTransformComponent):
                    c = new RelativeTransformViewModel(m_SelectedItem.GameObject);
                    break;
                default:
                    throw new Exception($"Unsupported component Type! Component: {component.ComponentName}");
            }
            return c;
        }

        #endregion
    }
}
