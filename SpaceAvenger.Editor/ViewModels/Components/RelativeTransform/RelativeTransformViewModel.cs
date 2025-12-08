using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Transform;
using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects.Transformable;

namespace SpaceAvenger.Editor.ViewModels.Components.RelativeTransforms
{
    internal class RelativeTransformViewModel : TransformComponentViewModel
    {
        #region Fields
        private double m_XMax;
        private double m_YMax;

        private double m_XMin;
        private double m_YMin;

        private bool m_EnableXScaleCompensation;
        private bool m_EnableYScaleCompensation;
        #endregion

        #region Properties
        public double XMax
        { get => m_XMax; set => Set(ref m_XMax, value); }

        public double YMax
        { get => m_YMax; set => Set(ref m_YMax, value); }

        public double XMin 
        { get => m_XMin; set => Set(ref m_XMin, value); }

        public double YMin 
        { get => m_YMin; set => Set(ref m_YMin, value); }

        public bool EnableXScaleCompensation 
        { 
            get => m_EnableXScaleCompensation;
            set
            { 
                Set(ref m_EnableXScaleCompensation, value);
                UpdateEnableXScaleCompensation(value);
            }
        }

        public bool EnableYScaleComensation 
        { 
            get => m_EnableYScaleCompensation;
            set 
            {
                Set(ref m_EnableYScaleCompensation, value);
                UpdateEnableYScaleCompensation(value);
            }
        }
        #endregion

        public RelativeTransformViewModel(IGameObjectMock gameObject) : 
            base(nameof(RelativeTransformComponent), gameObject)
        {
            LoadCurrentGameObjProperties();
            m_init = true;
        }

        protected override void LoadCurrentGameObjProperties()
        {
            if (GameObject != null)
            {
                var t = GameObject.Transform;
                var pt = (GameObject.Parent as ITransformable)?.Transform;
                var texture = GameObject.Parent.Texture;
                if (texture != null && pt != null)
                { 
                    XMax = texture.Width * pt.Scale.Width;
                    YMax = texture.Height * pt.Scale.Height;
                }

                PositionX = t.Position.X*XMax;
                PositionY = t.Position.Y*YMax;

                if (PositionX < 0)
                    m_XMin = XMax * -1;
                if (PositionY < 0)
                    m_YMin = YMax * -1;

                Rot = t.Rotation;
                ScaleX = t.Scale.Width;
                ScaleY = t.Scale.Height;
                CenterPositionX = t.CenterPosition.X;
                CenterPositionY = t.CenterPosition.Y;

                var rt = t as IRelativeTransform;

                if (rt != null)
                { 
                    EnableXScaleCompensation = rt.EnableXAxisCompensation;
                    EnableYScaleComensation = rt.EnableYAxisCompensation;
                }
            }
        }

        protected virtual void UpdateEnableXScaleCompensation(bool value)
        {
            if (GameObject == null && !m_init) return;

            var rt = GameObject.GetComponent<RelativeTransformComponent>();

            if(rt == null) return;
            rt.EnableXAxisCompensation = value;
        }

        protected virtual void UpdateEnableYScaleCompensation(bool value)
        {
            if (GameObject == null && !m_init) return;

            var rt = GameObject.GetComponent<RelativeTransformComponent>();

            if (rt == null) return;
            rt.EnableYAxisCompensation = value;
        }

        protected override void UpdatePositionX(float x)
        {
            if (GameObject == null && !m_init)
                return;

            var currentTransform = GameObject?.Transform;
            if (currentTransform == null)
                return;

            var parentTexture = GameObject?.Parent?.Texture;
            if (parentTexture == null)
                return;

            ITransform? parentTransform = (GameObject?.Parent as ITransformable)?.Transform;

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
            if (GameObject == null && !m_init)
                return;

            var currentTransform = GameObject?.Transform;
            if (currentTransform == null)
                return;

            var parentTexture = GameObject?.Parent?.Texture;
            if (parentTexture == null)
                return;

            ITransform? parentTransform = (GameObject?.Parent as ITransformable)?.Transform;

            if (parentTransform == null)
                return;

            float parentActualHeight = (float)parentTexture.Height * parentTransform.Scale.Height;

            if (parentActualHeight == 0)
                return;

            float normalizedY = y / parentActualHeight;
            float oldX = currentTransform.Position.X;
            currentTransform.Position = new Vector2(oldX, normalizedY);
        }

        public override IGEComponent? GetComponent()
        {
            return GameObject?.GetComponent<RelativeTransformComponent>(false);
        }
    }
}
