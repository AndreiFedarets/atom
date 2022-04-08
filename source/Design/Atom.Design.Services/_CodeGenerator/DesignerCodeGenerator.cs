using System;
using System.Collections.Generic;

namespace Atom.Design.Services
{
    public sealed class DesignerCodeGenerator : IDesignerCodeGenerator
    {
        private readonly Dictionary<Type, IDesignerCodeGenerator> _generators;

        public DesignerCodeGenerator()
        {
            _generators = new Dictionary<Type, IDesignerCodeGenerator>();
        }

        public void AddGenerator<T>(IDesignerCodeGenerator codeGenerator) where T : IObjectDesigner
        {
            _generators.Add(typeof(T), codeGenerator);
        }

        public string Generate(IObjectDesigner designer)
        {
            IDesignerCodeGenerator codeGenerator;
            if (!_generators.TryGetValue(designer.GetType(), out codeGenerator))
            {
                return string.Empty;
            }
            return codeGenerator.Generate(designer);
        }
    }
}
