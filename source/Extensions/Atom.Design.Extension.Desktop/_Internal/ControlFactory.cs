using Atom.Design.Reflection.Metadata;
using Atom.Runtime.Extension.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace Atom.Design.Extension.Desktop
{
    internal static class ControlFactory
    {
        public static string GenerateSafeName(Element element)
        {
            ElementPropertyCollection properties = element.Properties;
            StringBuilder safeName = new StringBuilder();
            string name = properties.Name ?? string.Empty;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsLetterOrDigit(c))
                {
                    safeName.Append(c);
                }
            }
            name = safeName.Length == 0 ? GenerateElementName() : safeName.ToString();
            string typeName = properties.ControlType.ProgrammaticName.Replace("ControlType.", string.Empty);
            return string.Format("{0}{1}", safeName, typeName);
        }

        private static string GenerateElementName()
        {
            const int length = 10;
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            Random random = new Random(DateTime.Now.Millisecond);
            string name = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return name;
        }

        public static TableValue CreateValue(string name, Element element)
        {
            ControlType controlType = element.Properties.ControlType;
            List<ControlProperty> properties = new List<ControlProperty>();

            ElementPropertyCollection elementProperties = element.Properties;
            ElementProperty elementProperty;
            elementProperty = elementProperties[AutomationElement.AutomationIdProperty];
            properties.Add(new ControlProperty(elementProperty.Id, elementProperty.Value));

            elementProperty = elementProperties[AutomationElement.ClassNameProperty];
            properties.Add(new ControlProperty(elementProperty.Id, elementProperty.Value));

            elementProperty = elementProperties[AutomationElement.NameProperty];
            properties.Add(new ControlProperty(elementProperty.Id, elementProperty.Value));

            Control control = null;
            TypeReference type = null;
            if (controlType == ControlType.Button)
            {
                type = ButtonTypeAdapter.InterfaceTypeReference;
                control = new Button(properties.ToArray());
            }
            else if (controlType == ControlType.Text)
            {
                type = TextBoxTypeAdapter.InterfaceTypeReference;
                control = new TextBox(properties.ToArray());
            }
            else if (controlType == ControlType.Window)
            {
                type = WindowTypeAdapter.InterfaceTypeReference;
                Application application = ProcessManager.GetApplicationInformation(element);
                control = new Window(application, properties.ToArray());
            }
            else
            {
                throw new NotSupportedException();
            }
            return new TableValue(name, type, control);
        }
    }
}
