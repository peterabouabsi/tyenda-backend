using System.Threading.Tasks;

namespace tyenda_backend.App.Services.Email_Service
{
    public interface IEmailService
    {
        public abstract Task SendEmailAsync(string to, string subject, string bodyType, string body);
    }
}