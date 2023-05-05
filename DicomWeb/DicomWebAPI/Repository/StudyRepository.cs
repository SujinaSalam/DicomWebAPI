using DicomWebAPI.Data;
using DicomWebAPI.Model;
using DicomWebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository
{
    public class StudyRepository : Repository<Study> , IStudyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StudyRepository(ApplicationDbContext context) : base(context)
        { 
            _dbContext = context;
        }
        public async Task<List<Study>> GetAllStudyUnderPatient(Expression<Func<Study, bool>> filter = null, bool tracked = true)
        {

            IQueryable<Study> query = _dbContext.Studies;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var studyinfo =  await query.ToListAsync();
            var studylist = await query.Include(s => s.Patient)
        .Include(s => s.Series).
        ThenInclude(seriesitem=>seriesitem.Images)
        .ToListAsync();

            //    var studyinfo = await query
            //.Include(studyitem=>_dbContext.Series.Where( seriesitem => ((Series)seriesitem).StudyInstanceUID == ((Study)studyitem).StudyInstanceUID))
            //.ThenInclude(seriesitems => _dbContext.Images.Where(imageitem => ((Image)imageitem).SeriesInstanceUID == ((Series)seriesitems).SeriesInstanceUID))
            //.ToListAsync();

            return studyinfo;

            //    foreach(var studyitem in studylist)
            //    {

            //        IQueryable<Series> querySer = _dbContext.Series;
            //        if (!tracked)
            //        {
            //            querySer = querySer.AsNoTracking();
            //        }

            //        if (filter != null)
            //        {
            //            querySer = querySer.Where(item=>item.StudyInstanceUID == studyitem.StudyInstanceUID);
            //        }

            //        var serieslist = await querySer.ToListAsync();


            //        foreach (var series in serieslist)
            //        {
            //            IQueryable<Image> queryImage = _dbContext.Images;
            //            if (!tracked)
            //            {
            //                queryImage = queryImage.AsNoTracking();
            //            }

            //            if (filter != null)
            //            {
            //                queryImage = queryImage.Where(item => item.SeriesInstanceUID == series.SeriesInstanceUID);
            //            }

            //            var imagelist = await queryImage.ToListAsync();
            //        }
            //    }
        }
    }
}
