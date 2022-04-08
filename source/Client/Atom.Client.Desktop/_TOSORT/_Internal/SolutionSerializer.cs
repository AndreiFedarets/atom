using System;
using System.IO;
using System.Text;

namespace Atom.Design
{
    internal sealed class SolutionSerializer : ISolutionSerializer
    {
        private const string FileFormatVersionPlaceholder = "{FILE_FORMAT_VERSION}"; //FileFormatVersion
        private const string VisualStudioVersionPlaceholder = "{VISUAL_STUDIO_VERSION}"; //VisualStudioVersion
        private const string MinimumVisualStudioVersionPlaceholder = "{MINIMUM_VISUAL_STUDIO_VERSION}"; //MinimumVisualStudioVersion
        private const string ProjectsListPlaceholder = "{PROJECTS_LIST}";
        private const string SolutionPlatformsPlaceholder = "{SOLUTION_PLATFORMS}";
        private const string NestedProjectsPlaceholder = "{NESTED_PROJECTS}";
        private const string ProjectPlatformsPlaceholder = "{PROJECTS_PLATFORMS}";


        public SolutionMetadata Deserialize(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return Deserialize(reader);
        }

        public SolutionMetadata Deserialize(StreamReader reader)
        {
            throw new NotImplementedException();
        }

        public void Serialize(SolutionMetadata metadata, Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            Serialize(metadata, writer);
        }

        public void Serialize(SolutionMetadata metadata, StreamWriter writer)
        {
            string solution = Encoding.Default.GetString(Properties.Resources.SolutionTemplate);
            solution = solution.Replace(FileFormatVersionPlaceholder, VisualStudio2013.FileFormatVersion);
            solution = solution.Replace(VisualStudioVersionPlaceholder, VisualStudio2013.VisualStudioVersion);
            solution = solution.Replace(MinimumVisualStudioVersionPlaceholder, VisualStudio2013.MinimumVisualStudioVersion);
            solution = solution.Replace(ProjectsListPlaceholder, BuildProjectsList(metadata).ToString());
            solution = solution.Replace(SolutionPlatformsPlaceholder, BuildSolutionPlatforms(metadata).ToString());
            solution = solution.Replace(NestedProjectsPlaceholder, BuildNestedProjects(metadata).ToString());
            solution = solution.Replace(ProjectPlatformsPlaceholder, BuildProjectPlatforms(metadata).ToString());

            writer.Write(solution);
        }

        private StringBuilder BuildProjectsList(SolutionMetadata metadata)
        {
            throw new NotImplementedException();
        }

        private StringBuilder BuildSolutionPlatforms(SolutionMetadata metadata)
        {
            throw new NotImplementedException();
        }

        private StringBuilder BuildNestedProjects(SolutionMetadata metadata)
        {
            throw new NotImplementedException();
        }

        private StringBuilder BuildProjectPlatforms(SolutionMetadata metadata)
        {
            throw new NotImplementedException();
        }

        private static class VisualStudio2013
        {
            public const string FileFormatVersion = "12.00";
            public const string VisualStudioVersion = "12.0.21005.1";
            public const string MinimumVisualStudioVersion = "10.0.40219.1";
        }
    }
}
