using System;
using System.Diagnostics;

namespace Atom
{
    public interface ILogger
    {
        SourceLevels MinimalSourceLevels { get; }

        bool ShouldLog(TraceEventType eventType);

        void Log(TraceEventType eventType, string source, string message);
    }

    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, TraceEventType eventType, string message)
        {
            logger.Log(eventType, string.Empty, message);
        }

        public static void Log(this ILogger logger, TraceEventType eventType, string message, params object[] args)
        {
            if (logger.ShouldLog(eventType))
            {
                logger.Log(eventType, string.Empty, string.Format(message, args));
            }
        }

        public static void Log(this ILogger logger, TraceEventType eventType, Exception exception)
        {
            logger.Log(eventType, string.Empty, exception.ToString());
        }
    }
}
