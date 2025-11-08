using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Math.Basis;

namespace WPFGameEngine.WPF.GE.Component.Transforms
{
    public interface ITransform : IGEComponent
    {
        public Vector2 ActualCenterPosition { get; set; }
        public SizeF ActualSize { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 CenterPosition { get; set; }

        public double Rotation { get; set; }

        public SizeF Scale { get; set; }

        Matrix GetLocalTransformMatrix();
    }
}
