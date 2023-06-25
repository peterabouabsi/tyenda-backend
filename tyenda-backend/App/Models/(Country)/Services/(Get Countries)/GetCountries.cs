using System.Collections.Generic;
using MediatR;

namespace tyenda_backend.App.Models._Country_.Services._Get_Countries_
{
    public class GetCountries : IRequest<ICollection<Country>>
    {
    }
}