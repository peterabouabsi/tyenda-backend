using System.Collections.Generic;

namespace tyenda_backend.App.Models._Account_.Services._Store_Signup_.Form
{
    public class StoreSignupForm
    {
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string StoreName { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string OwnerEmail { get; set; } = "";
        public string Website { get; set; } = "";
        public string Description { get; set; } = "";
        public List<BranchForm> Branches { get; set; } = new List<BranchForm>();
        public List<string> CategoryIds { get; set; } = new List<string>();
    }

    public class BranchForm
    {
        public string CityId { get; set; } = "";
        public string AddressDetails { get; set; } = "";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}