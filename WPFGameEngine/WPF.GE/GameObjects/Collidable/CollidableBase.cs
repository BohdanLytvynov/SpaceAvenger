using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.CollisionDetection.RaycastManager;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Component.Collider.Base;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.GameObjects.Collidable
{
    public abstract class CollidableBase : RenderableBase, ICollidable
    {
        private IColliderComponentBase m_colliderComponent;

        public bool IsCollidable => ColliderComponent is ICollider;
        public bool IsRaycastable => ColliderComponent is IRaycastComponent;
        public CollisionLayer CollisionLayer { get; set; }

        public IColliderComponentBase ColliderComponent
        {
            get
            {
                if (m_colliderComponent == null)
                    m_colliderComponent = GetColliderComponent();
                return m_colliderComponent;
            }
        }

        public virtual void ProcessCollision(List<CollisionData>? collisionInfo)
        {
            (GameView as IColliderView).CollisionManager.ForceRemove(this.Id);
        }

        public virtual void ProcessHit(List<RaycastData> data)
        {
            (GameView as IColliderView).RaycastManager.ForceRemove(this.Id);
        }

        protected CollidableBase() : base()
        {
            
        }

        public override void Update()
        {
            base.Update();
            //To do move to the GetWorldCenter, it's impossible, cause we don't use transform component 
            //in a collider component and local matrix can't be calculated. And we use Vector math approach
            if (ColliderComponent is ICollider collider && collider.CollisionShape != null)
            {
                var globMatrix = GetWorldTransformMatrix();
                ColliderComponent.ActualObjectSize = GetWorldScale(globMatrix);
                var leftUpperCorner = globMatrix.GetTranslate();
                collider.CollisionShape.Scale = Transform.Scale;
                collider.CollisionShape.Scale.CheckNegativeSize();
                collider.Basis = globMatrix.GetBasis();
                collider.CollisionShape.Basis = ColliderComponent.Basis;
                collider.CollisionShape.CenterPosition = leftUpperCorner +
                    collider.ActualCenterPosition;
                collider.CollisionShape.CalculatePoints();
            }
            else if (ColliderComponent is IRaycastComponent raycast)
            {
                var globMatrix = GetWorldTransformMatrix();
                raycast.ActualObjectSize = GetWorldScale(globMatrix);
                var leftUpperCorner = globMatrix.GetTranslate();
                raycast.Basis = globMatrix.GetBasis();
                raycast.Update(leftUpperCorner + raycast.ActualCenterPosition);
            }
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent)
        {
            base.Render(dc, parent);
            if (!GESettings.DrawColliders)
                return;

            if (ColliderComponent is ICollider collider
               && collider.CollisionShape != null)
            {
                collider.CollisionShape.Render(dc);
            }
            else if (ColliderComponent is IRaycastComponent raycast)
            {
                var startPoint = new System.Windows.Point(
                        raycast.CurrentPosition.X,
                        raycast.CurrentPosition.Y
                        );
                var v = raycast.CurrentPosition + 
                    (raycast.Basis.X * raycast.ActualObjectSize.Width);
                var endPoint = new System.Windows.Point(
                    v.X, v.Y
                    );
                dc.DrawLine(new Pen(GESettings.ColliderPointFillBrush, 3)
                    ,startPoint, endPoint);

                dc.DrawEllipse(GESettings.ColliderPointFillBrush,
                    GESettings.ColliderPointPen,
                    new System.Windows.Point(raycast.CurrentPosition.X,
                    raycast.CurrentPosition.Y), 5, 5);

                dc.DrawEllipse(GESettings.ColliderPointFillBrush,
                    GESettings.ColliderPointPen,
                    endPoint, 2.5f, 2.5f);
            }
        }

        public override void ForceUpdateOfLazyProperties()
        {
            m_colliderComponent = null;
        }

        private IColliderComponentBase? GetColliderComponent()
        {
            var collider = GetComponent<ColliderComponent>(false);
            if(collider != null)
                return collider;

            var rayCaster = GetComponent<RaycastComponent>(false);
            if(rayCaster != null)
                return rayCaster;

            return null;
        }

        public void ResetRaycastPosition(Vector2 newPosition)
        {
            if (IsRaycastable)
                (ColliderComponent as IRaycastComponent).ResetPosition(newPosition);
        }
    }
}
