using System.Windows.Media;
using WPFGameEngine.Services.Interfaces;

namespace SpaceAvenger.Services.WpfRenderProcedure
{
    public class WpfRenderProcedure : IRenderProcedure
    {
        public DrawingContext DrawingContext { get; set; }
        public void Render()
        {
            throw new System.NotImplementedException();
        }
    }
}
