using Layex;
using System;

namespace Atom.Client
{
    public sealed class CompositeDependencyContainer : BuiltinDependencyContainer
    {
        private readonly Design.Services.ServiceProvider _designServices;

        public CompositeDependencyContainer(Design.Services.ServiceProvider designServices)
        {
            _designServices = designServices;
        }

        protected override bool TryGetRegistration(Type type, string key, out IRegistration registration)
        {
            if (!base.TryGetRegistration(type, key, out registration))
            {
                object service = _designServices.GetService(type);
                if (service != null)
                {
                    registration = new InstanceRegistration(service);
                }
            }
            return registration != null;
        }
    }
}
