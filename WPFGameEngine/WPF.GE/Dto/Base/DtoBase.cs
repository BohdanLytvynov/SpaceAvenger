using WPFGameEngine.Factories.Base;
using System.Text.Json.Serialization;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects;
using WPFGameEngine.WPF.GE.Dto.Components;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    [JsonDerivedType(typeof(GameObjectDto), nameof(GameObject))]
    [JsonDerivedType(typeof(TransformDto), nameof(TransformComponent))]
    [JsonDerivedType(typeof(SpriteDto), nameof(Sprite))]
    [JsonDerivedType(typeof(AnimationDto), nameof(Animation))]
    [JsonDerivedType(typeof(AnimatorDto), nameof(Animator))]
    public abstract class DtoBase : IGameEngineEntity
    {
        public DtoBase() 
        {

        }
    }
}
