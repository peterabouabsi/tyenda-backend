using MediatR;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models.Configs;

namespace tyenda_backend.App.Models._Item_.Services._Get_Random_Items_
{
    public class GetRandomItems : IRequest<PagerDataConfig<ItemBasicView>>
    {
        public GetRandomItems(int top, int skip)
        {
            Top = top;
            Skip = skip;
        }

        public int Skip { get; set; }
        public int Top { get; set; }
    }
}