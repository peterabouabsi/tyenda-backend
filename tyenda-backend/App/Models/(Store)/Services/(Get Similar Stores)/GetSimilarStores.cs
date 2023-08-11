using System.Collections.Generic;
using MediatR;

namespace tyenda_backend.App.Models._Store_.Services._Get_Similar_Stores_
{
    public class GetSimilarStores : IRequest<ICollection<Store>>
    {
        public GetSimilarStores(int? take)
        {
            Take = take;
        }

        public int? Take { get; set; }
    }
}