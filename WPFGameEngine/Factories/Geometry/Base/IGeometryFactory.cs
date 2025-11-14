using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.Factories.Geometry.Base
{
    public interface IGeometryFactory<GeometryType> : IFactory
        where GeometryType : IShape2D
    {
    }
}
