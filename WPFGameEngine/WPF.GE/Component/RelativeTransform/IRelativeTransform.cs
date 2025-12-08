using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.RelativeTransforms
{
    public interface IRelativeTransform : ITransform
    {
        public bool EnableXAxisCompensation { get; set; }

        public bool EnableYAxisCompensation { get; set; }

        public Size ActualParentSize { get; set; }

        void XScaleCompensate(float value);

        void YScaleCompensate(float value);
    }
}
