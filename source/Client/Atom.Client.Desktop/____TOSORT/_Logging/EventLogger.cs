using System.Diagnostics;

namespace Atom
{
    public sealed class EventLogger : ILogger
    {
        private const string LogName = "Atom";

        public SourceLevels MinimalSourceLevels
        {
            get { return SourceLevels.All; }
        }

        public bool ShouldLog(TraceEventType eventType)
        {
            return (int)eventType <= (int)MinimalSourceLevels;
        }

        public void Log(TraceEventType eventType, string source, string message)
        {
            if (!ShouldLog(eventType))
            {
                return;
            }
            //if (!EventLog.SourceExists(source))
            //{
            //    EventLog.CreateEventSource(source, LogName);
            //}
            //EventLogEntryType entryType = ConvertTraceToEventLogType(eventType);
            //EventLog.WriteEntry(source, message, entryType);
        }

        private static EventLogEntryType ConvertTraceToEventLogType(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Critical:
                case TraceEventType.Error:
                    return EventLogEntryType.Error;
                case TraceEventType.Warning:
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}
