using MediatR;
using tyenda_backend.App.Models._Order_.Services._Approve_Reject_Order_.Form;
using tyenda_backend.App.Models._Order_.Services._Confirm_Order_.Form;

namespace tyenda_backend.App.Models._Order_.Services._Approve_Reject_Order_
{
    public class ApproveRejectOrder : IRequest<Order>
    {
        public ApproveRejectOrder(ApproveRejectOrderForm approveRejectOrderForm)
        {
            ApproveRejectOrderForm = approveRejectOrderForm;
        }

        public ApproveRejectOrderForm ApproveRejectOrderForm { get; set; }
    }
}