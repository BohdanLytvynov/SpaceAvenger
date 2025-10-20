using SpaceAvenger.Editor.ViewModels.Components.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.GameObjects;

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
        public TransformComponentViewModel(IGameObject gameObject) : base("Transform", gameObject)
        {
            LoadCurrentGameObjProperties();
        }
        #endregion

        #region Methods
        private void LoadCurrentGameObjProperties()
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);

                PositionX = t.Position.X;
                PositionY = t.Position.Y;
                Rot = t.Rotation;
                ScaleX = t.Scale.Width;
                ScaleY = t.Scale.Height;
                CenterPositionX = t.CenterPosition.X;
                CenterPositionY = t.CenterPosition.Y;
            }
        }

        private void UpdatePositionX(float x)
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float y = t.Position.Y;
                t.Position = new Vector2(x, y);
            }
        }

        private void UpdatePositionY(float y)
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float x = t.Position.X;
                t.Position = new Vector2(x, y);
            }
        }

        private void UpdateRotation(float rotation)
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                t.Rotation = rotation;
            }

        }

        private void UpdateScaleX(float x)
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float y = t.Scale.Height;
                t.Scale = new SizeF(x, y);
            }
        }

        private void UpdateScaleY(float y)
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float x = t.Scale.Width;
                t.Scale = new SizeF(x, y);
            }
        }

        private void UpdateCenterPositionX(float x)
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float y = t.CenterPosition.Y;
                t.CenterPosition = new Vector2(x, y);
            }
        }

        private void UpdateCenterPositionY(float y)
        {
            if (GameObject != null)
            {
                var t = GameObject.GetComponent<TransformComponent>(true);
                float x = t.CenterPosition.X;
                t.CenterPosition = new Vector2(x, y);
            }
        }
        #endregion
    }
}
