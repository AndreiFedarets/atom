namespace Atom
{
    public static class LoggingProvider
    {
        static LoggingProvider()
        {
            Current = new FileLogger();
            EventLogger = new EventLogger();
        }

        public static ILogger Current { get; private set; }

        public static EventLogger EventLogger { get; private set; }
    }
}
