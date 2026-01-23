using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.Collider.Base
{
    public interface IColliderComponentBase : IGEComponent
    {
        /// <summary>
        /// Actual Size of the object with Collider
        /// </summary>
        Size ActualObjectSize { get; set; }
        /// <summary>
        /// Normalized Collider Position in range [0 ; 1], set in Editor
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        /// Basis for center position calculation
        /// </summary>
        public Basis2D Basis { get; set; }
        /// <summary>
        /// Local center position (Calculated using Vector Math)
        /// </summary>
        public Vector2 ActualCenterPosition { get; }
        /// <summary>
        /// Is Collision Enabled
        /// </summary>
        bool CollisionEnabled { get; }
        /// <summary>
        /// Is Collision Resolved
        /// </summary>
        bool CollisionResolved { get; }
        /// <summary>
        /// Enable Collision
        /// </summary>
        void EnableCollision();
        /// <summary>
        /// Disable Collision
        /// </summary>
        void DisableCollision();
        /// <summary>
        /// Resolve Collision
        /// </summary>
        void ResolveCollision();
    }
}
