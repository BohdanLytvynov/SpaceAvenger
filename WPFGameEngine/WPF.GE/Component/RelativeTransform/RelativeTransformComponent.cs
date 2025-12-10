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
 
        public Size OriginalParentSize { get; set; }
        public bool EnableXAxisCompensation { get; set; }
        public bool EnableYAxisCompensation { get; set; }

        public override Matrix3x3 GetLocalTransformMatrix()
        {
            //Create I matrix, diagonal is 1
            Matrix3x3 matrix = new Matrix3x3();
            //Move to center of the texture
            matrix.Translate(TextureCenterPosition * -1);
            ////Apply Scale
            matrix.Scale(Scale);
            //Apply Rotation
            matrix.Rotate(Rotation);
            //Move back to initial origin
            matrix.Translate(TextureCenterPosition);
            //Apply Translate in the World with respect to parent
            matrix.Translate(new Vector2(OriginalParentSize.Width * Position.X,
                OriginalParentSize.Height * Position.Y));//Here Position has a normalized values

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
                EnableXAxisCompensation = EnableXAxisCompensation,
                EnableYAxisCompensation = EnableYAxisCompensation
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

        public void XScaleCompensate(float value)
        {
            if (value == 0 || OriginalParentSize.Width == 0) return;
            float actPos = Position.X - (value / OriginalParentSize.Width);
            var oldy = Position.Y;
            Position = new Vector2(actPos, oldy);
        }

        public void YScaleCompensate(float value)
        {
            if (value == 0 || OriginalParentSize.Height == 0) return;
            float actPos = Position.Y - (value / OriginalParentSize.Height);
            var oldX = Position.X;
            Position = new Vector2(oldX, actPos);
        }

        #endregion

    }
}
