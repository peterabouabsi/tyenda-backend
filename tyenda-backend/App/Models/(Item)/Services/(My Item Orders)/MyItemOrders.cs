using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Order_;

namespace tyenda_backend.App.Models._Item_.Services._My_Item_Orders_
{
    public class MyItemOrders : IRequest<ICollection<Order>>
    {
        public MyItemOrders(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
}