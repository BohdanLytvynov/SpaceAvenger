using System.Windows.Media;
using WPFGameEngine.WPF.GE.Math.Matrixes;

namespace WPFGameEngine.WPF.GE.GameObjects
{
    public interface IRenderable
    {
        /// <summary>
        /// Controls Rendering of the Object, but Update calculations will still take place
        /// </summary>
        public bool IsVisible { get; set; }

        void Render(DrawingContext dc, Matrix3x3 parent);

        void Hide();
        void Show();
    }
}
