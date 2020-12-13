using System;
using System.Threading.Tasks;

namespace RockContent.Common.DAL
{
    public interface IGenericRepository<T> : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetListAsync(object name, dynamic id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);
    }
}
