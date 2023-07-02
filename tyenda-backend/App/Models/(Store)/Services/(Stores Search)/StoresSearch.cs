using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models.Form;

namespace tyenda_backend.App.Models._Store_.Services._Stores_Search_
{
    public class StoresSearch : IRequest<ICollection<Store>>
    {
        public StoresSearch(ItemStoreSearchForm searchForm)
        {
            SearchForm = searchForm;
        }

        public ItemStoreSearchForm SearchForm { get; set; }
    }
}