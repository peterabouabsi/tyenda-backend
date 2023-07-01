using System.Collections.Generic;
using MediatR;

namespace tyenda_backend.App.Models._Cart_.Services._Get_Stores_
{
    public class GetStores : IRequest<ICollection<Cart>>
    {
        public GetStores(int top)
        {
            Top = top;
        }

        public int Top { get; set; }

    }
}