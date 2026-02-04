using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Models
{
    public class User : EntityBase
    {
        #region Properties
        public string ProfileName { get; set; }//Same as Commander
        public DateTime LastSaveTime { get; set; }
        public Commander Commander { get; set; }//1 Commander for 1 User
        #endregion
    }
}
