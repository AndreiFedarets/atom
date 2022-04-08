using Atom.Design.Reflection.Metadata;
using Atom.Design.Reflection.Metadata.Binary;
using Atom.Design.Services;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atom.Design.Extension.Common
{
    public sealed class StringTypeAdapter : TypeAdapter
    {
        public static readonly TypeReference TypeReference;

        static StringTypeAdapter()
        {
            TypeReference = MetadataProvider.GetReference(typeof(string));
        }

        public StringTypeAdapter()
            : base(TypeReference)
        {
        }

        public override IEnumerable<CodeStatement> GenerateCode(object value)
        {
            yield return new CodeMethodReturnStatement(new CodePrimitiveExpression((string)value));
        }

        public override object ReadValue(XElement valueElement)
        {
            return valueElement.Value;
        }

        public override void WriteValue(XElement valueElement, object value)
        {
            valueElement.Value = (string)value;
        }
    }
}
