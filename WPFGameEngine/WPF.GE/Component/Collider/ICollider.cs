using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    public interface ICollider : IGEComponent
    {
        public bool CollisionEnabled { get; }
        public Basis2D Basis { get; set; }
        /// <summary>
        /// Actual Size of the object with Collider
        /// </summary>
        public Size ActualObjectSize { get; set; }
        public Vector2 Position { get; set; }//Normalized Collider Position
        /// <summary>
        /// Actual Center Position Relative to GameObject according to it's Basis
        /// </summary>
        public Vector2 ActualCenterPosition { get; }
        IShape2D CollisionShape { get; set; }
        void EnableCollision();
        void DisableCollision();
        void ResolveCollision();
    }
}
