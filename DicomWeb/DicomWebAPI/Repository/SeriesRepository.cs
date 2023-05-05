using DicomWebAPI.Data;
using DicomWebAPI.Model;
using DicomWebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository
{
    public class SeriesRepository : Repository<Series> , ISeriesRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SeriesRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<List<Series>> GetAllSeriesUnderStudy(Expression<Func<Series, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Series> query = _dbContext.Series;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

             var seriesinfo = await query
                            .Include(s => s.Study)
                           .Include(s => s.Study.Patient)
            .Include(s => s.Images)
            .ToListAsync();
             return seriesinfo;
        }
    }
}
