using DicomWebAPI.Controllers;
using DicomWebAPI.Model;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository.IRepository
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient> GetAllPatientDetails(Expression<Func<Patient, bool>> filter = null, bool tracked = true);
    }
}
