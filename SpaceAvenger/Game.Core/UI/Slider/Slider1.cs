using SpaceAvenger.Game.Core.UI.Base;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Game.Core.UI.Slider
{
    public class Slider1 : UIElementBase
    {
        private float m_value;
        public float Max { get; set; }

        public Slider1() : base(nameof(UIElementBase))
        {
        }

        public virtual void Update(float value)
        { 
             
            m_value = value;
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent)
        {


            base.Render(dc, parent);
        }
    }
}
