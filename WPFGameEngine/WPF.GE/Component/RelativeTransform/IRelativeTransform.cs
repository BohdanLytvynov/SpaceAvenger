using System.Drawing;
using WPFGameEngine.WPF.GE.Component.Transforms;

namespace WPFGameEngine.WPF.GE.Component.RelativeTransforms
{
    public interface IRelativeTransform : ITransform
    {
        public SizeF ActualParentSize { get; set; }
    }
}
