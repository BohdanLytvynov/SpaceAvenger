using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.RelativeTransforms
{
    public interface IRelativeTransform : ITransform
    {
        public Size OriginalParentSize { get; set; }
    }
}
