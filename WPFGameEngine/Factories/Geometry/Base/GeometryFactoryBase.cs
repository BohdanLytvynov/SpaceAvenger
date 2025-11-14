using WPFGameEngine.Factories.Base;
using WPFGameEngine.WPF.GE.Geometry.Base;

namespace WPFGameEngine.Factories.Geometry.Base
{
    public class GeometryFactoryBase<GeometryType> : FactoryBase, IGeometryFactory<GeometryType>
        where GeometryType : IShape2D
    {
        public GeometryFactoryBase()
        {
            ProductName = typeof(GeometryType).Name;
        }

        public override IShape2D Create()
        {
            return (Shape2D)Activator.CreateInstance(typeof(GeometryType));
        }
    }
}
