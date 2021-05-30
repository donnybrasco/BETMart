﻿using Microsoft.Extensions.Configuration;

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

        #endregion
    }
}