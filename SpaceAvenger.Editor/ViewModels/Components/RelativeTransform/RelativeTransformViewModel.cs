using SpaceAvenger.Editor.ViewModels.Components.Transform;
using System.Drawing;
using System.Numerics;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;

namespace SpaceAvenger.Editor.ViewModels.Components.RelativeTransforms
{
    internal class RelativeTransformViewModel : TransformComponentViewModel
    {
        #region Fields
        private double m_XMax;
        private double m_YMax;
        #endregion

        #region Properties
        public double XMax 
        { get => m_XMax; set => Set(ref m_XMax, value); }

        public double MyProperty { get; set; }
        #endregion

        public RelativeTransformViewModel(IGameObject gameObject) : base(gameObject)
        {

        }

        protected override void UpdatePositionX(float x)
        {
            if (GameObject == null)
                return;

            var currentTransform = GameObject.GetComponent<RelativeTransformComponent>();
            if (currentTransform == null)
                return;

            var parentTexture = GameObject.Parent?.GetTexture();
            if (parentTexture == null)
                return;

            TransformComponent parentTransform = GameObject.Parent.GetTransformComponent();

            if (parentTransform == null)
                return;

            float parentActualWidth = (float)parentTexture.Width * parentTransform.Scale.Width;

            if(parentActualWidth == 0)
                return;

            float normalizedX = x / parentActualWidth;
            float oldY = currentTransform.Position.Y;
            currentTransform.Position = new Vector2(normalizedX, oldY);
        }

        protected override void UpdatePositionY(float y)
        {
            if (GameObject == null)
                return;

            var currentTransform = GameObject.GetComponent<RelativeTransformComponent>();
            if (currentTransform == null)
                return;

            var parentTexture = GameObject.Parent?.GetTexture();
            if (parentTexture == null)
                return;

            TransformComponent parentTransform = GameObject.Parent.GetTransformComponent();

            if (parentTransform == null)
                return;

            float parentActualHeight = (float)parentTexture.Height * parentTransform.Scale.Height;

            if (parentActualHeight == 0)
                return;

            float normalizedY = y / parentActualHeight;
            float oldX = currentTransform.Position.X;
            currentTransform.Position = new Vector2(oldX, normalizedY);
        }
    }
}
