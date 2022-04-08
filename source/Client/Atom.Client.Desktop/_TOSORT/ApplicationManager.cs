namespace Atom.Design
{
    public static class ApplicationManager
    {
        private static readonly object Lock;
        private static IApplication _application;

        static ApplicationManager()
        {
            Lock = new object();
        }

        public static IApplication RunApplication()
        {
            if (_application == null)
            {
                lock (Lock)
                {
                    if (_application == null)
                    {
                        Application application = new Application();
                        application.Run();
                        _application = application;
                    }
                }
            }
            return _application;
        }
    }
}
