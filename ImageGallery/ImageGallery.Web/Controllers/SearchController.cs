using ImageGallery.Core.Models.ImageApi;
using ImageGallery.Core.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IImageDetailsRepository _imageDetailsRepository;

        public SearchController(IImageDetailsRepository imageDetailsRepository)
        {
            _imageDetailsRepository = imageDetailsRepository;
        }

        [HttpGet("{searchTerm}")]
        public IEnumerable<ImageDetails> Search(string searchTerm)
        {
            var data = _imageDetailsRepository.GetAllQ().Where(x =>
                x.Author.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                x.Camera.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                x.Tags.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                x.Id.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)
            );

            return data;
        }
    }
}
