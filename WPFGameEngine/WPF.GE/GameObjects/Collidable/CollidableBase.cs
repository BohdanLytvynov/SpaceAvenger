
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.GameObjects.Renderable;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Settings;

namespace WPFGameEngine.WPF.GE.GameObjects.Collidable
{
    public abstract class CollidableBase : RenderableBase, ICollidable
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

        public virtual void ProcessCollision(List<IGameObject>? collisionInfo)
        {
        
        }

        protected CollidableBase() : base()
        {
            
        }

        public override void Update()
        {
            base.Update();
            if (IsCollidable && Collider.CollisionShape != null)
            {
                Collider.ActualObjectSize = Transform.ActualSize;
                var globMatrix = GetWorldTransformMatrix();
                var leftUpperCorner = globMatrix.GetTranslate();
                Collider.CollisionShape.Scale = Transform.Scale;
                Collider.CollisionShape.Scale.CheckNegativeSize();
                Collider.Basis = globMatrix.GetBasis();
                Collider.CollisionShape.Basis = Collider.Basis;
                Collider.CollisionShape.CenterPosition = leftUpperCorner +
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
    }
}
