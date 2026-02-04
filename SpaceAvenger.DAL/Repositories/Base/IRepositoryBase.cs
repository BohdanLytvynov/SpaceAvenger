using Microsoft.EntityFrameworkCore;
using SpaceAvenger.DAL.Models.Base;

namespace SpaceAvenger.DAL.Repositories.Base
{
    /// <summary>
    /// Basic interface for the CRUD operations
    /// </summary>
    public interface IRepositoryBase<TEntity> : IDataContextContainer
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Add an Entity to the repository
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);
        /// <summary>
        /// Edit entity in the repository
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newEntity"></param>
        void Edit(int id, TEntity newEntity);
        /// <summary>
        /// Delete entity in a repository
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// Get db set for filtering
        /// </summary>
        /// <returns></returns>
        DbSet<TEntity> GetAll();
        /// <summary>
        /// Update database by building sql querry
        /// </summary>
        /// <returns></returns>
        int Save();
        /// <summary>
        /// Delete an entity by Id
        /// </summary>
        /// <param name="id"></param>
        void DeleteById(int id);
        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id"></param>
        IQueryable<TEntity> GetById(int id);
    }
}
