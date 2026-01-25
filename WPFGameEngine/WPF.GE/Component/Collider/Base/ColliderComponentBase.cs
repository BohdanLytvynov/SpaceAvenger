using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.Collider.Base
{
    public abstract class ColliderComponentBase : ComponentBase, IColliderComponentBase
    {
        #region Properties

        public bool CollisionEnabled { get; private set; }
        public Size ActualObjectSize { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 ActualCenterPosition
        {
            get
            {
                var actX = Position.X * ActualObjectSize.Width;
                var actY = Position.Y * ActualObjectSize.Height;
                var locX = Basis.X * actX;
                var locY = Basis.Y * actY;
                return locX + locY;
            }
        }
        public Basis2D Basis { get; set; }

        #endregion

        #region Ctor

        protected ColliderComponentBase(string componentName) : base(componentName)
        {
            EnableCollision();
        }

        #endregion

        #region Methods

        public void DisableCollision()
        {
            CollisionEnabled = false;
        }
        public void EnableCollision()
        {
            CollisionEnabled = true;
        }
        #endregion
    }
}
