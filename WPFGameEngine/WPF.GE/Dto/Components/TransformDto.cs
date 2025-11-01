using System.Drawing;
using System.Numerics;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Base;

namespace WPFGameEngine.WPF.GE.Dto.Components
{
    public class TransformDto : DtoBase
    {
        public Vector2 Position { get; set; }
        public Vector2 CenterPosition { get; set; }
        public double Rotation { get; set; }//Degree
        public SizeF Scale { get; set; }

        public TransformDto() : base(nameof(TransformComponent))
        {
            
        }
    }
}
