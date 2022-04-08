using System.Collections.Generic;
using System.Windows.Automation;

namespace Atom.Design.Extension.Desktop
{
    public sealed class ElementLocator
    {
        public List<Element> GetWindows()
        {
            AutomationElement root = AutomationElement.RootElement;
            AutomationElementCollection collection = root.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            List<Element> windows = new List<Element>();
            foreach (AutomationElement automationElement in collection)
            {
                Element window = new Element(automationElement, null);
                windows.Add(window);
            }
            return windows;
        }

        public Element GetRootElement()
        {
            return new Element(AutomationElement.RootElement);
        }

        public List<Element> GetElements()
        {
            AutomationElement root = AutomationElement.RootElement;
            AutomationElementCollection collection = root.FindAll(TreeScope.Children, Condition.TrueCondition);
            List<Element> elements = new List<Element>();
            foreach (AutomationElement automationElement in collection)
            {
                Element element = new Element(automationElement, null);
                elements.Add(element);
            }
            return elements;
        }

        public static Element GetParent(Element element)
        {
            Element parentElement = null;
            try
            {
                AutomationElement parentAutomationElement = TreeWalker.RawViewWalker.GetParent(element.AutomationElement);
                if (parentAutomationElement != null && !Automation.Compare(parentAutomationElement, AutomationElement.RootElement))
                {
                    parentElement = new Element(parentAutomationElement);
                }
            }
            catch (System.Exception)
            {

            }
            return parentElement;
        }

        public static IList<Element> GetChildren(Element parent)
        {
            List<Element> collection = new List<Element>();
            try
            {
                AutomationElementCollection automationElements = parent.AutomationElement.FindAll(TreeScope.Children, Condition.TrueCondition);
                foreach (AutomationElement automationElement in automationElements)
                {
                    Element element = new Element(automationElement, parent);
                    collection.Add(element);
                }
            }
            catch (System.Exception)
            {
            }
            return collection;
        }

        public static Element FromPoint(System.Windows.Point point)
        {
            Element element = null;
            AutomationElement automationElement = null;
            try
            {
                automationElement = AutomationElement.FromPoint(point);
                if (automationElement != null)
                {
                    element = new Element(automationElement);
                }
            }
            catch (System.Exception)
            {
                
            }
            return element;
        }

        private static IEnumerable<AutomationElement> GetChildren(AutomationElement parent)
        {
            //List<AutomationElement> children = new List<AutomationElement>();
            //AutomationElementCollection collection = parent.FindAll(TreeScope.Children, Condition.TrueCondition);
            //foreach (AutomationElement child in collection)
            //{
            //    children.Add(child);
            //}
            //return children;
            TreeWalker treeWalker = TreeWalker.RawViewWalker;
            List<AutomationElement> children = new List<AutomationElement>();
            AutomationElement child = treeWalker.GetFirstChild(parent);
            while (child != null)
            {
                children.Add(child);
                child = treeWalker.GetNextSibling(child);
            }
            return children;
        }
    }
}
