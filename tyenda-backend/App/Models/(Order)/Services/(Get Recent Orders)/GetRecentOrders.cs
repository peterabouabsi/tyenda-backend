using System.Collections.Generic;
using MediatR;

namespace tyenda_backend.App.Models._Order_.Services._Get_Recent_Orders_
{
    public class GetRecentOrders : IRequest<ICollection<Order>>
    {
    }
}