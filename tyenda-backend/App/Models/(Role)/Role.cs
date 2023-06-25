using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Role_
{
    public class Role
    {

        public Role()
        {
            Accounts = new HashSet<Account>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";

        public virtual ICollection<Account> Accounts { get; set; }
    }
}