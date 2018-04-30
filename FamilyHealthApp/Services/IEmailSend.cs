using System.Threading.Tasks;

namespace FamilyHealthApp.Services
{
    // Interface for EmailSend
    public interface IEmailSend
    {
        Task SendEmailAsync(string email, string subject, string messege);
    }
}