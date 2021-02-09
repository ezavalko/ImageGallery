using ImageGallery.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Core.Repository.Base
{
    public class EFRespository<T> where T : class
    {
        protected readonly DbSet<T> dbset;
        protected readonly ImageGalleryContext _dbContext;

        public EFRespository(ImageGalleryContext dbContext)
        {
            _dbContext = dbContext;
            dbset = dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbset.AddAsync(entity);
        }

        public virtual void Delete(string id)
        {
            var entity = dbset.Find(id);
            if (entity != null)
            {
                dbset.Remove(entity);
            }
        }

        public virtual List<T> GetAll()
        {
            return dbset.AsNoTracking().ToList();
        }

        public virtual void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<T> GetAllQ()
        {
            return dbset;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
