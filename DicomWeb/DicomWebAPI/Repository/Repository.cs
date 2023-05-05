using DicomWebAPI.Data;
using DicomWebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DicomWebAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }
        public async Task Create(T model)
        {
            await _dbContext.AddAsync( model );
            await Save();
        }

        public async Task Delete(T model)
        {
            _dbContext.Remove(model);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
  
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if(filter != null)
            { 
                query = query.Where(filter); 
            }

            return await query.ToListAsync();
        }

        public async Task Save()
        {
           await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T model)
        {
            _dbContext.Update(model);
            await Save();
        }
    }
}
