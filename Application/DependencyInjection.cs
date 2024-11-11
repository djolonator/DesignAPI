using Application.Validations;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
           
            return services;
        }
    }
}
