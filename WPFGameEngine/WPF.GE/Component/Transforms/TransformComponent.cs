using System.Numerics;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Dto.Components;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.Transforms
{
    [VisibleInEditor(FactoryName = nameof(TransformComponent),
        DisplayName = "Transform",
        GameObjectType = Enums.GEObjectType.Component)]
    public class TransformComponent : ComponentBase, ITransform
    {
        #region Properties

        public Size OriginalObjectSize { get; set; }

        public Vector2 TextureCenterPosition 
        { 
            get => new Vector2(
                OriginalObjectSize.Width * CenterPosition.X,
                OriginalObjectSize.Height * CenterPosition.Y);
        }

        public override List<string> IncompatibleComponents => 
            new List<string>{ nameof(RelativeTransformComponent) };

        public Vector2 Position { get; set; } //World position
        public Vector2 CenterPosition { get; set; } //Local Center Position
        public double Rotation { get; set; } //Degree
        public Size Scale { get; set; } //Scale

        #endregion

        #region Ctor

        public TransformComponent(string componentName) : base(componentName)
        {
            
        }

        public TransformComponent() : base(nameof(TransformComponent)) 
        {
            Position = new Vector2(0, 0);
            Rotation = 0;
            Scale = new Size(1, 1);
            CenterPosition = new Vector2(0.5f, 0.5f);
        }

        public TransformComponent(Vector2 position, Vector2 centerPosition, double rotation, Size scale) 
            : base(nameof(TransformComponent))
        {
            Position = position;
            CenterPosition = centerPosition;
            Rotation = rotation;
            Scale = scale;
        }
        #endregion

        #region Methods
       
        public virtual Matrix3x3 GetLocalTransformMatrix()
        {
            //Identity matrix
            Matrix3x3 matrix = new Matrix3x3();
            //Move to the new Center Origin(back and up)
            matrix.Translate(TextureCenterPosition * -1);
            //Apply Local Scale
            matrix.Scale(Scale);
            //Apply Local Rotation
            matrix.Rotate(Rotation);
            //Compensate and restore center position
            matrix.Translate(TextureCenterPosition);
            //Apply Translation in the World
            matrix.Translate(Position);

            matrix.CheckMachineZero();
            return matrix;
        }
        

        #region IConvertToDto

        public override TransformDto ToDto() =>
            new TransformDto()
            {
                Position = Position,
                Rotation = Rotation,
                Scale = Scale,
                CenterPosition = CenterPosition,
            };

        #endregion

        #endregion

        #region IClonable

        public override object Clone()
        {
            return new TransformComponent()
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
