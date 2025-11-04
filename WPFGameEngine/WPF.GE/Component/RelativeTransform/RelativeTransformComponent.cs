using System.Drawing;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Transforms;

namespace WPFGameEngine.WPF.GE.Component.RelativeTransforms
{
    [VisibleInEditor(DisplayName = "Relative Transform", 
        FactoryName = nameof(RelativeTransformComponent), 
        GameObjectType = Enums.GEObjectType.Component)]
    public class RelativeTransformComponent : TransformComponent, IRelativeTransform
    {
        public RelativeTransformComponent() : base(nameof(RelativeTransformComponent))
        {
            
        }

        public override List<string> IncompatibleComponents => 
            new List<string> { nameof(TransformComponent) };

        public SizeF ActualParentSize { get; set; }
    }
}
