using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Atom.Extensibility;

namespace Atom
{
    internal sealed class ActionAssembly : IActionAssembly
    {
        private readonly IActionLocator _actionLocator;
        private readonly Lazy<IActionTypeCollection> _actions;

        public ActionAssembly(Assembly assembly, IActionLocator actionLocator, IReflectionFacade reflection)
        {
            Assembly = assembly;
            _actionLocator = actionLocator;
            _actions = new Lazy<IActionTypeCollection>(GetActions);
            Uid = reflection.GetAssemblyGuid(assembly);
            Name = reflection.GetAssemblyName(assembly);
        }

        public Guid Uid { get; private set; }

        public AssemblyName Name { get; private set; }

        public Assembly Assembly { get; private set; }

        public IActionTypeCollection Actions
        {
            get { return _actions.Value; }
        }

        private IActionTypeCollection GetActions()
        {
            IEnumerable<IActionType> actions = _actionLocator.LoadActions(this);
            IDictionary<Guid, IActionType> dictionary = actions.ToDictionary(x => x.Uid, x => x);
            return new ActionTypeCollection(dictionary);
        }
    }
}
