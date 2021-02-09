using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImageGallery.Core.Models.ImageApi
{
    public class Images
    {
        [JsonProperty("pictures")]
        public List<ImageInfo> Pictures { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("pageCount")]
        public int PageCount { get; set; }
        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }
    }
}
