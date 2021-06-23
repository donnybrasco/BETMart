using Microsoft.Extensions.Configuration;

namespace BETMart.Infrastructure
{
    public interface ISettings
    {
    }

    public class Settings
        : ISettings
    {
        #region Ctor

        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Properties

        public string BETMartAPI => _configuration["AppSettings:BETMart.API"];

        //Mail Settings
        public string DefaultEmailTemplateFolder => _configuration["MailSettings:DefaultEmailTemplateFolder"];
        public string DisplayName => _configuration["MailSettings:DisplayName"];
        public string FromEmailAddress => _configuration["MailSettings:From"];
        public string Host => _configuration["MailSettings:Host"];
        public string UserName => _configuration["MailSettings:UserName"];
        public string Password => _configuration["MailSettings:Password"];

        #endregion
    }
}
