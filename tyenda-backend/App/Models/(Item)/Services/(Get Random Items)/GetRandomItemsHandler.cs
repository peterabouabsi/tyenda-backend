using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models.Configs;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Item_.Services._Get_Random_Items_
{
    public class GetRandomItemsHandler : IRequestHandler<GetRandomItems, PagerDataConfig<ItemBasicView>>
    {
        private readonly TyendaContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public GetRandomItemsHandler(TyendaContext context, IMapper mapper, ITokenService tokenService)
        {
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<PagerDataConfig<ItemBasicView>> Handle(GetRandomItems request, CancellationToken cancellationToken)
        {
            try
            {
                var top = request.Top;
                var skip = request.Skip;

                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == Guid.Parse(accountId), cancellationToken);


                var data = await _context.Items
                    .Include(item => item.Images)
                    .Include(item => item.Rates)
                    .Include(item => item.Carts.Where(cart => cart.CustomerId == customer!.Id))
                    .Include(item => item.Store)
                    .ThenInclude(store => store!.Account)
                    .Include(item => item.Likes.Where(like => like.CustomerId == customer!.Id))
                    .OrderBy(item => item.Id)
                    .Skip(skip)
                    .Take(top)
                    .ToArrayAsync(cancellationToken);
                
                var count = await _context.Items.CountAsync(cancellationToken);
                
                var items = _mapper.Map<ICollection<ItemBasicView>>(data);

                return new PagerDataConfig<ItemBasicView>()
                {
                    Data = items,
                    Count = count
                };
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}