using Newtonsoft.Json;

namespace ImageGallery.Core.Models.ImageApi
{
    public class ImageInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("cropped_picture")]
        public string CroppedPicture { get; set; }
    }
}
