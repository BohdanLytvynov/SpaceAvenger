using SpaceAvenger.Game.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using WPFGameEngine.GameViewControl;
using WPFGameEngine.Timers.Base;

namespace SpaceAvenger.Game.Core.Base
{
    public abstract class Moveable_Explosive_SpaceShipBase<TJetType, TExplosion>
        : ExplosiveSpaceShipBase<TExplosion>
        where TExplosion : ExplosionBase
        where TJetType : JetBase
    {
        private IEnumerable<TJetType?>? m_mainEngines;
        private IEnumerable<TJetType?>? m_rightAccelerators;
        private IEnumerable<TJetType?>? m_leftAccelerators;

        public abstract List<string> MainEnginesNames 
        { get; }

        public abstract List<string> RightAcceleratorsNames 
        { get; }

        public abstract List<string> LeftAcceleratorsNames 
        { get; }

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);

            var allEngines = GetAllChildrenOfType<TJetType>();

            m_leftAccelerators = allEngines.Where(e => LeftAcceleratorsNames.Contains(e.UniqueName!));

            m_rightAccelerators = allEngines.Where(e => RightAcceleratorsNames.Contains(e.UniqueName!));

            m_mainEngines = allEngines.Where(e => MainEnginesNames.Contains(e.UniqueName!));
        }

        protected Moveable_Explosive_SpaceShipBase(Faction faction) : base(faction)
        {

        }

        protected override void StopAllEngines()
        {
            StopAll(m_mainEngines!);
            StopAll(m_leftAccelerators!);
            StopAll(m_rightAccelerators!);
        }

        private void StopAll(IEnumerable<JetBase>? jets)
        {
            if (jets == null) return;

            foreach (var jet in jets)
            {
                jet.Stop();
            }
        }

        protected override void MoveForward()
        {
            foreach (var item in m_mainEngines!)
            {
                item.Start();
            }
        }

        protected override void MoveBackward()
        {
            foreach (var item in m_mainEngines!)
            {
                item.Stop();
            }
        }

        protected override void MoveLeft()
        {
            foreach (var item in m_rightAccelerators!)
            {
                item.Start();
            }
        }

        protected override void MoveRight()
        {
            foreach (var item in m_leftAccelerators!)
            {
                item.Start();
            }
        }
    }
}