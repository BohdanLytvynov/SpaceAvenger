using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.CollisionDetection.RaycastManager;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.GameObjects.Raycastable
{
    public abstract class RaycastableBase : СacheableObject, IRaycastable
    {
        private IRaycastComponent m_raycastComponent;

        public IRaycastComponent RaycastComponent
        {
            get 
            {
                if (m_raycastComponent == null)
                    m_raycastComponent = GetComponent<RaycastComponent>(false);

                return m_raycastComponent;
            }
        }

        public bool IsRaycastable { get => RaycastComponent != null; }
        public CollisionLayer CollisionLayer { get; set; }

        protected RaycastableBase() : base()
        {
            
        }

        public override void Update()
        {
            base.Update();
            //TO DO move to the GetWorldCenter, it's impossible, cause we don't use transform component 
            //in a collider component and local matrix can't be calculated. And we use Vector math approach
            if (IsRaycastable)
            {
                var globMatrix = GetWorldTransformMatrix();
                RaycastComponent.ActualObjectSize = GetWorldScale(globMatrix);
                var leftUpperCorner = globMatrix.GetTranslate();
                RaycastComponent.Basis = globMatrix.GetBasis();
                RaycastComponent.Position = leftUpperCorner;
                RaycastComponent.Update();
            }
        }

        public virtual void ProcessHit(List<RaycastData> data)
        {
            (GameView as IColliderView).RaycastManager.ForceRemove(this.Id);
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent)
        {
            base.Render(dc, parent);
            if (GESettings.DrawColliders)
            {
                dc.DrawEllipse(GESettings.ColliderFillBrush, GESettings.ColliderPointPen,
                    new System.Windows.Point(RaycastComponent.ActualCenterPosition.X,
                    RaycastComponent.ActualCenterPosition.Y), 5, 5);
                var startPoint = new System.Windows.Point(
                        RaycastComponent.ActualCenterPosition.X,
                        RaycastComponent.ActualCenterPosition.Y
                        );
                var v = RaycastComponent.Basis.X * 5f;
                var endPoint = new System.Windows.Point(
                    v.X, v.Y
                    );
                dc.DrawLine(GESettings.ColliderPointPen, startPoint,endPoint);
            }
        }

        public override void ForceUpdateOfLazyProperties()
        {
            m_raycastComponent = null;
        }
    }
}
