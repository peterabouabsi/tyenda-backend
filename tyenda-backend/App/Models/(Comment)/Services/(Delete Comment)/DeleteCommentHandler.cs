using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Comment_.Services._Delete_Comment_
{
    public class DeleteCommentHandler : IRequestHandler<DeleteComment, bool>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;

        public DeleteCommentHandler(TyendaContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> Handle(DeleteComment request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = Guid.Parse(_tokenService.GetHeaderTokenClaim(Constants.AccountId));
                var role = _tokenService.GetHeaderTokenClaim(Constants.Role);
                var commentId = Guid.Parse(request.CommentId);
                
                if (role == Constants.CustomerRole)
                {
                    var customer = await _context.Customers.SingleOrDefaultAsync(customer => customer.AccountId == accountId, cancellationToken);
                    var comment = await _context.Comments.SingleOrDefaultAsync(comment => comment.Id == commentId, cancellationToken);
                    
                    if (comment == null)
                    {
                        throw new Exception("Comment not found");
                    }

                    if (comment.CustomerId != customer!.Id)
                    {
                        throw new Exception("Sorry! the comment you are trying to delete is not posted by you.");
                    }

                    await Task.FromResult(_context.Comments.Remove(comment));
                }

                if (role == Constants.StoreRole)
                {
                    var store = await _context.Stores.SingleOrDefaultAsync(customer => customer.AccountId == accountId, cancellationToken);
                    var comment = await _context.Comments
                        .Include(comment => comment.Item)
                        .SingleOrDefaultAsync(comment => comment.Id == commentId && comment.Item!.StoreId == store!.Id, cancellationToken);
                    if (comment == null)
                    {
                        throw new Exception("Comment not found");
                    } 
                    
                    await Task.FromResult(_context.Comments.Remove(comment));
                }

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