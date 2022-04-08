using System.Collections.Generic;

namespace Atom.Office
{
    public interface ICommonApplicationProvider<T> : IEnumerable<T> where T : ICommonApplication
    {
        int Count { get; }

        /// <summary>
        /// Start application of specific version and wait while it is starting
        /// </summary>
        /// <param name="version">Application version</param>
        /// <returns>Initialized application instance</returns>
        T Start(ApplicationVersion version);

        /// <summary>
        /// Start default application and wait while it is starting
        /// </summary>
        /// <returns>Initialized application instance</returns>
        T Start();

        void Close(T application);
    }
}
