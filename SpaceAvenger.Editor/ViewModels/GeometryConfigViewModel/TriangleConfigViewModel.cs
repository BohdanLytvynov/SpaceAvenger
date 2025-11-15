using SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel.GeometryConfigBase;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;

namespace SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel
{
    internal class TriangleConfigViewModel : GeometryConfigViewModelBase
    {
        private double m_Height;
        private double m_Base;

        public double Height 
        {
            get=> m_Height;
            set
            {
                Set(ref m_Height, value);
                UpdateHeight(value);
            }
        }

        public double Base 
        {
            get=> m_Base;
            set
            { 
                Set(ref m_Base, value);
                UpdateBase(value);
            }
        }

        public TriangleConfigViewModel(IShape2D shape2D) : base("Triangle Configuration", shape2D)
        {
        }

        protected override void LoadCurrentGeometryProperties()
        {
            if(Shape2D == null) return;
            var triangle = Shape2D as Triangle;
            if(triangle == null) return;
            Height = triangle.Height;
            Base = triangle.Base;
        }

        private void UpdateHeight(double value)
        {
            if (Shape2D == null) return;
            var triangle = Shape2D as Triangle;
            if(triangle == null) return;
            triangle.Height = (float)value;
        }

        private void UpdateBase(double value)
        {
            if (Shape2D == null) return;
            var triangle = Shape2D as Triangle;
            if(triangle == null) return;
            triangle.Base = (float)value;
        }
    }
}
