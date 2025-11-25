using System.Numerics;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Components;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;

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
 
        public Size ActualParentSize { get; set; }

        public override Matrix3x3 GetLocalTransformMatrix()
        {
            //Create I matrix, diagonal is 1
            Matrix3x3 matrix = new Matrix3x3();
            //Move to center of the texture
            matrix.Translate(ActualCenterPosition * -1);
            //Apply Rotation
            matrix.Rotate(Rotation);
            //Apply Translate in the World with respect to parent
            matrix.Translate(new Vector2(ActualParentSize.Width * Position.X,
                ActualParentSize.Height * Position.Y));//Here Position has a normalized values
            //Move back to initial origin
            matrix.Translate(ActualCenterPosition);
            
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

        #region IClonable

        public override object Clone()
        {
            return new RelativeTransformComponent()
            {
                Position = Position,
                CenterPosition = CenterPosition,
                Rotation = Rotation,
                Scale = Scale,
            };
        }

        #endregion

    }
}
