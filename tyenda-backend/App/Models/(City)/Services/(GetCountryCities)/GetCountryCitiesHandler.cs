using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._City_.Services._GetCountryCities_
{
    public class GetCountryCitiesHandler : IRequestHandler<GetCountryCities, ICollection<City>>
    {
        private readonly TyendaContext _context;

        public GetCountryCitiesHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<ICollection<City>> Handle(GetCountryCities request, CancellationToken cancellationToken)
        {
            try
            {
                var countryId = Guid.Parse(request.CountryId);
                return await _context.Cities.Where(city => city.CountryId == countryId).ToArrayAsync(cancellationToken);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}