using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Models.Base;
using System.Linq;

namespace SpaceAvenger.DAL.Repositories.Base
{
    public abstract class RepositoryBase<TEntity> : 
        IRepositoryBase<TEntity>
        where TEntity : class, IEntity
    {
        public DbContext Context { get; set; }

        protected RepositoryBase()
        {
            
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = 0;
            Context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Context.Set<TEntity>().Remove(entity);
        }

        public void Edit(int id, TEntity newEntity)
        {
            if(newEntity == null)
                throw new ArgumentNullException(nameof(newEntity));

            var entity = Context.Set<TEntity>()
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return;

            Context.Update(entity);
        }

        public DbSet<TEntity> GetAll()
        {
            return Context.Set<TEntity>();
        }

        public int Save()
        { 
            return Context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var set = Context.Set<TEntity>();
            var entity = set.FirstOrDefault(x => x.Id == id);

            if(entity == null) return;

            set.Remove(entity);
        }

        public IQueryable<TEntity> GetById(int id)
        {
            return Context.Set<TEntity>().Where(x => x.Id == id);
        }
    }
}
