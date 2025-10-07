using System.Drawing;
using System.Numerics;
using System.Windows.Media.Media3D;
using WPFGameEngine.GameViewControl;


namespace WPFGameEngine.WPF.GE.GameObjects
{
    public abstract class GameObject
    {
        #region Propeties

        public Vector<float> Position { get; set; }

        public double Rotation { get; set; }

        public Size Scale { get; set; }

        #endregion

        #region Ctor
        protected GameObject()
        {

        }
        #endregion

        #region Methods
        public abstract void Render(GameViewHost canvas);
        #endregion
    }
}
