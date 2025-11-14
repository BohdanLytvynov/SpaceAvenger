using SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel.GeometryConfigBase;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;

namespace SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel
{
    internal class CircleConfigViewModel : GeometryConfigViewModelBase
    {
        #region Fields
        private double m_Radius;
        #endregion

        #region Properties
        public double Radius 
        { 
            get => m_Radius;
            set 
            {
                Set(ref m_Radius, value);
                UpdateRadius(value);
            }
        }
        #endregion

        public CircleConfigViewModel(IShape2D shape) : base("Circle Geometry", shape)
        {
            
        }

        protected override void LoadCurrentGeometryProperties()
        {
            if(Shape2D == null) return;

            var circle = (Circle)Shape2D;
            m_Radius = circle.Radius;
        }

        private void UpdateRadius(double value)
        {
            if (Shape2D == null) return;
            if (value < 0) return;
            (Shape2D as Circle).Radius = (float)value;
        }
    }
}
