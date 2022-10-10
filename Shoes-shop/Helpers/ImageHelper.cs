using Shoes_shop.Models;

namespace Shoes_shop.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public ImageHelper(IWebHostEnvironment webhost)
        {
            webHostEnvironment = webhost;

        }

        public string UploadImage(Shoes shoes)
        {
            string uniqueName = null;
            if(shoes.ImageFile != null)
            {
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueName = Guid.NewGuid().ToString() + "_" + shoes.ImageFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    shoes.ImageFile.CopyTo(fileStream);

                }

            }
            return uniqueName;

        }
    }
}
