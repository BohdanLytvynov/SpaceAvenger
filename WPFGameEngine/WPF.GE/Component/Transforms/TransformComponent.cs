using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.Extensions;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Dto.Components;

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
        public SizeF ActualSize { get; set; }
        public override List<string> IncompatibleComponents => 
            new List<string>{ nameof(RelativeTransformComponent) };
        public virtual Vector2 Position { get; set; }
        public Vector2 CenterPosition { get; set; }
        public double Rotation { get; set; }//Degree
        public SizeF Scale { get; set; }
        #endregion

        #region Ctor

        public TransformComponent(string componentName) : base(componentName)
        {
            
        }

        public TransformComponent() : base(nameof(TransformComponent)) 
        {
            Position = new Vector2(0, 0);
            Rotation = 0;
            Scale = new SizeF(1, 1);
            CenterPosition = new Vector2(0.5f, 0.5f);
        }

        public TransformComponent(Vector2 position, Vector2 centerPosition, double rotation, SizeF scale) 
            : base(nameof(TransformComponent))
        {
            Position = position;
            CenterPosition = centerPosition;
            Rotation = rotation;
            Scale = scale;
        }
        #endregion

        #region Methods

        public virtual Matrix GetLocalTransformMatrix()
        {
            //Create I matrix, diagonal is 1
            Matrix matrix = Matrix.Identity;
            //Move to center of the texture
            matrix.Translate(-ActualCenterPosition.X, -ActualCenterPosition.Y);
            //Apply scale
            matrix.Scale(Scale.Width, Scale.Height);
            //Apply Rotation
            matrix.Rotate(Rotation);
            //Move back to initial origin
            matrix.Translate(ActualCenterPosition.X, ActualCenterPosition.Y);
            //Apply Translate in the World
            matrix.Translate(Position.X, Position.Y);

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
    }
}
