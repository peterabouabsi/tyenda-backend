using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Item_;

namespace tyenda_backend.App.Models._Store_.Services._Top_Selling_Items_
{
    public class TopSellingItems : IRequest<ICollection<Item>>
    {
        public TopSellingItems(string storeId)
        {
            StoreId = storeId;
        }

        public string StoreId { get; set; }
    }
}