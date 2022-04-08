namespace Atom.Design
{
    public static class Constants
    {
        internal static class Serialization
        {
            internal static class Reflection
            {
                public const string Name = "Name";
                public const string Type = "Type";
                public const string DeclaringType = "DeclaringType";
                public const string PropertyType = "PropertyType";
                public const string Property = "Property";
                public const string Method = "Method";
                public const string Namespace = "Namespace";
                public const string Assembly = "Assembly";
                public const string Parameters = "Parameters";
                public const string Parameter = "Parameter";
                public const string ParameterDirection = "ParameterDirection";
            }

            internal static class Method
            {
                public const string Title = "Title";
                public const string TitleText = "TitleText";
                public const string Parameters = "Parameters";
                public const string Parameter = "Parameter";
                public const string Direction = "Direction";
                public const string Instructions = "Instructions";
                public const string Invoke = "Invoke";
                public const string Argument = "Argument";
                public const string ValueSource = "ValueSource";
                public const string Name = "Name";
                public const string ValueName = "Name";
                public const string Variable = "Variable";
                //public const string OutputArgument = "OutputArgument";
                //public const string InputParameter = "InputParameter";
                public const string ReferenceParameter = "ReferenceParameter";
                public const string Property = "Property";
                public const string Constant = "Constant";
            }

            internal static class Action
            {
                public const string Root = "Action";
            }

            internal static class Workflow
            {
                public const string Root = "Workflow";
            }

            internal static class Title
            {
                public const string Root = "Title";
                public const string Block = "Block";
                public const string BlockType = "BlockType";
                public const string Content = "Content";
            }

            internal static class Table
            {
                public const string Root = "Table";
                public const string Title = "Title";
                public const string ValueName = "ValueName";
                public const string TableValue = "TableValue";
                //TODO: split and move serialization to Variable
                public const string Value = "Value";
                public const string DesignerToken = "DesignerToken";
            }

            internal static class Package
            {
                public const string Id = "Id";
                public const string Version = "Version";
                public const string Name = "Name";
                public const string Description = "Description";
                public const string Designer = "Designer";
                public const string Dependencies = "Dependencies";
            }
        }

        public const string ActionDesignerDocumentExtension = ".action";
        public const string WorkflowDesignerDocumentExtension = ".workflow";
        public const string TableDesignerDocumentExtension = ".datatable";
        public const string ConditionDesignerDocumentExtension = ".condition";
    }
}