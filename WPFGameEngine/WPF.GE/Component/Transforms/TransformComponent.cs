using System.Numerics;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Extensions;
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
        /// <summary>
        /// Actual Position of the Center with respect to texture
        /// </summary>
        public Vector2 ActualCenterPosition { get; set; }
        /// <summary>
        /// Actual Size of the Texture after Scaling applied
        /// </summary>
        public Size ActualSize { get; set; }
        public override List<string> IncompatibleComponents => 
            new List<string>{ nameof(RelativeTransformComponent) };
        public Vector2 Position { get; set; }
        public Vector2 CenterPosition { get; set; }
        public double Rotation { get; set; }//Degree
        public Size Scale { get; set; }
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
            //Create I matrix, diagonal is 1
            Matrix3x3 matrix = new Matrix3x3();
            //Move to center of the texture
            matrix.Translate(ActualCenterPosition * -1);
            //Apply Rotation
            matrix.Rotate(Rotation);
            //Apply Translate in the World
            matrix.Translate(Position);
            //Move back to initial origin
            matrix.Translate(ActualCenterPosition);
            
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

        #endregion
    }
}
