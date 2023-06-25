using System;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._Account_;
using TyendaBackend.App.Models._Account_;

namespace tyenda_backend.App.Models._Token_
{
    public class Token
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";
        public DateTime ExpiresIn { get; set; }

        public virtual Account? Account { get; set; }
        public Guid AccountId { get; set; }
    }
}