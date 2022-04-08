using System;
using System.Collections.Generic;

namespace Atom.Design.Services
{
    public sealed class DesignerValidator : IDesignerValidator
    {
        private readonly Dictionary<Type, IDesignerValidator> _serializers;

        public DesignerValidator()
        {
            _serializers = new Dictionary<Type, IDesignerValidator>();
        }

        public void AddValidator<T>(IDesignerValidator validator)
        {
            _serializers.Add(typeof(T), validator);
        }
        
        public bool Validate(IObjectDesigner designer)
        {
            IDesignerValidator validator;
            if (!_serializers.TryGetValue(designer.GetType(), out validator))
            {
                return false;
            }
            return validator.Validate(designer);
        }
    }
}
