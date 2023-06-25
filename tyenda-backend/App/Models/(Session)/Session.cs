using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Session_
{
    public class Session
    {
        [Key]
        public Guid Id { get; set; }
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime ExpiresIn { get; set; }

        public virtual Account? Account { get; set; }
        public Guid AccountId { get; set; }
    }
}