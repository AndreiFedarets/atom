using System;

namespace Atom.Design
{
    public sealed class ProjectSettings
    {
        public ProjectSettings()
        {
            Version = new Version();
        }

        public string Output { get; set; }

        public Version Version { get; set; }
    }
}
