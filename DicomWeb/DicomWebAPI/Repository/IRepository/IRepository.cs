using DicomWebAPI.Model;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T model);
        Task Update(T model);
        Task Delete(T model);
        Task Save();
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);
    }
}
