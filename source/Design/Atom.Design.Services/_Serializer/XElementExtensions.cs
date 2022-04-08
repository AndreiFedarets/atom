using System;
using System.Xml.Linq;

namespace Atom
{
    public static class XElementExtensions
    {
        public static T ReadAttribute<T>(this XElement parentElement, string name, T defaultValue = default(T))
        {
            XAttribute attribute = parentElement.Attribute(name);
            if (attribute == null)
            {
                return defaultValue;
            }
            Type targetType = typeof(T);
            if (targetType == typeof(string))
            {
                return (dynamic)attribute.Value;
            }
            if (targetType == typeof(int))
            {
                return (dynamic)int.Parse(attribute.Value);
            }
            if (targetType == typeof(Guid))
            {
                return (dynamic)Guid.Parse(attribute.Value);
            }
            if (targetType.IsEnum)
            {
                return (dynamic)Enum.Parse(targetType, attribute.Value);
            }
            return defaultValue;
        }

        public static string ReadElement(this XElement parentElement, string name)
        {
            XElement element = parentElement.Element(name);
            if (element == null)
            {
                return null;
            }
            return element.Value;
        }

        public static void WriteAttribute<T>(this XElement parentElement, string name, T value)
        {
            XAttribute attribute = new XAttribute(name, value);
            parentElement.Add(attribute);
        }

        public static void WriteElement<T>(this XElement parentElement, string name, T value)
        {
            XElement element = new XElement(name, value);
            parentElement.Add(element);
        }
    }
}
