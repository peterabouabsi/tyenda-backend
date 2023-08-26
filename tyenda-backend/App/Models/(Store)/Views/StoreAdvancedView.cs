using System;
using System.Collections.Generic;

namespace tyenda_backend.App.Models._Store_.Views
{
    public class StoreAdvancedView
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string BackgroundImage { get; set; } = "";
        public string ProfileImage { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string OwnerName { get; set; } = "";
        public string OwnerEmail { get; set; } = "";
        public string Website { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string VideoUrl { get; set; } = "";
        public string VideoPosterUrl { get; set; } = "";
        public List<string> Categories { get; set; } = new List<string>();
        public List<BranchView> Branches { get; set; } = new List<BranchView>();
        public decimal[] DisplayedBranch { get; set; } = new decimal[2];
        public bool IsFollowed { get; set; }
        public bool IsAddedToCart { get; set; }

        public bool IsMyProfile { get; set; } = false;
        public int CountOrders { get; set; }
        public int CountItems { get; set; }
        public int CountFollowers { get; set; }
    }

    public class BranchView
    {
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string AddressDetails { get; set; } = "";
        public decimal Latitude { get; set; } = 0;
        public decimal Longitude { get; set; } = 0;
    }
}