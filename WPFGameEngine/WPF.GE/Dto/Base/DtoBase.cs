using WPFGameEngine.Factories.Base;

namespace WPFGameEngine.WPF.GE.Dto.Base
{
    public abstract class DtoBase : IGameEngineEntity
    {
        /// <summary>
        /// Name of the type to be recreated
        /// </summary>
        public string RecreatedTypeName { get; set; }
        
        public DtoBase(string typeName) 
        {
            RecreatedTypeName = typeName;
        }        
    }
}
