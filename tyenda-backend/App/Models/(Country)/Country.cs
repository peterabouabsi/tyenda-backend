using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tyenda_backend.App.Models._City_;

namespace tyenda_backend.App.Models._Country_
{
    public class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }
        
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; } = "";

        public virtual ICollection<City> Cities { get; set; }
    }
}