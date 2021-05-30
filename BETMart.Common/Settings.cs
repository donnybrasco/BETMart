using Microsoft.Extensions.Configuration;

namespace BETMart.Common
{
    public interface ISettings
    {
        string BETMartAPI { get; }
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

        public string BETMartAPI => _configuration[""];

        #endregion
    }
}
