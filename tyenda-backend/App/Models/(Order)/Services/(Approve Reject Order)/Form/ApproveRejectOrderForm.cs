using MediatR;

namespace tyenda_backend.App.Models._Order_.Services._Approve_Reject_Order_.Form
{
    public class ApproveRejectOrderForm
    {
        public string OrderId { get; set; } = "";
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
    }
}