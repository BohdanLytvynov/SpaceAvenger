using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel;
using SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel.GeometryConfigBase;
using SpaceAvenger.Editor.ViewModels.Options;
using System.Collections.ObjectModel;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Enums;
using WPFGameEngine.Extensions;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.Services.Interfaces;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;

namespace SpaceAvenger.Editor.ViewModels.Components.Collider
{
    internal class ColliderComponentViewModel : ComponentViewModel
    {
        #region Fields
        private bool m_init;
        private IFactoryWrapper m_factoryWrapper;
        private IAssemblyLoader m_assemblyLoader;
        private ObservableCollection<OptionsViewModel> m_geometryTypes;
        private ObservableCollection<GeometryConfigViewModelBase> m_geomConfig;
        private OptionsViewModel m_selectedGeometry;

        private double m_XMax;
        private double m_YMax;

        private double m_XRel;
        private double m_YRel;

        #endregion

        #region Properties
        public double XRel 
        { 
            get => m_XRel;
            set
            {
                Set(ref m_XRel, value);
                UpdateRelX(value);
            }
        }
        public double YRel 
        { 
            get=> m_YRel;
            set
            {
                Set(ref m_YRel, value);
                UpdateRelY(value);
            }
        }

        public double XMax 
        { 
            get => m_XMax;
            set 
            {
                Set(ref m_XMax, value);
            }
        }
        public double YMax 
        { 
            get => m_YMax;
            set
            {
                Set(ref m_YMax, value);
            }
        }
        public ObservableCollection<OptionsViewModel> GeometryTypes 
        { get => m_geometryTypes; set => m_geometryTypes = value; }

        public OptionsViewModel SelectedGeometry
        {
            get => m_selectedGeometry;
            set
            {
                Set(ref m_selectedGeometry, value);
                var shape = SelectedGeometry.FactoryName;
                if (string.IsNullOrEmpty(shape))
                    return;

                UpdateGeometry(shape);
            }
        }

        public ObservableCollection<GeometryConfigViewModelBase> GeomConfig 
        { get => m_geomConfig; set => Set(ref m_geomConfig, value); }
        #endregion

        #region Ctor

        public ColliderComponentViewModel(
            IGameObjectMock gameObject,
            IAssemblyLoader assemblyLoader,
            IFactoryWrapper factoryWrapper)
            : base(nameof(ColliderComponent), gameObject)
        {
            m_assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
            m_factoryWrapper = factoryWrapper ?? throw new ArgumentNullException(nameof(factoryWrapper));
            m_selectedGeometry = new OptionsViewModel();
            m_geometryTypes = new ObservableCollection<OptionsViewModel>();
            m_geomConfig = new ObservableCollection<GeometryConfigViewModelBase>();

            var types = m_assemblyLoader["WPFGameEngine"].GetTypes();

            foreach (var type in types)
            {
                var attr = type.GetAttribute<VisibleInEditor>();
                if (attr != null &&
                    attr.GetValue<GEObjectType>("GameObjectType") == GEObjectType.Geometry)
                {
                    m_geometryTypes.Add(new OptionsViewModel(
                        attr.GetValue<string>("DisplayName"),
                        attr.GetValue<string>("FactoryName")));
                }
            }

            LoadCurrentGameObjProperties();
            m_init = true;
        }

        #endregion

        #region Methods

        protected override void LoadCurrentGameObjProperties()
        {
            if (GameObject == null && !GameObject.IsCollidable) return;
            var collider = GameObject.Collider;

            var transform = GameObject.Transform;
            XMax = transform.ActualSize.Width;
            YMax = transform.ActualSize.Height;

            m_XRel = collider.Position.X * transform.ActualSize.Width;
            m_YRel = collider.Position.Y * transform.ActualSize.Height;

            var shape = GameObject.Collider.CollisionShape;

            if (shape == null) return;
            string shapeType = shape.GetType().Name;
            LoadCurrentGeometry(shapeType, shape);
            SelectedGeometry = new OptionsViewModel(shapeType, shapeType);
        }

        private void UpdateGeometry(string factoryName)
        {
            if (!m_init) return;

            GeomConfig.Clear();
            GeometryConfigViewModelBase geomConfig = null;
            IShape2D shape = GameObject.Collider.CollisionShape;
            switch (factoryName)
            {
                case nameof(Circle):
                    shape = m_factoryWrapper.CreateObject<Circle>();
                    geomConfig = new CircleConfigViewModel(shape);
                    break;
                case nameof(Rectangle):
                    shape = m_factoryWrapper.CreateObject<Rectangle>();
                    geomConfig = new RectangleConfigViewModel(shape); 
                    break;
                case nameof(Triangle):
                    shape = m_factoryWrapper.CreateObject<Triangle>();
                    geomConfig = new TriangleConfigViewModel(shape);
                    break;
                default:
                    throw new NotImplementedException();
            }
            GameObject.Collider.CollisionShape = shape;
            GeomConfig.Add(geomConfig);
        }

        public void LoadCurrentGeometry(string factoryName, IShape2D shape)
        {
            GeomConfig.Clear();
            GeometryConfigViewModelBase geomConfig = null;
            switch (factoryName)
            {
                case nameof(Circle):
                    geomConfig = new CircleConfigViewModel(shape);
                    break;
                case nameof(Rectangle):
                    geomConfig = new RectangleConfigViewModel(shape);
                    break;
                case nameof(Triangle):
                    geomConfig = new TriangleConfigViewModel(shape);
                    break;
                default:
                    throw new NotImplementedException();
            }
            GeomConfig.Add(geomConfig);
        }

        private void UpdateRelX(double value)
        { 
            if(GameObject == null) return;
            var collider = GameObject.Collider;
            if(collider == null) return;
            float oldY = collider.Position.Y;
            float x = (float)value / collider.ActualObjectSize.Width;
            collider.Position = new System.Numerics.Vector2 (x, oldY);
        }

        private void UpdateRelY(double value)
        { 
            if (GameObject == null) return;
            var collider = GameObject.Collider;
            if(collider == null) return;
            float oldX = collider.Position.X;
            float y = (float)value / collider.ActualObjectSize.Height;
            collider.Position = new System.Numerics.Vector2(oldX, y);
        }

        public override IGEComponent? GetComponent()
        {
            return GameObject?.GetComponent<ColliderComponent>(false);
        }
        #endregion
    }
}