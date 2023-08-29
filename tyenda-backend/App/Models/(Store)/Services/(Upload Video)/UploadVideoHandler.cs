using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.File_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Store_.Services._Upload_Video_
{
    public class UploadVideoHandler : IRequestHandler<UploadVideo, string>
    {
        private readonly ITokenService _tokenService;
        private readonly TyendaContext _context;
        private readonly IFileService _fileService;

        public UploadVideoHandler(ITokenService tokenService, TyendaContext context, IFileService fileService)
        {
            _tokenService = tokenService;
            _context = context;
            _fileService = fileService;
        }

        public async Task<string> Handle(UploadVideo request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts.SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null) throw new Exception("Account not found");

                var store = await _context.Stores.SingleOrDefaultAsync(store => store.AccountId == account.Id, cancellationToken);
                if (store == null) throw new Exception("Store not found");
                
                var fileUrl = _fileService.UploadFile(request.File, "Videos", accountId);
                store.VideoUrl = fileUrl;

                await _context.SaveChangesAsync(cancellationToken);

                return fileUrl;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}