using SpaceAvenger.Game.Core.Animations.Explosions;
using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAvenger.Game.Core.Factions.F1.Corvettes
{
    public class F1Corvette : ExplosiveSpaceShipBase<Explosion3>
    {
        

        public F1Corvette() : base(Faction.F1)
        {

        }

        protected override void MoveBackward()
        {
            throw new NotImplementedException();
        }

        protected override void MoveForward()
        {
            throw new NotImplementedException();
        }

        protected override void MoveLeft()
        {
            throw new NotImplementedException();
        }

        protected override void MoveRight()
        {
            throw new NotImplementedException();
        }
    }
}
