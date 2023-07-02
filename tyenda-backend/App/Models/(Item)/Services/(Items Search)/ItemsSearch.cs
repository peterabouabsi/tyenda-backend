using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models.Form;

namespace tyenda_backend.App.Models._Item_.Services._Items_Search_
{
    public class ItemsSearch : IRequest<ICollection<Item>>
    {
        public ItemsSearch(ItemStoreSearchForm searchForm)
        {
            SearchForm = searchForm;
        }

        public ItemStoreSearchForm SearchForm { get; set; }
    }
}