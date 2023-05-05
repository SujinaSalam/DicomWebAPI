using DicomWebAPI.Controllers;
using DicomWebAPI.Model;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository.IRepository
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<List<Image>> GetAllImagesUnderSeries(Expression<Func<Image, bool>> filter = null, bool tracked = true);
    }
}
