using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Editor.ViewModels.Components.Transform
{
    internal class TransformComponentViewModel : ComponentViewModel
    {
        #region Fields
        private double m_posX;
        private double m_posY;
        private double m_rot;
        private double m_ScaleX;
        private double m_ScaleY;
        private double m_CenterPositionX;
        private double m_CenterPositionY;
        protected bool m_init = false;
        #endregion

        #region Properties
        public double PositionX
        {
            get => m_posX;
            set
            {
                Set(ref m_posX, value);
                UpdatePositionX((float)PositionX);
            }
        }

        public double PositionY
        {
            get => m_posY;
            set
            {
                Set(ref m_posY, value);
                UpdatePositionY((float)PositionY);
            }
        }

        public double Rot
        {
            get => m_rot;
            set
            {
                Set(ref m_rot, value);
                UpdateRotation((float)Rot);
            }
        }

        public double ScaleX
        {
            get => m_ScaleX;
            set
            {
                Set(ref m_ScaleX, value);
                UpdateScaleX((float)ScaleX);
            }
        }

        public double ScaleY
        {
            get => m_ScaleY;
            set
            {
                Set(ref m_ScaleY, value);
                UpdateScaleY((float)ScaleY);
            }
        }

        public double CenterPositionX
        {
            get => m_CenterPositionX;
            set
            {
                Set(ref m_CenterPositionX, value);
                UpdateCenterPositionX((float)CenterPositionX);
            }
        }

        public double CenterPositionY
        {
            get => m_CenterPositionY;
            set
            {
                Set(ref m_CenterPositionY, value);
                UpdateCenterPositionY((float)CenterPositionY);
            }
        }
        #endregion

        #region Ctor

        public TransformComponentViewModel(string componentName, IGameObjectMock gameObject)
            : base(componentName, gameObject)
        {
            
        }

        public TransformComponentViewModel(IGameObjectMock gameObject) 
            : base(nameof(TransformComponent), gameObject)
        {
            LoadCurrentGameObjProperties();
            m_init = true;
        }

        #endregion

        #region Methods
        protected override void LoadCurrentGameObjProperties()
        {
            if (GameObject != null)
            {
                var t = GameObject.Transform;

                PositionX = t.Position.X;
                PositionY = t.Position.Y;
                Rot = t.Rotation;
                ScaleX = t.Scale.Width;
                ScaleY = t.Scale.Height;
                CenterPositionX = t.CenterPosition.X;
                CenterPositionY = t.CenterPosition.Y;
            }
        }

        protected virtual void UpdatePositionX(float x)
        {
            if (GameObject != null && m_init)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float y = t.Position.Y;
                t.Position = new Vector2(x, y);
            }
        }

        protected virtual void UpdatePositionY(float y)
        {
            if (GameObject != null && m_init)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float x = t.Position.X;
                t.Position = new Vector2(x, y);
            }
        }

        private void UpdateRotation(float rotation)
        {
            if (GameObject != null && m_init)
            {
                var t = GameObject.Transform;
                t.Rotation = rotation;
            }

        }

        private void UpdateScaleX(float x)
        {
            if (GameObject != null && m_init)
            {
                var t = GameObject.Transform;
                float y = t.Scale.Height;
                GameObject.Scale(new Size(x, y));
            }
        }

        private void UpdateScaleY(float y)
        {
            if (GameObject != null && m_init)
            {
                var t = GameObject.Transform;
                float x = t.Scale.Width;
                GameObject.Scale(new Size(x, y));
            }
        }

        private void UpdateCenterPositionX(float x)
        {
            if (GameObject != null && m_init)
            {
                var t = GameObject.Transform;
                float y = t.CenterPosition.Y;
                t.CenterPosition = new Vector2(x, y);
            }
        }

        private void UpdateCenterPositionY(float y)
        {
            if (GameObject != null && m_init)
            {
                var t = GameObject.Transform;
                float x = t.CenterPosition.X;
                t.CenterPosition = new Vector2(x, y);
            }
        }

        public override IGEComponent? GetComponent()
        {
            return GameObject?.GetComponent<TransformComponent>(false);
        }
        #endregion
    }
}
