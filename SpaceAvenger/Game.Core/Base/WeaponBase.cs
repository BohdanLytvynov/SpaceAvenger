using System.Numerics;
using WPFGameEngine.CollisionDetection.CollisionMatrixes;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class WeaponBase : MapableObject
    {
        public CollisionLayer ProjectilesCollisionLayer { get; set; }
        public float AimThreshold { get; set; }
        public float RotationSpeed { get; set; }
        public bool WeaponAimed { get; set; }
        public abstract void Shoot(Vector2 dir);
    }
}
