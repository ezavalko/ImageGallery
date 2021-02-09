using System.Threading.Tasks;

namespace ImageGallery.Core.Api
{
    public interface IAPIImageClient
    {
        Task RefreshImagesData();
    }
}
