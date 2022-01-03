using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSettingsManager.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TSettingsModel ConfigureAppSettings<TSettingsModel>(this IServiceCollection services, IConfiguration configuration)
            where TSettingsModel : class, new()              
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            var settingsModel = new TSettingsModel();
            configuration.Bind(settingsModel);

            services.AddSingleton(settingsModel);

            return settingsModel;
        }
    }
}
