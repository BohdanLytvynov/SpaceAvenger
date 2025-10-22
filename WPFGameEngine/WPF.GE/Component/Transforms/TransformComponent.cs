using System.Drawing;
using System.Numerics;
using System.Windows.Media;
using WPFGameEngine.Atributes;
using WPFGameEngine.Attributes.Editor;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Transforms
{
    [GEComponent]
    [VisibleInEditor(Name = nameof(TransformComponent))]
    public class TransformComponent : ComponentBase, ITransform
    {
        #region Fields

        #endregion

        #region Properties

        public Vector2 Position { get; set; }
        public Vector2 CenterPosition { get; set; }
        public double Rotation { get; set; }//Degree
        public SizeF Scale { get; set; }

        #endregion

        #region Ctor


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

        public Matrix GetLocalTransformMatrix(Vector2 center)
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
            matrix.Translate(Position.X, Position.Y);

            return matrix;
        }

        #endregion
    }
}
