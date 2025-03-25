using Domain.Base;

namespace Application.Interfaces.Email
{
    public interface IEmailService
    {
        public Task<Operation> SendEmail(string to, string subject, string body, bool isHtml = false);
    }
}
