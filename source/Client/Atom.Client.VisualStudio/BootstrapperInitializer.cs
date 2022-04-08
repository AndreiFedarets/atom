using Atom.Design.Common;
using System;
using System.Runtime.CompilerServices;

namespace Atom.Client.VisualStudio
{
    public sealed class BootstrapperInitializer : MarshalByRefObject, IDisposable
    {
        private readonly DesignAssemblyResover _assemblyResolver;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public BootstrapperInitializer()
        {
            _assemblyResolver = new DesignAssemblyResover();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Initialize(IVisualStudioPackage package)
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Initialize(package);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Dispose()
        {
            _assemblyResolver.Dispose();
        }
    }
}
