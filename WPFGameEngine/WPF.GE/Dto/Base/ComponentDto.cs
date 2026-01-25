using System.Text.Json.Serialization;
using WPFGameEngine.FactoryWrapper.Base;
using WPFGameEngine.WPF.GE.Component.Animations;
using WPFGameEngine.WPF.GE.Component.Animators;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Component.Collider;
using WPFGameEngine.WPF.GE.Component.Controllers;
using WPFGameEngine.WPF.GE.Component.RelativeTransforms;
using WPFGameEngine.WPF.GE.Component.Sprites;
using WPFGameEngine.WPF.GE.Component.Transforms;
using WPFGameEngine.WPF.GE.Dto.Components;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    [JsonDerivedType(typeof(TransformDto), nameof(TransformComponent))]
    [JsonDerivedType(typeof(AnimatorDto), nameof(Animator))]
    [JsonDerivedType(typeof(SpriteDto), nameof(Sprite))]
    [JsonDerivedType(typeof(AnimationDto), nameof(Animation))]
    [JsonDerivedType(typeof(RelativeTransformDto), nameof(RelativeTransformComponent))]
    [JsonDerivedType(typeof(ColliderDto), nameof(ColliderComponent))]
    [JsonDerivedType(typeof(RaycastDto), nameof(RaycastComponent))]
    public abstract class ComponentDto : DtoBase, IConvertToObject<IGEComponent>
    {
        public abstract IGEComponent ToObject(IFactoryWrapper factoryWrapper);
    }
}
