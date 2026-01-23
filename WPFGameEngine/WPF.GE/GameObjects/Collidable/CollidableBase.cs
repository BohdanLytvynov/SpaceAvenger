using System.Windows.Media;
using WPFGameEngine.CollisionDetection.CollisionManager.Base;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.GameObjects.Collidable
{
    public abstract class CollidableBase : СacheableObject, ICollidable
    {
        public bool IsCollidable => Collider != null;

        private ICollider m_colliderComponent;

        public ICollider Collider
        {
            get
            {
                if (m_colliderComponent == null)
                    m_colliderComponent = GetComponent<ColliderComponent>(false);
                return m_colliderComponent;
            }
        }

        public CollisionLayer CollisionLayer { get; set; }

        public virtual void ProcessCollision(List<CollisionData>? collisionInfo)
        {
            (GameView as IColliderView).CollisionManager.ForceRemove(this.Id);
        }

        protected CollidableBase() : base()
        {
            
        }

        public override void Update()
        {
            base.Update();
            //To do move to the GetWorldCenter, it's impossible, cause we don't use transform component 
            //in a collider component and local matrix can't be calculated. And we use Vector math approach
            if (IsCollidable && Collider.CollisionShape != null)
            {
                var globMatrix = GetWorldTransformMatrix();
                Collider.ActualObjectSize = GetWorldScale(globMatrix);
                var leftUpperCorner = globMatrix.GetTranslate();//
                Collider.CollisionShape.Scale = Transform.Scale;
                Collider.CollisionShape.Scale.CheckNegativeSize();
                Collider.Basis = globMatrix.GetBasis();
                Collider.CollisionShape.Basis = Collider.Basis;
                Collider.CollisionShape.CenterPosition = leftUpperCorner +//
                    Collider.ActualCenterPosition;
                Collider.CollisionShape.CalculatePoints();
            }
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent)
        {
            base.Render(dc, parent);
            if (GESettings.DrawColliders
               && IsCollidable
               && Collider.CollisionShape != null)
            {
                Collider.CollisionShape.Render(dc);
            }
        }

        public override void ForceUpdateOfLazyProperties()
        {
            m_colliderComponent = null;
        }
    }
}
