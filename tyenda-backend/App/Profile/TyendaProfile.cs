﻿using System.Collections.Generic;
using System.Linq;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Cart_;
using tyenda_backend.App.Models._Cart_.Views;
using tyenda_backend.App.Models._Category_;
using tyenda_backend.App.Models._City_;
using tyenda_backend.App.Models._City_.Views;
using tyenda_backend.App.Models._Country_;
using tyenda_backend.App.Models._Country_.Views;
using tyenda_backend.App.Models._Item_;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models._ItemRate_;
using tyenda_backend.App.Models._Notification_.Views;
using tyenda_backend.App.Models._Order_;
using tyenda_backend.App.Models._Order_.Views;
using tyenda_backend.App.Models._Store_;
using tyenda_backend.App.Models._Store_.Views;

namespace tyenda_backend.App.Profile
{
    public class TyendaProfile : AutoMapper.Profile
    {
        public TyendaProfile()
        {
            //------------------------------------------------Mapping------------------------------------------------
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
                .ForMember(dest => dest.IsItemLiked, map => map.MapFrom(src => src.Likes.Count > 0 ? true : false))
                .ForMember(dest => dest.StoreEmail, map => map.MapFrom(src => src.Store!.Account!.Email))
                .ForMember(dest => dest.Price, map => map.MapFrom(src => src.Discount > 0 ? src.Price - (src.Price * ((decimal) src.Discount / 100)) : src.Price))
                .ForMember(dest => dest.Rate, map => map.MapFrom(src => GenerateItemRate(src.ItemRates)));

            CreateMap<Store, StoreModerateView>()
                .ForMember(dest => dest.ProfileImage, map => map.MapFrom(src => src.Account!.ProfileImage))
                .ForMember(dest => dest.Email, map => map.MapFrom(src => src.Account!.Email))
                .ForMember(dest => dest.Phone, map => map.MapFrom(src => src.Account!.PhoneNumber))
                .ForMember(dest => dest.IsFollowed, map => map.MapFrom(src => src.Followers.Count > 0? true : false))
                .ForMember(dest => dest.IsAddedToCart, map => map.MapFrom(src => src.Carts.Count > 0 ? true : false))
                .ForMember(dest => dest.CreatedAt, map => map.MapFrom(src => src.Account!.CreatedAt));

            CreateMap<Order, OrderBasicView>()
                .ForMember(dest => dest.City, map => map.MapFrom(src => src.City!.Value))
                .ForMember(dest => dest.Country, map => map.MapFrom(src => src.City!.Country!.Value))
                .ForMember(dest => dest.Price, map => map.MapFrom(src => src.Item!.Discount == 0? src.Item!.Price * src.Quantity : (src.Item!.Price - (src.Item!.Price * ((decimal) src.Item!.Discount / 100))) * src.Quantity))
                .ForMember(dest => dest.CustomerName, map => map.MapFrom(src => src.Customer!.Firstname + " " + src.Customer!.Lastname))
                .ForMember(dest => dest.ItemName, map => map.MapFrom(src => src.Item!.Value))
                .ForMember(dest => dest.ProfileImage, map => map.MapFrom(src => src.Item!.Store!.Account!.ProfileImage))
                .ForMember(dest => dest.StoreName, map => map.MapFrom(src => src.Item!.Store!.Name));

            CreateMap<Cart, CartStoreBasicView>()
                .ForMember(dest => dest.StoreId, map => map.MapFrom(src => src.Store!.Id))
                .ForMember(dest => dest.StoreName, map => map.MapFrom(src => src.Store!.Name))
                .ForMember(dest => dest.ProfileImage, map => map.MapFrom(src => src.Store!.Account!.ProfileImage))
                .ForMember(dest => dest.BackgroundImage, map => map.MapFrom(src => src.Store!.BackgroundImage))
                .ForMember(dest => dest.FollowersCount, map => map.MapFrom(src => src.Store!.Followers.Count))
                .ForMember(dest => dest.ItemsCount, map => map.MapFrom(src => src.Store!.Items.Count))
                .ForMember(dest => dest.OrdersCount, map => map.MapFrom(src => GenerateOrdersCount(src.Store!.Items)))
                .ForMember(dest => dest.Description, map => map.MapFrom(src => src.Store!.Description));

            //------------------------------------------------Mapping------------------------------------------------
        }
        
        public static double GenerateItemRate(ICollection<ItemRate> itemRates)
        {
            var sum = 0.0;
            if (itemRates.Count == 0) return 0;
            
            foreach (var rate in itemRates.Select(itemRate => itemRate.Rate))
            {
                sum += rate;
            }

            return sum / itemRates.Count;

        }

        public static int GenerateOrdersCount(ICollection<Item> items)
        {
            var count = 0;
            foreach (var item in items)
            {
                count += item.Orders.Count;
            }

            return count;
        }
    }
}