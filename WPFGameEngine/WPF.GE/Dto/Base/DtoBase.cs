using System.Text.Json.Serialization;
using WPFGameEngine.WPF.GE.Component.Base;
using WPFGameEngine.WPF.GE.Dto.GameObjects;
using WPFGameEngine.WPF.GE.GameObjects;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "RecreateType")]
    [JsonDerivedType(typeof(GameObjectDto), nameof(GameObject))]
    [JsonDerivedType(typeof(ComponentDto), nameof(ComponentBase))]
    public abstract class DtoBase
    {
        public DtoBase() 
        {

        }
    }
}
