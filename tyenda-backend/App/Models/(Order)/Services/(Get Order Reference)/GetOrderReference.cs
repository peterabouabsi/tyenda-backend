using MediatR;

namespace tyenda_backend.App.Models._Order_.Services._Get_Order_Reference_
{
    public class GetOrderReference : IRequest<string>
    {
        public GetOrderReference(string orderId)
        {
            this.orderId = orderId;
        }

        public string orderId { get; set; }
    }
}