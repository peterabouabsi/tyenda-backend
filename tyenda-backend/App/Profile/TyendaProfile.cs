using System.Collections.Generic;
using System.Linq;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Category_;
using tyenda_backend.App.Models._City_;
using tyenda_backend.App.Models._City_.Views;
using tyenda_backend.App.Models._Country_;
using tyenda_backend.App.Models._Country_.Views;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models._ItemRate_;
using tyenda_backend.App.Models._Notification_.Views;
using tyenda_backend.App.Models._Store_;
using tyenda_backend.App.Models._Store_.Views;

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

            CreateMap<Item, ItemBasicView>()
                .ForMember(dest => dest.ItemImage,
                    map => map.MapFrom(src => src.ItemImages.Count > 0 ? src.ItemImages.First().Url : null))
                .ForMember(dest => dest.StoreName, map => map.MapFrom(src => src.Store!.Name))
                .ForMember(dest => dest.ProfileImage, map => map.MapFrom(src => src.Store!.Account!.ProfileImage))
                .ForMember(dest => dest.IsAddedToCart, map => map.MapFrom(src => src.Carts.Count > 0 ? true : false))
                .ForMember(dest => dest.StoreEmail, map => map.MapFrom(src => src.Store!.Account!.Email))
                .ForMember(dest => dest.Price, map => map.MapFrom(src => src.Discount > 0 ? src.Price - (src.Price * ((decimal) src.Discount / 100)) : src.Price))
                .ForMember(dest => dest.Rate, map => map.MapFrom(src =>  GenerateItemRate(src.ItemRates)));

            CreateMap<Store, StoreModerateView>()
                .ForMember(dest => dest.ProfileImage, map => map.MapFrom(src => src.Account!.ProfileImage))
                .ForMember(dest => dest.Email, map => map.MapFrom(src => src.Account!.Email))
                .ForMember(dest => dest.Phone, map => map.MapFrom(src => src.Account!.PhoneNumber))
                .ForMember(dest => dest.IsFollowed, map => map.MapFrom(src => src.Followers.Count > 0? true : false))
                .ForMember(dest => dest.IsAddedToCart, map => map.MapFrom(src => src.Carts.Count > 0 ? true : false))
                .ForMember(dest => dest.CreatedAt, map => map.MapFrom(src => src.Account!.CreatedAt));
        }

        private static double GenerateItemRate(ICollection<ItemRate> itemRates)
        {
            var sum = 0.0;
            if (itemRates.Count == 0) return 0;
            
            foreach (var rate in itemRates.Select(itemRate => itemRate.Rate))
            {
                sum += rate;
            }

            return sum / itemRates.Count;
        } 
    }
}