using $safeprojectname$.Common.Email.Configurations;
using $safeprojectname$.Common.Email.Interfaces;
using $safeprojectname$.Common.Email.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace $safeprojectname$.Common.Email.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmtpEmailSender(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "Smtp")
        {
            services.Configure<SmtpEmailSettings>(configuration.GetRequiredSection(sectionName));
            services.AddSingleton<IEmailSender, SmtpEmailSender>();
            return services;
        }
    }
}
