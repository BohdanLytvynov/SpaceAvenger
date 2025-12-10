using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.Component.Transforms
{
    public interface ITransform : IGEComponent
    {
        /// <summary>
        /// Coordinates of the Center of the Texture in a Local Space
        /// </summary>
        public Vector2 TextureCenterPosition { get; }
        /// <summary>
        /// Original size of the object(Texture.Width, Texture.Height)
        /// </summary>
        public Size OriginalObjectSize { get; set; }
        /// <summary>
        /// Actual size of the Object after all scales Applied
        /// </summary>
        public Size ActualSize { get; }
        /// <summary>
        /// World Position Of the Object
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Normalized Center Position
        /// </summary>
        public Vector2 CenterPosition { get; set; }
        /// <summary>
        /// Object Rotation in a Local Space
        /// </summary>
        public double Rotation { get; set; }
        /// <summary>
        /// Object Scale Factors in a Local Space
        /// </summary>
        public Size Scale { get; set; }
        /// <summary>
        /// Gets Local Transform Matrix for the current Object
        /// </summary>
        /// <returns></returns>
        Matrix3x3 GetLocalTransformMatrix();
    }
}
