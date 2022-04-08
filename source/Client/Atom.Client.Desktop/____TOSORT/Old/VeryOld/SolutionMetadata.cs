using System;

namespace Atom
{
    internal sealed class SolutionMetadata : IProjectMetadata
    {
        public IActionType ResolveActionType(Guid actionTypeUid)
        {
            throw new NotImplementedException();
        }

        public void RegisterActionType(IActionType actionType)
        {
            throw new NotImplementedException();
        }

        public IActionInstance CreateActionInstance(IActionType actionType)
        {
            throw new NotImplementedException();
        }

        public IActionInstance CreateActionInstance(Guid instanceUid, Guid typeUid)
        {
            throw new NotImplementedException();
        }


        public IActionInstance CreateActionInstance(ActionInstanceMetadata instanceUid)
        {
            throw new NotImplementedException();
        }
    }
}
