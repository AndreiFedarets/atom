using System;
using System.IO;
using System.Linq;

namespace Atom.Design
{
    internal sealed class Solution : ISolution
    {
        private readonly SolutionMetadata _metadata;
        private readonly ISolutionSerializer _serializer;

        private Solution(SolutionMetadata metadata, ISolutionSerializer serializer)
        {
            _metadata = metadata;
            _serializer = serializer;
            Projects = new ProjectCollection(this);
        }

        public string Name
        {
            get { return _metadata.Name; }
            set
            {
                //string oldName = Name;
                //string newFullName = System.IO.Path.Combine(Path, value);
                //_solutionFile.MoveTo(newFullName);
                ////TODO: do we need it?
                //_solutionFile = new FileInfo(newFullName);
                //NameChangedEventArgs.RaiseEvent(NameChanged, this, oldName, Name);
            }
        }

        public IProjectCollection Projects { get; private set; }

        internal static Solution Create(string path, string name)
        {
            //TODO: bad design, direct instantiating of services, direct access to FS, hardcode
            ISolutionSerializer serializer = new SolutionSerializer();
            FileName fileName = new FileName(path, name, ".sln");
            ProjectCollectionMetadata projectsMetadata = new ProjectCollectionMetadata(Enumerable.Empty<ProjectMetadata>());
            SolutionMetadata metadata = new SolutionMetadata(fileName.FullName, projectsMetadata);
            using (Stream stream = File.OpenWrite(fileName.FullName))
            {
                serializer.Serialize(metadata, stream);
            }
            Solution solution = new Solution(metadata, serializer);
            return solution;
        }

        internal static Solution Open(string fileFullName)
        {
            throw Fail.NotImplemented();
        }

        public event EventHandler<NameChangedEventArgs> NameChanged;

        //SolutionInfo ISerializable<SolutionInfo>.Serialize()
        //{
        //    SolutionInfo solutionInfo = new SolutionInfo();
        //    solutionInfo.Projects = Projects.Serailize<ProjectInfoCollection>();
        //    return solutionInfo;
        //}

        //public static void Serialize(ISolution solution, string fileFullName)
        //{
        //    SolutionInfo solutionInfo = CreateInfoObject(solution);
        //    using (FileStream stream = new FileStream(fileFullName, FileMode.Create, FileAccess.Write))
        //    {
        //        using (StreamWriter writer = new StreamWriter(stream))
        //        {
        //            string serialized = JsonConvert.SerializeObject(solutionInfo);
        //            writer.Write(serialized);
        //        }
        //    }
        //    string path = Path.GetDirectoryName(fileFullName);
        //}

        //internal static ISolution Deserialize(string fileFullName, IApplication application)
        //{
        //    throw Fail.NotImplemented();
        //}

        //internal static SolutionInfo CreateInfoObject(ISolution solution)
        //{
        //    SolutionInfo solutionInfo = new SolutionInfo();
        //    solutionInfo.Projects = ProjectInfoCollection.CreateInfoObject(solution.Projects);
        //    return solutionInfo;
        //}
    }
}
