using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atom.Design
{
    internal sealed class ProjectFolder : ReadOnlyCollection<IProjectItem>, IProjectFolder
    {
        private ProjectFolder(string fullName)
            : base(new List<IProjectItem>())
        {
            FullName = fullName;
            Path = System.IO.Path.GetDirectoryName(fullName);
            Name = System.IO.Path.GetFileName(fullName);
        }

        public void Delete()
        {
            //DirectoryInfo.Delete(true);
            throw new NotImplementedException();
        }

        public string Name { get; set; }

        public string Path { get; private set; }

        public string FullName { get; private set; }

        public void Sort()
        {
            //TODO: I don't like this algorithm
            List<IProjectFolder> folders = Items.OfType<IProjectFolder>().ToList();
            List<IProjectFile> files = Items.OfType<IProjectFile>().ToList();
            folders.Sort((x,y) => String.CompareOrdinal(x.Name, y.Name));
            files.Sort((x, y) => String.CompareOrdinal(x.Name, y.Name));
            foreach (IProjectFolder folder in folders)
            {
                Items.Add(folder);
            }
            foreach (IProjectFile file in files)
            {
                Items.Add(file);
            }
        }

        //private List<IProjectItem> LoadItems()
        //{
        //    List<IProjectItem> items = new List<IProjectItem>();
        //    //Step #1: split all files on direct and indirect children
        //    List<ProjectFileMetadata> directChildrenMetadata = new List<ProjectFileMetadata>();
        //    List<ProjectFileMetadata> indirectChildrenMetadata = new List<ProjectFileMetadata>();
        //    foreach (ProjectFileMetadata fileMetadata in _childrenMetadata)
        //    {
        //        if (IsDirectChild(fileMetadata))
        //        {
        //            directChildrenMetadata.Add(fileMetadata);
        //        }
        //        else
        //        {
        //            indirectChildrenMetadata.Add(fileMetadata);
        //        }
        //    }
        //    foreach (ProjectFileMetadata directChildMetadata in directChildrenMetadata)
        //    {
        //        IProjectFile projectFile = new ProjectFile(directChildMetadata);
        //        items.Add(projectFile);
        //    }

        //    foreach (FileSystemInfo fileSystemInfo in DirectoryInfo.GetFileSystemInfos())
        //    {
        //        if (fileSystemInfo is FileInfo)
        //        {
        //            ProjectFileMetadata metadata = Metadata.Files.FirstOrDefault(x => ProjectItemMetadata.NamesEquals(x.Name, fileSystemInfo.Name));
        //            IProjectFile item = new ProjectFile(this, metadata, (FileInfo)fileSystemInfo);
        //            items.Add(item);
        //        }
        //        else if (fileSystemInfo is DirectoryInfo)
        //        {
        //            ProjectFolderMetadata metadata = Metadata.Folders.FirstOrDefault(x => ProjectItemMetadata.NamesEquals(x.Name, fileSystemInfo.Name));
        //            IProjectFolder item = new ProjectFolder(this, metadata, (DirectoryInfo)fileSystemInfo);
        //            items.Add(item);
        //        }
        //    }
        //    return items;
        //}

        //private List<IProjectFolder> LoadFolders()
        //{
            
        //}

        //private bool IsDirectChild(ProjectFileMetadata fileMetadata)
        //{
        //    string relativePath = string.IsNullOrWhiteSpace(fileMetadata.Link) ? fileMetadata.Include : fileMetadata.Link;
        //    string fileFullName = _pathResolver.Resolve(relativePath);
        //    string filePath = Path.GetDirectoryName(fileFullName);
        //    return string.Equals(DirectoryInfo.FullName, filePath, StringComparison.OrdinalIgnoreCase);
        //}

        //private List<IProjectItem> LoadFiles()
        //{
        //    foreach (ProjectFileMetadata fileMetadata in _childrenMetadata)
        //    {
                
        //    }
        //}

        internal IProjectFolder BuildFolder(string projectPath, ProjectMetadata projectMetadata)
        {
            //TODO: we also should handle empty Folders in the project
            IRelativePathResolver pathResolver = new RelativePathResolver(projectPath);
            List<IProjectFile> allFiles = new List<IProjectFile>();
            foreach (ProjectFileMetadata fileMetadata in projectMetadata.Files)
            {
                IProjectFile file = new ProjectFile(fileMetadata, pathResolver);
                allFiles.Add(file);
            }
            ProjectFolder projectFolder = new ProjectFolder(projectPath);
            foreach (IProjectFile file in allFiles)
            {
                string fileRelativePath = pathResolver.ToRelative(file.Path);
                ProjectFolder folder = GetOrCreateFolder(projectFolder, fileRelativePath);
                folder.Items.Add(file);
            }
            SortFolderItemsRecursively(projectFolder);
            return projectFolder;
        }

        private static void SortFolderItemsRecursively(IProjectFolder folder)
        {
            folder.Sort();
            foreach (IProjectItem item in folder)
            {
                IProjectFolder childFolder = item as IProjectFolder;
                if (childFolder != null)
                {
                    SortFolderItemsRecursively(childFolder);
                }
                else
                {
                    //items in the folder are already sorted, we reached file, then the rest of items are files
                    break;
                }
            }
        }

        private static ProjectFolder GetOrCreateFolder(ProjectFolder projectFolder, string relativePath)
        {
            string[] relativePathParts = relativePath.Split(System.IO.Path.PathSeparator);

            ProjectFolder parentFolder = projectFolder;
            ProjectFolder currentFolder = null;
            for (int i = 0; i < relativePathParts.Length; i++)
            {
                string currentFolderName = relativePathParts[i];
                foreach (IProjectItem item in parentFolder)
                {
                    ProjectFolder folder = item as ProjectFolder;
                    if (folder != null && string.Equals(currentFolderName, folder.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        currentFolder = (ProjectFolder) item;
                        break;
                    }
                }
                if (currentFolder == null)
                {
                    string currentFolderFullName = System.IO.Path.Combine(parentFolder.FullName, currentFolderName);
                    currentFolder = new ProjectFolder(currentFolderFullName);
                    parentFolder.Items.Add(currentFolder);
                }
                parentFolder = currentFolder;
                currentFolder = null;
            }
            return parentFolder;
        }
    }
}
