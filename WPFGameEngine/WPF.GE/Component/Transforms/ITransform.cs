using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.Transforms
{
    public interface ITransform : IGEComponent
    {
        public Vector2 ActualCenterPosition { get; set; }
        public Size ActualSize { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 CenterPosition { get; set; }
        public double Rotation { get; set; }
        public Size Scale { get; set; }
        Matrix3x3 GetLocalTransformMatrix();
    }
}
