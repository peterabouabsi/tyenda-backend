using System.Linq;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Category_;
using tyenda_backend.App.Models._City_;
using tyenda_backend.App.Models._City_.Views;
using tyenda_backend.App.Models._Country_;
using tyenda_backend.App.Models._Country_.Views;
using tyenda_backend.App.Models._Notification_.Views;

namespace tyenda_backend.App.Profile
{
    public class TyendaProfile : AutoMapper.Profile
    {
        public TyendaProfile()
        {
            CreateMap<Country, BasicCountryView>();

            CreateMap<City, BasicCityView>();

            CreateMap<Category, BasicCategoryView>();

            CreateMap<Alert, ViewModerateAlert>()
                .ForMember(dest => dest.Title, map => map.MapFrom(src => src.Notification!.Title))
                .ForMember(dest => dest.Description, map => map.MapFrom(src => src.Notification!.Description))
                .ForMember(dest => dest.CreatedAt, map => map.MapFrom(src => src.Notification!.CreatedAt))
                .ForMember(dest => dest.Link, map => map.MapFrom(src => src.Notification!.Link))
                .ForMember(dest => dest.ItemImageUrl, map => map.MapFrom(src => src.Notification!.Item!.ItemImages.First().Url))
                .ForMember(dest => dest.StoreImageUrl, map => map.MapFrom(src => src.Notification!.Store!.Account!.ProfileImage))
                .ForMember(dest => dest.IsViewed, map => map.MapFrom(src => src.IsViewed));
        }
    }
}