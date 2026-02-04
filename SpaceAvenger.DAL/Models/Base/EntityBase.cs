using System.ComponentModel.DataAnnotations;

namespace SpaceAvenger.DAL.Models.Base
{
    public class EntityBase : IEntity
    {
       [Key]
       public int Id { get; set; }
    }
}
