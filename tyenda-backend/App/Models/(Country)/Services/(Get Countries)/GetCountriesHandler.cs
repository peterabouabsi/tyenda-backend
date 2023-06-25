using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Country_.Services._Get_Countries_
{
    public class GetCountriesHandler : IRequestHandler<GetCountries, ICollection<Country>>
    {
        private readonly TyendaContext _context;

        public GetCountriesHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Country>> Handle(GetCountries request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Countries.ToArrayAsync(cancellationToken);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}