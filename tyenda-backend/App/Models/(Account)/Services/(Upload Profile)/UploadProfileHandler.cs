using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using tyenda_backend.App.Context;
using tyenda_backend.App.Services.File_Service;
using tyenda_backend.App.Services.Token_Service;

namespace tyenda_backend.App.Models._Account_.Services._Upload_Profile_
{
    public class UploadProfileHandler : IRequestHandler<UploadProfile, string>
    {
        private readonly TyendaContext _context;
        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;

        public UploadProfileHandler(TyendaContext context, ITokenService tokenService, IFileService fileService)
        {
            _context = context;
            _tokenService = tokenService;
            _fileService = fileService;
        }

        public async Task<string> Handle(UploadProfile request, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = _tokenService.GetHeaderTokenClaim(Constants.AccountId);
                var account = await _context.Accounts
                    .Include(account => account.Role)
                    .Include(account => account.Store)
                    .SingleOrDefaultAsync(account => account.Id == Guid.Parse(accountId), cancellationToken);
                if (account == null)
                {
                    throw new UnauthorizedAccessException("Account not found");
                }

                
                if (account.Role!.Value == Constants.CustomerRole)
                {
                    var fileUrl = _fileService.UploadFile(request.File, "Profiles", accountId);
                    
                    account.ProfileImage = fileUrl;
                    account.CreatedAt = account.CreatedAt.ToUniversalTime();
                    await Task.FromResult(_context.Accounts.Update(account));

                    await _context.SaveChangesAsync(cancellationToken);
                    
                    return fileUrl;

                }
                if (account.Role!.Value == Constants.StoreRole)
                {
                    if (request.Folder == "Profile")
                    {
                        var fileUrl = _fileService.UploadFile(request.File, "Profiles", accountId);

                        account.ProfileImage = fileUrl;
                        account.CreatedAt = account.CreatedAt.ToUniversalTime();
                        await Task.FromResult(_context.Accounts.Update(account));
                        
                        await _context.SaveChangesAsync(cancellationToken);
                        
                        return fileUrl;

                    }
                    else if (request.Folder == "Background")
                    {
                        var fileUrl = _fileService.UploadFile(request.File, "Backgrounds", accountId);

                        account.Store!.BackgroundImage = fileUrl;
                        account.CreatedAt = account.CreatedAt.ToUniversalTime();
                        
                        await Task.FromResult(_context.Accounts.Update(account));
                        
                        await _context.SaveChangesAsync(cancellationToken);

                        return fileUrl;
                    }
                }

                throw new Exception("Invalid Folder");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}