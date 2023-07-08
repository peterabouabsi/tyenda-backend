using MediatR;

namespace tyenda_backend.App.Models._Store_.Services._Get_Store_Name_
{
    public class GetStoreName : IRequest<string>
    {
        public GetStoreName(string storeId)
        {
            StoreId = storeId;
        }

        public string StoreId { get; set; }
    }
}