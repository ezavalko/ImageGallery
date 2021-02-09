using ImageGallery.Core.Models.ImageApi;
using ImageGallery.Core.Repository.Base;
using System.Threading.Tasks;

namespace ImageGallery.Core.Repository.Interfaces
{
    public interface IImageDetailsRepository : IRepository<ImageDetails>
    {
        Task UpdateAsync(ImageDetails image);
    }
}
