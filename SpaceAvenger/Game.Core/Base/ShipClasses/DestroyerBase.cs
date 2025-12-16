using SpaceAvenger.Game.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
