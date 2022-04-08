using Atom.Design.Reflection.Metadata;
using Atom.Design.Reflection.Metadata.Binary;
using Atom.Runtime;
using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace Atom.Design.Reflection.Binary
{
    internal sealed class AssemblyInternalLoader : MarshalByRefObject
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public IAssembly LoadAssembly(string assemblyName)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyName);
            ActionCollection actions = LoadActions(assembly);
            ConditionCollection conditions = LoadConditions(assembly);
            TableCollection tables = LoadTables(assembly);
            AssemblyReference assemblyReference = new AssemblyReference(assembly.GetName());
            Assembly actionAssembly = new Assembly(assemblyReference, actions, conditions, tables);
            return actionAssembly;
        }

        private ActionCollection LoadActions(System.Reflection.Assembly assembly)
        {
            Dictionary<MethodReference, IAction> collection = new Dictionary<MethodReference, IAction>();
            foreach (Type type in assembly.GetTypes())
            {
                foreach (System.Reflection.MethodInfo methodInfo in type.GetMethods())
                {
                    IAction action;
                    if (TryLoadAction(methodInfo, out action))
                    {
                        collection.Add(action.Reference, action);
                    }
                }
            }
            return new ActionCollection(collection);
        }

        private bool TryLoadAction(System.Reflection.MethodInfo methodInfo, out IAction action)
        {
            ActionMethodAttribute attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<ActionMethodAttribute>(methodInfo);
            if (attribute == null)
            {
                action = null;
                return false;
            }
            MethodReference methodReference = MetadataProvider.GetReference(methodInfo);
            action = new Action(attribute.Title, methodReference);
            return true;
        }

        private ConditionCollection LoadConditions(System.Reflection.Assembly assembly)
        {
            Dictionary<MethodReference, ICondition> collection = new Dictionary<MethodReference, ICondition>();
            foreach (Type type in assembly.GetTypes())
            {
                foreach (System.Reflection.MethodInfo methodInfo in type.GetMethods())
                {
                    ICondition condition;
                    if (TryLoadCondition(methodInfo, out condition))
                    {
                        collection.Add(condition.Reference, condition);
                    }
                }
            }
            return new ConditionCollection(collection);
        }

        private bool TryLoadCondition(System.Reflection.MethodInfo methodInfo, out ICondition condition)
        {
            ConditionMethodAttribute attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<ConditionMethodAttribute>(methodInfo);
            if (attribute == null || methodInfo.ReturnType != typeof(bool))
            {
                condition = null;
                return false;
            }
            MethodReference methodReference = MetadataProvider.GetReference(methodInfo);
            condition = new Condition(attribute.Title, methodReference);
            return true;
        }

        private TableCollection LoadTables(System.Reflection.Assembly assembly)
        {
            Dictionary<TypeReference, ITable> collection = new Dictionary<TypeReference, ITable>();
            foreach (Type type in assembly.GetTypes())
            {
                ITable table;
                if (TryLoadTable(type, out table))
                {
                    collection.Add(table.Reference, table);
                }
            }
            return new TableCollection(collection);
        }

        private bool TryLoadTable(Type type, out ITable table)
        {
            DataTableAttribute attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<DataTableAttribute>(type);
            if (attribute == null)
            {
                table = null;
                return false;
            }
            string name = type.Name; //attribute.Name;
            TypeReference typeReference = MetadataProvider.GetReference(type);
            Dictionary<string, ITableValue> collection = new Dictionary<string, ITableValue>();
            foreach (System.Reflection.PropertyInfo propertyInfo in type.GetProperties())
            {
                PropertyReference propertyReference = MetadataProvider.GetReference(propertyInfo);
                TableValue tableValue = new TableValue(propertyReference);
                collection.Add(tableValue.Property.Name, tableValue);
            }
            table = new Table(name, typeReference, collection);
            return true;
        }
    }
}
