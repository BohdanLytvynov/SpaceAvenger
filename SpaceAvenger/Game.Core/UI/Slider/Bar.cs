using SpaceAvenger.Extensions.Math;
using SpaceAvenger.Game.Core.UI.Base;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace SpaceAvenger.Game.Core.UI.Slider
{
    public enum GrowStartPosition
    { 
        Start = 0,
        End
    }
    public class Bar : UIElementBase
    {
        private float m_value;
        private float m_Max;
        public float Max 
        {
            get => m_Max;
            set
            {
                if (value == 0)
                    m_Max = 1;
                m_Max = value;
            }
        }
        public GrowStartPosition BarGrowStart { get; set; }
        public Brush Low { get; set; }
        public Brush Medium { get; set; }
        public Brush Full { get; set; }

        public Bar() : base(nameof(UIElementBase))
        {
        }

        public virtual void Update(float value)
        {
            m_value = value;
        }

        private Brush GetBrush(float value)
        {
            if (value >= 0f && value < 0.25f)
                return Low;
            else if(value >= 0.25f && value < 0.75f)
                return Medium;
            else
                return Full;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Render(DrawingContext dc, Matrix3x3 parent)
        {
            switch (BarGrowStart)
            {
                case GrowStartPosition.Start:
                    Transform.Rotation = 0;
                    break;
                case GrowStartPosition.End:
                    Transform.Rotation = 90;
                    break;
            }

            var m = Transform.GetLocalTransformMatrix();
            float normValue = m_value / Max;
            Brush brush = GetBrush(normValue);
            m *= parent;
            var wm = Matrix.Identity;
            wm.FromGEMatrix(m);

            dc.PushTransform(new MatrixTransform(wm));

            dc.DrawRectangle(brush, new Pen() { Brush = Brushes.Transparent },
                        new System.Windows.Rect(0, 0,
                        Transform.ActualSize.Width * normValue, Transform.ActualSize.Height));

            dc.Pop();

            base.Render(dc, parent);
        }
    }
}
