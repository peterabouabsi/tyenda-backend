using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Order_.Views;

namespace tyenda_backend.App.Models._Order_.Services._Get_Orders_Overview_
{
    public class GetOrdersOverview : IRequest<ICollection<OrderCountView>>
    {
    }
}