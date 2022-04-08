using System;
using System.IO;
using System.Linq;

namespace Atom.Design
{
    internal abstract class ProjectItem : IProjectItem
    {
        public abstract string Name { get; set; }

        public void Exclude()
        {
            if (Parent.Contains(this))
            {
                throw new NotImplementedException();
                //Parent.Remove()
            }
        }

        public void Include()
        {
            throw new NotImplementedException();
        }

        public abstract void Delete();

        protected virtual void Rename(string name)
        {
            string path = Path.GetDirectoryName(FileSystemInfo.FullName);
            if (path == null)
            {
                return;
            }
            string newFullName = Path.Combine(path, name);
            //Both FileInfo and DirectoryInfo have MoveTo(string) method,
            //but it's in the parent class FileSystemInfo for some reasons
            //So, to do not duplicate code use the hack
            dynamic d = FileSystemInfo;
            d.MoveTo(newFullName);
        }
    }
}
