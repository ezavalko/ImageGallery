using ImageGallery.Core.Data;
using ImageGallery.Core.Models.ImageApi;
using ImageGallery.Core.Repository.Base;
using ImageGallery.Core.Repository.Interfaces;
using System.Threading.Tasks;

namespace ImageGallery.Core.Repository.Implementations
{
    public class ImageDetailsRepository : EFRespository<ImageDetails>, IImageDetailsRepository
    {
        public ImageDetailsRepository(ImageGalleryContext dbContext) : base(dbContext)
        {
        }

        public async Task UpdateAsync(ImageDetails image)
        {
            var entity = _dbContext.ImageDetails.Find(image.Id);

            entity.Author = image.Author;
            entity.Camera = image.Camera;
            entity.CroppedPicture = image.CroppedPicture;
            entity.FullPicture = image.FullPicture;
            entity.Tags = image.Tags;

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
