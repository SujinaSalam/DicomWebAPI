using DicomWebAPI.Data;
using DicomWebAPI.Model;
using DicomWebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository
{
    public class ImageRepository : Repository<Image> , IImageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ImageRepository(ApplicationDbContext context) : base(context)
        { 
            _dbContext = context;
        }

        public async Task<List<Image>> GetAllImagesUnderSeries(Expression<Func<Image, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Image> query = _dbContext.Images;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query
                .Include(imageitems=> imageitems.Series)
                .ThenInclude(seriesitem => seriesitem.Study)
                .ThenInclude(studyitem=> studyitem.Patient)
                .ToListAsync();
        }
    }
}
