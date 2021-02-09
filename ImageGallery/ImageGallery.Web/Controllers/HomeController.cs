using ImageGallery.Core.Models.ImageApi;
using ImageGallery.Core.Repository.Interfaces;
using ImageGallery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageDetailsRepository _imageDetailsRepository;

        public HomeController(IImageDetailsRepository imageDetailsRepository)
        {
            _imageDetailsRepository = imageDetailsRepository;
        }

        public IActionResult Index(string searchTerm)
        {
            if (!String.IsNullOrEmpty(searchTerm) && searchTerm.Length >= 3)
            {
                var data = _imageDetailsRepository.GetAllQ().Where(x =>
                    x.Author.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Camera.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Tags.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    x.Id.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)
                );

                return View(data);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
