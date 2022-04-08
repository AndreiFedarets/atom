using System.Diagnostics;

namespace Atom.Runtime.Extension.Desktop
{
    public sealed class ExecutableApplication : Application
    {
        public ExecutableApplication(string fileFullName)
        {
            FileFullName = fileFullName;
        }

        public string FileFullName { get; private set; }

        public override Process Start()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(FileFullName);
            process.Start();
            return process;
        }
    }
}
