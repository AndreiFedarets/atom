using Atom.Design.Hosting;
using System;
using System.Collections.Generic;

namespace Atom.Design.Reflection.Code.Services
{
    public sealed class CommonCodeParser : ICodeParser
    {
        private readonly Dictionary<Type, ICodeParser> _parsers;

        public CommonCodeParser()
        {
            _parsers = new Dictionary<Type, ICodeParser>();
            AddParser<IAction>(new ActionCodeParser());
            AddParser<ICondition>(new ConditionCodeParser());
            AddParser<IWorkflow>(new WorkflowCodeParser());
            AddParser<ITable>(new TableCodeParser());
        }

        public void AddParser<T>(ICodeParser parser)
        {
            _parsers.Add(typeof(T), parser);
        }

        public IList<T> Parse<T>(IProject project)
        {
            List<T> collection = new List<T>();
            ICodeParser specificParser;
            if (!_parsers.TryGetValue(typeof(T), out specificParser))
            {
                return collection;
            }
            foreach (IDocument document in project.Documents)
            {
                if (document.DocumentType == DocumentType.Code)
                {
                    collection.AddRange(specificParser.Parse<T>(document));
                }
            }
            return collection;
        }

        public IList<T> Parse<T>(IDocument document)
        {
            ICodeParser specificParser;
            if (document.DocumentType == DocumentType.Code && _parsers.TryGetValue(typeof(T), out specificParser))
            {
                return specificParser.Parse<T>(document);
            }
            return new List<T>();
        }
    }
}
