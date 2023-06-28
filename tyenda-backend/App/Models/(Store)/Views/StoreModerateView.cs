using System;

namespace tyenda_backend.App.Models._Store_.Views
{
    public class StoreModerateView
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string BackgroundImage { get; set; } = "";
        public string ProfileImage { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool IsFollowed { get; set; } = false;
        public bool IsAddedToCart { get; set; } = false;
    }
}