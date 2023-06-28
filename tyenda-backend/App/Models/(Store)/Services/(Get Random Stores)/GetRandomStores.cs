using MediatR;
using tyenda_backend.App.Models._Store_.Views;
using tyenda_backend.App.Models.Configs;

namespace tyenda_backend.App.Models._Store_.Services._Get_Random_Stores_
{
    public class GetRandomStores : IRequest<PagerDataConfig<StoreModerateView>>
    {
        public GetRandomStores(int top, int skip)
        {
            Top = top;
            Skip = skip;
        }

        public int Skip { get; set; }
        public int Top { get; set; }
    }
}