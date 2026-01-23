using System.Numerics;
using WPFGameEngine.Extensions;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Math.Basis;
using WPFGameEngine.WPF.GE.Math.Matrixes;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace WPFGameEngine.WPF.GE.GameObjects.Transformable
{
    public abstract class TransformableBase : GameObject, ITransformable
    {
        private ITransform m_transform;

        public ITransform Transform
        {
            get
            {
                if (m_transform == null)
                    m_transform = GetTransformComponent();
                return m_transform;
            }
        }

        protected TransformableBase() : base()
        {
            
        }

        public void Scale(Size newScale)
        {
            Transform.Scale = newScale;
        }

        private TransformComponent GetTransformComponent()
        {
            if (!IsChild)
            {
                return GetComponent<TransformComponent>(false);
            }
            else
            {
                return GetComponent<RelativeTransformComponent>(false);
            }
        }

        public void Translate(Vector2 position)
        {
            Transform.Position = position;
        }

        public void Translate(Vector2 dir, float speed, double deltaTime)
        {
            Translate(
                Transform.Position +
                dir.Multiply(speed * deltaTime));
        }

        public void Rotate(double angle)
        {
            Transform.Rotation = angle;
        }

        public Matrix3x3 GetWorldTransformMatrix()
        {
            Matrix3x3 m = Matrix3x3.Identity;
            GetGlobalMatrixRec(this, ref m);
            return m;
        }

        #region Static Methods
        public static void RemoveObject(Func<IGameObject, bool> predicate, List<IGameObject> world, bool recursive = false)
        {
            foreach (var o in world)
            {
                if (predicate(o))
                {
                    world.Remove(o);
                    return;
                }
                else if (recursive)
                {
                    bool res = o.RemoveChild(predicate, recursive);

                    if (res)
                        return;
                }
            }
        }

        private static void GetGlobalMatrixRec(IGameObject obj, ref Matrix3x3 matrix)
        {
            if (obj == null)
                return;

            if (!(obj is ITransformable transformable))
                return;

            var t = transformable.Transform;
            if (t != null)
            {
                var m = t.GetLocalTransformMatrix();
                matrix *= m;

                GetGlobalMatrixRec(obj.Parent, ref matrix);
            }
        }

        public static Matrix3x3 GetWorldTransformMatrix(IGameObject obj)
        {
            Matrix3x3 m = new Matrix3x3();
            GetGlobalMatrixRec(obj, ref m);
            return m;
        }

        #endregion

        public Vector2 GetWorldCenter(Matrix3x3 worldMatrix)
        {
            return worldMatrix.GetCenter(Transform.TextureCenterPosition);
        }

        public Size GetWorldScale()
        {
            Size scaleFactors = GetWorldTransformMatrix().GetScaleFactors();
            return new Size(
                Transform.OriginalObjectSize.Width * scaleFactors.Width,
                Transform.OriginalObjectSize.Height * scaleFactors.Height);
        }

        public Size GetWorldScale(Matrix3x3 worldMatrix)
        {
            Size scaleFactors = worldMatrix.GetScaleFactors();
            return new Size(
                Transform.OriginalObjectSize.Width * scaleFactors.Width,
                Transform.OriginalObjectSize.Height * scaleFactors.Height);
        }

        public Vector2 GetDirection(Vector2 position, Matrix3x3 worldMatrix)
        {
            var objW = GetWorldCenter(worldMatrix);
            return Vector2.Normalize(position - objW);
        }

        public Basis2D GetBasis()
        {
            return GetWorldTransformMatrix().GetBasis();
        }

        public void LookAt(Vector2 position, double rotSpeed, double deltaTime, Matrix3x3 worldMatrix)
        {
            //Get Dir Vector to Target
            var dir = GetDirection(position, worldMatrix);
            //Check that we are not in bounds of target
            if (dir.LengthSquared() < 0.0001)
                return;
            //Get Local Basis -> Xl, Yl
            var basis = GetBasis();
            //Calculate angle to rotate to
            var angle = dir.GetAngleDeg(basis.X);
            //Decide the hemicircle of rotation according to Yl
            double sign = Vector2.Dot(dir, basis.Y) < 0 ? -1 : 1;
            //Get Current Angle
            double currAngle = Transform.Rotation;
            //Calculate destination angle
            double destAngle = currAngle + sign * angle;
            //LERP
            //Get short rotation Way
            double diff = destAngle - currAngle;
            //Clamp between -180, 180, so we can operate using hemicirles
            while (diff > 180) diff -= 360;
            while (diff < -180) diff += 360;
            //Claculate rotation Step independent to FPS
            double step = diff * rotSpeed * deltaTime;
            //Get new Angle
            double newAngle = currAngle + step;
            //Apply new rotation
            Rotate(newAngle);
        }

        public bool LookAtWithTreshold(Vector2 position, 
            double rotSpeed, double deltaTime, Matrix3x3 worldMatrix,
            double threshold)
        {
            bool isAimed = false;

            //Get Dir Vector to Target
            var dir = GetDirection(position, worldMatrix);
            //Check that we are not in bounds of target
            if (dir.LengthSquared() < 0.0001)
                return isAimed;
            //Get Local Basis -> Xl, Yl
            var basis = GetBasis();
            //Calculate angle to rotate to
            var angle = dir.GetAngleDeg(basis.X);
            //Decide the hemicircle of rotation according to Yl
            double sign = Vector2.Dot(dir, basis.Y) < 0 ? -1 : 1;
            //Get Current Angle
            double currAngle = Transform.Rotation;
            //Calculate destination angle
            double destAngle = currAngle + sign * angle;
            //LERP
            //Get short rotation Way
            double diff = destAngle - currAngle;
            //Clamp between -180, 180, so we can operate using hemicirles
            while (diff > 180) diff -= 360;
            while (diff < -180) diff += 360;

            isAimed = System.Math.Abs(diff) <= threshold;

            if (isAimed)
            {
                Transform.Rotation = destAngle;
                return true;
            }

            //Claculate rotation Step independent to FPS
            double step = diff * rotSpeed * deltaTime;
            //Get new Angle
            double newAngle = currAngle + step;

            if (System.Math.Sign(step) != System.Math.Sign(diff) 
                || System.Math.Abs(step) >= System.Math.Abs(diff))
            {
                newAngle = destAngle;
                isAimed = true;
            }

            //Apply new rotation
            Rotate(newAngle);

            return isAimed;
        }

        public override void ForceUpdateOfLazyProperties()
        {
            m_transform = null;
        }
    }
}
