using DicomWebAPI.Controllers;
using DicomWebAPI.Model;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository.IRepository
{
    public interface IStudyRepository : IRepository<Study>
    {
        Task<List<Study>> GetAllStudyUnderPatient(Expression<Func<Study, bool>> filter = null, bool tracked = true);
    }
}
