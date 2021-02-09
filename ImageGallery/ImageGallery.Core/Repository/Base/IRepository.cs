using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Core.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Delete(string id);
        void Update(T entity);
        List<T> GetAll();
        IQueryable<T> GetAllQ();
        Task CommitAsync();
    }
}
