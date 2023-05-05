using DicomWebAPI.Controllers;
using DicomWebAPI.Model;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository.IRepository
{
    public interface ISeriesRepository : IRepository<Series>
    {
        Task<List<Series>> GetAllSeriesUnderStudy(Expression<Func<Series, bool>> filter = null, bool tracked = true);
    }
}
