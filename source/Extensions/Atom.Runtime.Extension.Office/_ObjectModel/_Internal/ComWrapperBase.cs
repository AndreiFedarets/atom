using System;
using System.Globalization;
using System.Reflection;

namespace Atom.Office
{
    internal abstract class ComWrapperBase<TCom> : IDisposable where TCom : class
    {
        private TCom _comObject;
        private bool _disposed;
        private readonly Type _comType;

        protected ComWrapperBase(TCom comObject)
        {
            _comObject = comObject;
            _comType = typeof(TCom);
        }

        ~ComWrapperBase()
        {
            Dispose(false);
        }

        public TCom ComObject
        {
            get { return _comObject; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //Here we must dispose managed resources
                }
                //Here we must dispose unmanaged resources and LOH objects
                ComFinalizer.Release(ref _comObject);
                _disposed = true;
            }
        }

        protected T InvokeMethod<T>(string name, CultureInfo cultureInfo, params object[] args)
        {
            return (T)InvokeMethod(name, cultureInfo, args);
        }

        protected object InvokeMethod(string name, CultureInfo cultureInfo, params object[] args)
        {
            return _comType.InvokeMember(name, BindingFlags.InvokeMethod, null, _comObject, args, cultureInfo);
        }
    }
}
