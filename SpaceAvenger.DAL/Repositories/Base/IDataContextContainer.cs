using Microsoft.EntityFrameworkCore;

namespace SpaceAvenger.DAL.Repositories.Base
{
    public interface IDataContextContainer
    {
        public DbContext Context { get; set; }
    }
}
