using System.Text.Json.Serialization;

namespace WPFGameEngine.WPF.GE.AnimationFrames
{
    [JsonDerivedType(typeof(AnimationFrame), nameof(AnimationFrame))]
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    public interface IAnimationFrame
    {
        public double Lifespan { get; init; }
    }
}
