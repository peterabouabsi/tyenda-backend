using System.Collections.Generic;
using MediatR;

namespace tyenda_backend.App.Models._City_.Services._GetCountryCities_
{
    public class GetCountryCities : IRequest<ICollection<City>>
    {
        public GetCountryCities(string countryId)
        {
            CountryId = countryId;
        }

        public string CountryId { get; set; }
        
    }
}