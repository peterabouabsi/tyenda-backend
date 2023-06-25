using System;
using System.ComponentModel.DataAnnotations;

namespace tyenda_backend.App.Models._Size_
{
    public class Size
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";
    }
}