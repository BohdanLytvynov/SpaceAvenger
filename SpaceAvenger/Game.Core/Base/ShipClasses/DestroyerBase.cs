using SpaceAvenger.Game.Core.Enums;

namespace SpaceAvenger.Game.Core.Base.ShipClasses
{
    public abstract class DestroyerBase<TPrimWeapons, TJet, TExplosion>
        : BattleShipBase<TPrimWeapons, TJet, TExplosion>
        where TJet : JetBase
        where TExplosion : ExplosionBase
        where TPrimWeapons : WeaponBase
    {
        protected DestroyerBase(Faction faction) : base(faction)
        {
        }
    }
}
