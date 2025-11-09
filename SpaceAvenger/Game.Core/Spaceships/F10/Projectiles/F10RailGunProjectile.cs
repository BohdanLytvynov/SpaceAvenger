using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Game.Core.Spaceships.F10.Projectiles
{
    internal class F10RailGunProjectile : MapableObject
    {
        public F10RailGunProjectile() : base(nameof(F10RailGunProjectile))
        {
        }

        public override void StartUp()
        {
            Scale(new System.Drawing.SizeF(1, 1));
        }

        public override void Render(DrawingContext dc, Matrix parent = default)
        {
            base.Render(dc, parent);
        }
    }
}
