using System;
using System.IO;

namespace Atom.Design
{
    internal sealed class ProjectFile : IProjectFile
    {
        private readonly IRelativePathResolver _pathResolver;

        public ProjectFile(ProjectFileMetadata metadata, IRelativePathResolver pathResolver)
        {
            Metadata = metadata;
            _pathResolver = pathResolver;
        }

        public void Delete()
        {
            
        }

        public ProjectFileMetadata Metadata { get; private set; }

        public string Name
        {
            get
            {
                string relativePath = string.IsNullOrWhiteSpace(Metadata.Link) ? Metadata.Include : Metadata.Link;
                return System.IO.Path.GetFileName(relativePath);
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public string Path
        {
            get { return System.IO.Path.GetDirectoryName(FullName); }
        }


        public string FullName
        {
            get
            {
                string relativePath = string.IsNullOrWhiteSpace(Metadata.Link) ? Metadata.Include : Metadata.Link;
                return _pathResolver.ToAbsolute(relativePath);
            }
        }
    }
}
