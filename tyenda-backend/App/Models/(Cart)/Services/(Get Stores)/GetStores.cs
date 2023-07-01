using System.Collections.Generic;
using MediatR;

namespace tyenda_backend.App.Models._Cart_.Services._Get_Stores_
{
    public class GetStores : IRequest<ICollection<Cart>>
    {
        public GetStores(int top, int skip)
        {
            Top = top;
            Skip = skip;
        }

        public int Top { get; set; }
        public int Skip { get; set; }

    }
}