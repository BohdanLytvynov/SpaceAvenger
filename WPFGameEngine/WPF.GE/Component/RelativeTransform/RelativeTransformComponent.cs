using System.Drawing;
using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Extensions;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Components;

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

        public override Matrix GetLocalTransformMatrix()
        {
            //Create I matrix, diagonal is 1
            Matrix matrix = Matrix.Identity;
            //Move to center of the texture
            matrix.Translate(-ActualCenterPosition.X, -ActualCenterPosition.Y);
            //Apply Rotation
            matrix.Rotate(Rotation);
            //Apply Translate in the World with respect to parent
            matrix.Translate(ActualParentSize.Width * Position.X,
                ActualParentSize.Height * Position.Y);//Here Position has a normalized values
            //Move back to initial origin
            matrix.Translate(ActualCenterPosition.X, ActualCenterPosition.Y);
            
            matrix.CheckMachineZero();
            return matrix;
        }

        public override RelativeTransformDto ToDto()
        {
            return new RelativeTransformDto()
            { 
                Position = Position,
                CenterPosition = CenterPosition,
                Scale = Scale,
                Rotation = Rotation,
            };
        }
        
    }
}
