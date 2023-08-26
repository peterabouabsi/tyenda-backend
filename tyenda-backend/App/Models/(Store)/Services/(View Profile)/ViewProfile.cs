using MediatR;
using tyenda_backend.App.Models._Store_.Views;

namespace tyenda_backend.App.Models._Store_.Services._View_Profile_
{
    public class ViewProfile : IRequest<StoreAdvancedView>
    {
        public ViewProfile(string? storeId)
        {
            StoreId = storeId;
        }

        public string? StoreId { get; set; }
    }
}