using System.Collections.Generic;

namespace tyenda_backend.App.Models._Account_.Services._Update_Profile_.Forms
{
    public class UpdateProfileForm
    {
        public UpdateCustomerForm? UpdateCustomerForm { get; set; }
        public UpdateStoreForm? UpdateStoreForm { get; set; }
    }
    
    public class UpdateCustomerForm
    {
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public bool OnItem { get; set; } = false;
        public bool OnOrder { get; set; } = false;
    }
    
    public class UpdateStoreForm
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Website { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Description { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string OwnerEmail { get; set; } = "";
        public List<string> CategoryIds { get; set; } = new List<string>();
    }
}