using SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel.GeometryConfigBase;
using WPFGameEngine.WPF.GE.Geometry.Base;
using WPFGameEngine.WPF.GE.Geometry.Realizations;
using WPFGameEngine.WPF.GE.Math.Sizes;

namespace SpaceAvenger.Editor.ViewModels.GeometryConfigViewModel
{
    internal class RectangleConfigViewModel : GeometryConfigViewModelBase
    {
        private double m_width;
        private double m_height;

        public double Width
        {
            get => m_width;
            set 
            { 
                Set(ref m_width, value);
                UpdateWidth(value);
            }
        }

        public double Height
        {
            get => m_height;
            set 
            { 
                Set(ref m_height, value);
                UpdateHeight(value);
            }
        }

        public RectangleConfigViewModel(IShape2D shape2D) : base("Rectangle Geometry", shape2D)
        {

        }

        protected override void LoadCurrentGeometryProperties()
        {
            if (Shape2D == null) return;
            var rect = Shape2D as Rectangle;
            if (rect == null) return;
            Width = rect.Size.Width;
            Height = rect.Size.Height;
        }

        private void UpdateWidth(double value)
        {
            if (Shape2D == null) return;
            var rect = Shape2D as Rectangle;
            if (rect == null) return;
            float oldHeight = rect.Size.Height;
            rect.Size = new Size(value, oldHeight);
        }

        private void UpdateHeight(double value)
        {
            if (Shape2D == null) return;
            var rect = Shape2D as Rectangle;
            if (rect == null) return;
            float oldWidth = rect.Size.Width;
            rect.Size = new Size(oldWidth, value);
        }
    }
}
