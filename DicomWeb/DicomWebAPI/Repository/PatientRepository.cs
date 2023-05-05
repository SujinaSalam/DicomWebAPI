using DicomWebAPI.Data;
using DicomWebAPI.Model;
using DicomWebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository
{
    public class PatientRepository : Repository<Patient> , IPatientRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<Patient> GetAllPatientDetails(Expression<Func<Patient, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Patient> query = _dbContext.Patients;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query
                .Include(p => p.Studies)
                .ThenInclude(study => study.Series)
                .ThenInclude(series => series.Images)
                .FirstOrDefaultAsync();
        }
    }
}
