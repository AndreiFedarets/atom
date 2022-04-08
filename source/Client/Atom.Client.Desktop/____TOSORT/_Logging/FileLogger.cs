using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Atom
{
    public sealed class FileLogger : ILogger
    {
        private readonly object _lock;
        private StreamWriter _writer;

        public FileLogger()
        {
            _lock = new object();
        }

        public SourceLevels MinimalSourceLevels
        {
            get { return SourceLevels.Warning; }
        }

        public bool ShouldLog(TraceEventType eventType)
        {
            switch (MinimalSourceLevels)
            {
                case SourceLevels.All:
                    return true;
                case SourceLevels.Critical:
                    return (int)eventType <= (int)TraceEventType.Critical;
                case SourceLevels.Error:
                    return (int)eventType <= (int)TraceEventType.Error;
                case SourceLevels.Warning:
                    return (int)eventType <= (int)TraceEventType.Warning;
                case SourceLevels.Information:
                    return (int)eventType <= (int)TraceEventType.Information;
                case SourceLevels.Verbose:
                    return (int)eventType <= (int)TraceEventType.Verbose;
            }
            return false;
        }

        public void Log(TraceEventType eventType, string source, string message)
        {
            if (!ShouldLog(eventType))
            {
                return;
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("EventType: {0}{1}", eventType, Environment.NewLine);
            builder.AppendFormat("Source: {0}{1}", source, Environment.NewLine);
            builder.AppendFormat("Message: {0}{1}", message, Environment.NewLine);
            builder.AppendLine("====================================================================================");
            StreamWriter writer = GetWriter();
            writer.Write(builder);
            writer.Flush();
        }

        private StreamWriter GetWriter()
        {
            lock (_lock)
            {
                if (_writer == null)
                {
                    string path = @"C:\Atom";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName;
                    using (Process process = Process.GetCurrentProcess())
                    {
                        fileName = string.Format("{0}.{1}.log", process.ProcessName, Environment.TickCount.ToString(CultureInfo.InvariantCulture));
                    }
                    _writer = new StreamWriter(Path.Combine(path, fileName));
                }
                return _writer;
            }
        }
    }
}
