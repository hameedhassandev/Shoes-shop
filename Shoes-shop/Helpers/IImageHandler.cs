using Shoes_shop.Models;

namespace Shoes_shop.Helpers
{
    public interface IImageHandler
    {
        public string UploadImage(Shoes shoes);
        public void RemoveImage(string imgPath);
    }
}
