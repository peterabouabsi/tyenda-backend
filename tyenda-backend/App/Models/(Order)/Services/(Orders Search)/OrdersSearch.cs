using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Order_.Services._Orders_Search_.Form;

namespace tyenda_backend.App.Models._Order_.Services._Orders_Search_
{
    public class OrdersSearch : IRequest<ICollection<Order>>
    {
        public OrdersSearch(SearchForm searchForm)
        {
            SearchForm = searchForm;
        }

        public SearchForm SearchForm { get; set; }
    }
}