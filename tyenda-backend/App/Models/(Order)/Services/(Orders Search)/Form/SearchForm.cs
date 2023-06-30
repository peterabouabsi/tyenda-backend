using System;
using System.Collections.Generic;
using tyenda_backend.App.Models.Enums;

namespace tyenda_backend.App.Models._Order_.Services._Orders_Search_.Form
{
    public class SearchForm
    {
        public string? Keyword { get; set; } = "";
        public string? Reference { get; set; } = "";
        public DateTime? MinDate { get; set; }
        public IList<OrderStatus> OrderStatuses { get; set; } = new List<OrderStatus>();
    }
}