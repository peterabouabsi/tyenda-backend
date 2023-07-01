using System.Collections.Generic;
using MediatR;

namespace tyenda_backend.App.Models._Cart_.Services._Get_Items_
{
    public class GetItems : IRequest<ICollection<Cart>>
    {
        public GetItems(int top)
        {
            Top = top;
        }

        public int Top { get; set; }
    }
}