using SpaceAvenger.Game.Core.Base;
using SpaceAvenger.Game.Core.Base.Interfaces;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.AI
{
    /// <summary>
    /// AI Control Module for the Corvete Type
    /// </summary>
    public class CorvetteControlModule : SpaceShipControlModule
    {
        public CorvetteControlModule() : base()
        {

        }

        public override void Process(IGameObject gameObject)
        {
            var corvette = gameObject as IBattleShip;

            if (corvette == null) return;
            //Switch to the Primary Weapons, Corvetes has ligth canons
            corvette.SetWeaponType(WeaponType.Primary);

            base.Process(gameObject);
        }
    }
}
