using Atom.Runtime.Extension.Desktop;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atom.Design.Extension.Desktop
{
    public sealed class WindowTypeAdapter : ControlTypeAdapter<IWindow, Window>
    {
        protected override List<CodeExpression> GenerateControlArguments(Window control)
        {
            List<CodeExpression> arguments = base.GenerateControlArguments(control);
            arguments.Insert(0, CreateApplicationCreateCode(control.Application));
            return arguments;
        }

        private CodeObjectCreateExpression CreateApplicationCreateCode(Application application)
        {
            if (application is StoreApplication)
            {
                StoreApplication storeApplication = (StoreApplication)application;
                return new CodeObjectCreateExpression(typeof(StoreApplication),
                       new CodePrimitiveExpression(storeApplication.AppName));
            }
            else if (application is ExecutableApplication)
            {
                ExecutableApplication executableApplication = (ExecutableApplication)application;
                return new CodeObjectCreateExpression(typeof(ExecutableApplication),
                       new CodePrimitiveExpression(executableApplication.FileFullName));
            }
            else
            {
                throw new System.NotSupportedException();
            }
        }

        protected override List<object> ReadControlArguments(XElement valueElement)
        {
            List<object> arguments = base.ReadControlArguments(valueElement);
            arguments.Insert(0, ReadApplication(valueElement));
            return arguments;
        }

        private Application ReadApplication(XElement parentElement)
        {
            XElement executableApplicationElement = parentElement.Element(Constants.Serialization.ExecutableApplication);
            if (executableApplicationElement != null)
            {
                string fileFullName = executableApplicationElement.ReadAttribute<string>(Constants.Serialization.FileFullName);
                return new ExecutableApplication(fileFullName);
            }
            XElement storeApplicationElement = parentElement.Element(Constants.Serialization.StoreApplication);
            if (storeApplicationElement != null)
            {
                string appName = storeApplicationElement.ReadAttribute<string>(Constants.Serialization.AppName);
                return new StoreApplication(appName);
            }
            return null;
        }
        
        protected override void WriteControlArguments(XElement valueElement, Window control)
        {
            WriteApplication(valueElement, control.Application);
            base.WriteControlArguments(valueElement, control);
        }

        private void WriteApplication(XElement valueElement, Application application)
        {
            if (application == null)
            {
                return;
            }
            XElement applicationElement;
            if (application is StoreApplication)
            {
                StoreApplication storeApplication = (StoreApplication)application;
                applicationElement = new XElement(Constants.Serialization.StoreApplication);
                applicationElement.Add(
                    new XAttribute(Constants.Serialization.AppName, storeApplication.AppName)
                );
            }
            else if (application is ExecutableApplication)
            {
                ExecutableApplication executableApplication = (ExecutableApplication)application;
                applicationElement = new XElement(Constants.Serialization.ExecutableApplication);
                applicationElement.Add(
                    new XAttribute(Constants.Serialization.FileFullName, executableApplication.FileFullName)
                );
            }
            else
            {
                throw new System.NotSupportedException();
            }
            valueElement.Add(applicationElement);
        }
    }
}
