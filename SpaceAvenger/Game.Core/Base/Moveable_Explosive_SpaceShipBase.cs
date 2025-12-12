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

        public override void StartUp(IGameObjectViewHost viewHost, IGameTimer gameTimer)
        {
            base.StartUp(viewHost, gameTimer);

            var allEngines = GetAllChildrenOfType<TJetType>();

            m_mainEngines = allEngines.Where(e => e.UniqueName.Equals("Jet_Main_L")
            || e.UniqueName.Equals("Jet_Main_R")).Select(e => e as TJetType);

            m_leftAccelerators = allEngines.Where(e => e.UniqueName.Equals("Jet_Accelerator_L_1")
            || e.UniqueName.Equals("Jet_Accelerator_L_2")).Select(e => e as TJetType);

            m_rightAccelerators = allEngines.Where(e => e.UniqueName.Equals("Jet_Accelerator_R_1")
            || e.UniqueName.Equals("Jet_Accelerator_R_2")).Select(e => e as TJetType);
        }

        protected Moveable_Explosive_SpaceShipBase(Faction faction) : base(faction)
        {

        }

        protected override void StopAllEngines()
        {
            StopAll(m_mainEngines);
            StopAll(m_leftAccelerators);
            StopAll(m_rightAccelerators);
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
            foreach (var item in m_mainEngines)
            {
                item.Start();
            }
        }

        protected override void MoveBackward()
        {
            foreach (var item in m_mainEngines)
            {
                item.Stop();
            }
        }

        protected override void MoveLeft()
        {
            foreach (var item in m_rightAccelerators)
            {
                item.Start();
            }
        }

        protected override void MoveRight()
        {
            foreach (var item in m_leftAccelerators)
            {
                item.Start();
            }
        }
    }
}