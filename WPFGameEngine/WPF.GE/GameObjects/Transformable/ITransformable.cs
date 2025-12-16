using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.GameObjects.Transformable
{
    public interface ITransformable : IGameObject
    {
        ITransform Transform { get; }

        void Translate(Vector2 position);
        void Translate(Vector2 dir, float speed, double deltaTime);
        /// <summary>
        /// Rotates an object
        /// </summary>
        /// <param name="angle">In Degrees</param>
        void Rotate(double angle);
        void Scale(Size newScale);
        Matrix3x3 GetWorldTransformMatrix();
        /// <summary>
        /// Returns Center of the object in world Coordinates
        /// </summary>
        /// <returns></returns>
        Vector2 GetWorldCenter(Matrix3x3 worldmatrix);
        Size GetWorldScale(Matrix3x3 worldMatrix);
        Size GetWorldScale();
        void LookAt(Vector2 position, double rotSpeed, double deltaTime, Matrix3x3 worldMatrix);
        bool LookAtWithTreshold(Vector2 position, double rotSpeed, 
            double deltaTime, Matrix3x3 worldMatrix, double threshold);
        Vector2 GetDirection(Vector2 position, Matrix3x3 worldMatrix);
    }
}
