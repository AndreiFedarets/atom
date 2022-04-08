using System;

namespace Atom
{
    public interface IProjectMetadata
    {
        IActionType ResolveActionType(Guid actionTypeUid);

        void RegisterActionType(IActionType actionType);

        IActionInstance CreateActionInstance(ActionInstanceMetadata instanceUid);
    }

    //public static class ProjectMetadataExtensions
    //{
    //    public static IActionInstance CreateActionInstance(this IProjectMetadata metadata, IActionType actionType)
    //    {
    //        return metadata.CreateActionInstance(Guid.NewGuid(), actionType.Uid);
    //    }
    //}
}
