using System.Windows.Shapes;
using WPFGameEngine.WPF.GE.Component.Base;

namespace WPFGameEngine.WPF.GE.Component.Collider
{
    public interface ICollaider : IGEComponent
    {
        Shape CollisionShape { get; set; }
    }
}
