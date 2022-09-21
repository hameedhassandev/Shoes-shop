using Shoes_shop.Models;

namespace Shoes_shop.Helpers
{
    public class ImageHandler : IImageHandler
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public ImageHandler(IWebHostEnvironment webhost)
        {
            webHostEnvironment = webhost;

        }
        public void RemoveImage(string imgPath)
        {
            string image = Path.Combine(webHostEnvironment.WebRootPath, "images", imgPath);
            FileInfo fileInfo = new FileInfo(image);
            if (fileInfo != null)
            {
                File.Delete(image);
                fileInfo.Delete();
            }
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
