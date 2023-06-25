using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;

namespace tyenda_backend.App.Models._Category_.Services._GetCategories_
{
    public class GetCategoriesHandler : IRequestHandler<GetCategories, ICollection<Category>>
    {
        private readonly TyendaContext _context;

        public GetCategoriesHandler(TyendaContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Category>> Handle(GetCategories request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Categories.ToArrayAsync(cancellationToken);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}