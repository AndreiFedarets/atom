using System;

namespace Atom.Design
{
    internal sealed class FileName
    {
        public FileName(string path, string name, string extension)
        {
            if (!string.IsNullOrEmpty(extension) && 
                string.Equals(extension, System.IO.Path.GetExtension(name), StringComparison.OrdinalIgnoreCase))
            {
                name = System.IO.Path.GetFileNameWithoutExtension(name);
            }
            Path = path;
            Name = name;
            Extension = extension;
        }

        public string Path { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string FullName
        {
            get
            {
                string path = System.IO.Path.Combine(Path, Name);
                if (!string.IsNullOrWhiteSpace(Extension))
                {
                    path += Extension;
                }
                return path;
            }
        }
    }
}
