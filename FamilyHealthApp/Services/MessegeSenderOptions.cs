
namespace FamilyHealthApp.Services
{
 
    // Options for SendGrid, stored as UserSecrets
    public class MessegeSenderOptions
    {
        public string SendGridApiKey { get; set; }
        public string SendGridAzurePassword { get; set; }
    }

}
