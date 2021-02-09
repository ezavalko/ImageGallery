using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImageGallery.Core.Models.ImageApi;

namespace ImageGallery.Core.Data
{
    public class ImageGalleryContext : DbContext
    {
        public ImageGalleryContext (DbContextOptions<ImageGalleryContext> options)
            : base(options)
        {
        }

        public DbSet<ImageDetails> ImageDetails { get; set; }
    }
}
