using SpaceAvenger.Editor.Mock;
using SpaceAvenger.Editor.ViewModels.Components.Base;
using SpaceAvenger.Editor.ViewModels.Options;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;

namespace SpaceAvenger.Editor.ViewModels.Components.Collider
{
    internal class RaycastComponentViewModel : ComponentViewModel
    {
        #region Fields
        private double m_XMax;
        private double m_YMax;

        private double m_XRel;
        private double m_YRel;
        #endregion

        #region Properties
        public double XRel
        {
            get => m_XRel;
            set
            {
                Set(ref m_XRel, value);
                UpdateRelX(value);
            }
        }
        public double YRel
        {
            get => m_YRel;
            set
            {
                Set(ref m_YRel, value);
                UpdateRelY(value);
            }
        }

        public double XMax
        {
            get => m_XMax;
            set
            {
                Set(ref m_XMax, value);
            }
        }
        public double YMax
        {
            get => m_YMax;
            set
            {
                Set(ref m_YMax, value);
            }
        }
        #endregion

        public RaycastComponentViewModel(IGameObjectMock gameObject) : 
            base(nameof(RaycastComponent), gameObject)
        {
            LoadCurrentGameObjProperties();
        }

        #region Methods

        public override IGEComponent? GetComponent()
        {
            return GameObject.GetComponent<RaycastComponent>(false);
        }

        protected override void LoadCurrentGameObjProperties()
        {
            if (GameObject == null && !GameObject.IsCollidable) return;
            var collider = GameObject.ColliderComponent;

            XMax = collider.ActualObjectSize.Width;
            YMax = collider.ActualObjectSize.Height;

            m_XRel = collider.Position.X * collider.ActualObjectSize.Width;
            m_YRel = collider.Position.Y * collider.ActualObjectSize.Height;
        }

        private void UpdateRelX(double value)
        {
            if (GameObject == null) return;
            var collider = GameObject.ColliderComponent;
            if (collider == null) return;
            float oldY = collider.Position.Y;
            float x = (float)value / collider.ActualObjectSize.Width;
            collider.Position = new System.Numerics.Vector2(x, oldY);
        }

        private void UpdateRelY(double value)
        {
            if (GameObject == null) return;
            var collider = GameObject.ColliderComponent;
            if (collider == null) return;
            float oldX = collider.Position.X;
            float y = (float)value / collider.ActualObjectSize.Height;
            collider.Position = new System.Numerics.Vector2(oldX, y);
        }

        #endregion
    }
}
