using System.ComponentModel.DataAnnotations;

namespace SpaceAvenger.DAL.Models.Base
{
    public class LoreEntity : EntityBase, ILoreEntity
    {
        public string NameKey { get ; set ; }
        public string DescriptionKey { get; set; }
    }
}
