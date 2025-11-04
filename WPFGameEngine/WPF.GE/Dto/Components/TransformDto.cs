using System.Drawing;
using System.Numerics;
using WPFGameEngine.Factories.Base;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class TransformDto : ComponentDto
    {
        public Vector2 Position { get; set; }
        public Vector2 CenterPosition { get; set; }
        public double Rotation { get; set; }//Degree
        public SizeF Scale { get; set; }

        public TransformDto()
        {
            
        }

        public override ITransform ToObject(IFactoryWrapper factoryWrapper)
        {
            return new TransformComponent(Position, CenterPosition, Rotation, Scale);
        }
    }
}
