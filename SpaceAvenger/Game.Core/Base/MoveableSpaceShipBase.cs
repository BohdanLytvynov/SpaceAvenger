using SpaceAvenger.Game.Core.Factions.F10.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class MoveableSpaceShipBase<JetType, TExplosion> 
    {
        private IEnumerable<F10Jet?> m_mainEngines;
        private IEnumerable<F10Jet?> m_rightAccelerators;
        private IEnumerable<F10Jet?> m_leftAccelerators;

        protected MoveableSpaceShipBase()
        {
            
        }
    }
}
