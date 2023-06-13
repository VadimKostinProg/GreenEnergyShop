using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Data access tools for enitites
    /// </summary>
    /// <typeparam name="T">Entity class from data source.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Method for creating new instance in data source.
        /// </summary>
        /// <param name="entity">Entity to insert to data source.</param>
        /// <returns>Inserted entity.</returns>
        T Create(T entity);

        /// <summary>
        /// Method for reading entity from data source by it`s Guid.
        /// </summary>
        /// <param name="id">Guid of entity to read.</param>
        /// <returns>Entity with passed guid, null - if entity with passed guid doues not exist.</returns>
        T? GetValueById(Guid id);

        /// <summary>
        /// Method for reading all entities from data source.
        /// </summary>
        /// <returns>Collection IEnumerable of enitites.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Method for updating existing instance in data source.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Updated entity, null - if entity with passed guid doues not exist.</returns>
        T? Update(T entity);

        /// <summary>
        /// Method for deleting existing entity from data source by it`s Guid.
        /// </summary>
        /// <param name="entity">Guid of entity to delete.</param>
        /// <returns>True - of deleting is successful, otherwise - false.</returns>
        bool Delete(Guid id);

        /// <summary>
        /// Method for saving hanges in the data base.
        /// </summary>
        Task Save();
    }
}
