using System;

namespace Atom.Client.VisualStudio
{
    internal static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            T service = (T)serviceProvider.GetService(typeof(T));
            return service;
        }
    }
}
