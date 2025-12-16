using SpaceAvenger.Game.Core.AI;
using SpaceAvenger.Game.Core.Enums;
using System.Collections.Generic;
using System.Numerics;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Base.ShipClasses
{
    public abstract class CorvetteBase<TPrimWeapons, TJet, TExplosion>
        : BattleShipBase<TPrimWeapons, TJet, TExplosion>
        where TPrimWeapons : WeaponBase
        where TJet : JetBase
        where TExplosion : ExplosionBase
    {
        protected CorvetteBase(Faction faction) : base(faction)
        {
            var ai = new CorvetteControlModule();
            ai.MoveForward = MoveForward;
            ai.MoveBackward = MoveBackward;
            ai.MoveLeft = MoveLeft;
            ai.MoveRight = MoveRight;
            ai.GetTime = GetTime;

            AIModule = ai;
            
        }
    }
}
