using System.Drawing;
using System.Numerics;
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

        public override Matrix GetLocalTransformMatrix(Vector2 center)
        {
            //Create I matrix, diagonal is 1
            Matrix matrix = Matrix.Identity;
            //Move to center of the texture
            matrix.Translate(-center.X, -center.Y);
            //Apply scale
            matrix.Scale(Scale.Width, Scale.Height);
            //Apply Rotation
            matrix.Rotate(Rotation);
            //Move back to initial origin
            matrix.Translate(center.X, center.Y);
            //Apply Translate in the World
            matrix.Translate(ActualParentSize.Width*Position.X, 
                ActualParentSize.Height*Position.Y);//Here Position has a normalized values
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
