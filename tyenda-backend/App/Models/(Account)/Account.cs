using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Alert_;
using tyenda_backend.App.Models._Customer_;
using tyenda_backend.App.Models._Role_;
using tyenda_backend.App.Models._Session_;
using tyenda_backend.App.Models._Store_;
using tyenda_backend.App.Models._Token_;

namespace tyenda_backend.App.Models._Account_
{
    public class Account
    {

        public Account()
        {
            Tokens = new HashSet<Token>();
            Alerts = new HashSet<Alert>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public string? ProfileImage { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; } = "";
        public string? Password { get; set; }
        public bool Active { get; set; } = false;

        public virtual Role? Role { get; set; }
        public Guid RoleId { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Store? Store { get; set; }
        public virtual Session? Session { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; }

    }
}