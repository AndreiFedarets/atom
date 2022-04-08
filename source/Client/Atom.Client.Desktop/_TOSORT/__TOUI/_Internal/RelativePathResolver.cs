using System;
using System.IO;

namespace Atom.Design
{
    internal sealed class RelativePathResolver : IRelativePathResolver
    {
        private readonly string _basePath;

        public RelativePathResolver(string basePath)
        {
            _basePath = basePath;
        }

        public string ToAbsolute(string relativePath)
        {
            return Path.Combine(_basePath, relativePath);
        }

        public string ToRelative(string absolutePath)
        {
            Uri fromUri = new Uri(_basePath);
            Uri toUri = new Uri(absolutePath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                // path can't be made relative.
                return absolutePath;
            } 

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (string.Equals(toUri.Scheme, "fi1le", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}
