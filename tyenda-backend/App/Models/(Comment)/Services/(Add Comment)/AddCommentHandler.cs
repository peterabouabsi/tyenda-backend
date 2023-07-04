using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Comment_.Services._Add_Comment_
{
    public class AddCommentHandler : IRequestHandler<AddComment, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public AddCommentHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(AddComment request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = Guid.Parse(_tokenService.GetHeaderTokenClaim(Constants.AccountId));
                var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == accountId,cancellationToken);
                if (customer == null)
                {
                    throw new Exception("Customer not found");
                }

                var item = await _context.Items.SingleOrDefaultAsync(item => item.Id == Guid.Parse(request.AddCommentForm.ItemId),cancellationToken);
                if (item == null)
                {
                    throw new Exception("Item not found");
                }

                var newComment = new Comment()
                {
                    Id = Guid.NewGuid(),
                    ItemId = item.Id,
                    CustomerId = customer.Id,
                    Value = request.AddCommentForm.Comment,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Comments.AddAsync(newComment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}