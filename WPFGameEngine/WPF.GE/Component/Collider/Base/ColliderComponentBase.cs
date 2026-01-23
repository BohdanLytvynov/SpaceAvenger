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
        public bool CollisionResolved { get; private set; }
        public Size ActualObjectSize { get; set; }
        public Vector2 Position { get; set; }
        public abstract Vector2 ActualCenterPosition
        {
            get;
        }
        public Basis2D Basis { get; set; }

        #endregion

        #region Ctor

        protected ColliderComponentBase(string componentName) : base(componentName)
        {
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
        public void ResolveCollision()
        {
            CollisionResolved = true;
        }

        #endregion
    }
}
