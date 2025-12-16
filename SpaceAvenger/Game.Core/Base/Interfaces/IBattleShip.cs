using System.Collections.Generic;
using System.Numerics;

namespace SpaceAvenger.Game.Core.Base.Interfaces
{
    public interface IBattleShip
    {
        public bool WeaponsAimed { get; }
        public float DetectionDistance { get; set; }
        WeaponType WeaponType { get; }
        void ShootWeapons(Vector2 targetDir);
        void AimWeapons(Vector2 target, bool useThreshold = false);
        void SetWeaponType(WeaponType weaponType);
        IEnumerable<WeaponBase>? GetActiveWeapons();
    }
}
