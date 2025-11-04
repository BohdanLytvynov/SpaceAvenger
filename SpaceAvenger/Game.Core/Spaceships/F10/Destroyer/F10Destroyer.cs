using SpaceAvenger.Game.Core.Spaceships.Base;
using System.Numerics;
using System.Drawing;

namespace SpaceAvenger.Game.Core.Spaceships.F10.Destroyer
{
    public class F10Destroyer : SpaceShipBase
    {
        public F10Destroyer() : base(nameof(F10Destroyer))
        {
            
        }

        #region Methods
        public override void StartUp()
        {
            base.StartUp();

            transform.Position = new Vector2(100,200);
            Scale(new SizeF(0.5f, 0.5f));
            transform.Rotation = -90;
        }
        
        #endregion
    }
}
