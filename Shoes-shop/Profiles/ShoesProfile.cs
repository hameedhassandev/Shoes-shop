using AutoMapper;

namespace Shoes_shop.Profiles
{
    public class ShoesProfile : Profile
    {
        public ShoesProfile()
        {
            CreateMap<Models.Shoes, ViewModels.ShoesViewModel>();
        }
    }
}
