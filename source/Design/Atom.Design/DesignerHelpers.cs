using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Atom.Design
{
    public static class DesignerHelpers
    {
        public static IEnumerable<IValueSource> FindLocalValueSources(TypeReference type, IValueScope relativeTo, IValueScopeCollection collection)
        {
            List<IValueScope> scopes = new List<IValueScope>();
            FindLocalValueScopes(collection, relativeTo, scopes);
            IEnumerable<IValueSource> sources = scopes.SelectMany(x => x.Sources);
            if (type != null)
            {
                sources = sources.Where(x => type.IsAssignableFrom(x.ValueType));
            }
            return sources;
        }

        private static bool FindLocalValueScopes(IValueScopeCollection collection, IValueScope relativeTo, List<IValueScope> resultScopes)
        {
            foreach (IValueScope scope in collection.Scopes)
            {
                if (ReferenceEquals(scope, relativeTo))
                {
                    return false;
                }
                if (scope is IValueScopeCollection)
                {
                    if (!FindLocalValueScopes((IValueScopeCollection)scope, relativeTo, resultScopes))
                    {
                        return false;
                    }
                }
                else
                {
                    resultScopes.Add(scope);
                }
            }
            return true;
        }

        public static void BindLocalValueConsumers(IValueScope targetScope, IValueScopeCollection scopes)
        {
            foreach (IValueConsumer consumer in targetScope.Consumers)
            {
                IValueSource source = FindLocalValueSources(consumer.ValueType, targetScope, scopes).LastOrDefault();
                //For now we auto-bind to last available value source
                if (source != null)
                {
                    consumer.Value = source.CreateValue();
                }
            }
        }

        public static void UnbindLocalValueSources(IValueScope sourceScope, IValueScopeCollection scopes)
        {
            foreach (IValueSource source in sourceScope.Sources)
            {

            }
        }

        public static bool CanRenameTo(IValueSource source, IValueScopeCollection scopes, string desiredName)
        {
            if (string.IsNullOrEmpty(desiredName))
            {
                return false;
            }
            return FindLocalValueSources(null, null, scopes).Any(x => string.Equals(x.ValueName, desiredName) && x.ValueType != source.ValueType);
        }


        public static void RebindConsumers(BaseValue oldValue, BaseValue newValue, IValueScopeCollection scopes)
        {
            
        }

        public static T GetParent<T>(object obj) where T : class
        {
            DependencyObject dependencyObject = obj as DependencyObject;
            while (dependencyObject != null)
            {
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                if (dependencyObject is T)
                {
                    return (T)(object)dependencyObject;
                }
            }
            return default(T);
        }

        //public static InputArgument FindInputArgument(Method method, string argumentName)
        //{
        //    foreach (InvokeInstruction instance in method.Instructions.OfType<InvokeInstruction>())
        //    {
        //        foreach (InputArgument inputArgument in instance.Arguments.OfType<InputArgument>())
        //        {
        //            if (string.Equals(inputArgument.ValueName, argumentName, StringComparison.Ordinal))
        //            {
        //                return inputArgument;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public static OutputArgument FindOutputArgument(Method method, string argumentName)
        //{
        //    foreach (InvokeInstruction instance in method.Instructions.OfType<InvokeInstruction>())
        //    {
        //        foreach (OutputArgument outputArgument in instance.Arguments.OfType<OutputArgument>())
        //        {
        //            if (string.Equals(outputArgument.ValueName, argumentName, StringComparison.Ordinal))
        //            {
        //                return outputArgument;
        //            }
        //        }
        //    }
        //    return null;
        //}


        //public static InvokeInstruction InsertAction(Method method, IAction action)
        //{
        //    InvokeInstruction invoke = new InvokeInstruction(action);
        //    //GenerateOutputArgumentsSafeName(invoke, method);
        //    //AutoBindInputArguments(invoke, method);
        //    method.Instructions.Add(invoke);
        //    return invoke;
        //}

        //public static ReferenceArgument FindReferenceArgument(Method method, string argumentName)
        //{
        //    foreach (Invoke instance in method.OfType<Invoke>())
        //    {
        //        foreach (ReferenceArgument referenceArgument in instance.Arguments.OfType<ReferenceArgument>())
        //        {
        //            if (string.Equals(referenceArgument.ValueName, argumentName, StringComparison.Ordinal))
        //            {
        //                return referenceArgument;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public static void GenerateOutputArgumentsSafeName(InvokeInstruction instance, Method method)
        //{
        //    foreach (OutputArgument outputArgument in instance.Arguments.OfType<OutputArgument>())
        //    {
        //        outputArgument.ValueName = GenerateSafeVariableName(outputArgument.ValueName, method);
        //    }
        //}

        //public static InputParameter FindInputParameter(Action action, string parameterName)
        //{
        //    foreach (InputParameter inputParameter in action.Parameters.OfType<InputParameter>())
        //    {
        //        if (string.Equals(inputParameter.ValueName, parameterName, StringComparison.Ordinal))
        //        {
        //            return inputParameter;
        //        }
        //    }
        //    return null;
        //}

        //public static OutputArgument FindOutputValueSource(Method method, string argumentName)
        //{
        //    foreach (InvokeInstruction invoke in method.Instructions.OfType<InvokeInstruction>())
        //    {
        //        foreach (OutputArgument outputArgument in invoke.Arguments.OfType<OutputArgument>())
        //        {
        //            if (string.Equals(outputArgument.ValueName, argumentName, StringComparison.Ordinal))
        //            {
        //                return outputArgument;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public static string GenerateSafeVariableName(string variableDefaultName, Method method)
        //{
        //    string safeName = variableDefaultName;
        //    int index = 0;
        //    while (FindOutputValueSource(method, safeName) != null)
        //    {
        //        index++;
        //        safeName = variableDefaultName + index;
        //    }
        //    return safeName;
        //}

        //public static Parameter CreateParameter(Argument sourceArgument, bool bind)
        //{
        //    Parameter parameter = null;
        //    if (sourceArgument is InputArgument)
        //    {
        //        InputArgument inputArgument = (InputArgument)sourceArgument;
        //        string safeName = GenerateSafeVariableName(inputArgument.Parameter.Name, sourceArgument.Parent.Parent);
        //        InputParameter inputParameter = new InputParameter(safeName, inputArgument);
        //        if (bind)
        //        {
        //            inputArgument.Source = inputParameter;
        //        }
        //        parameter = inputParameter;
        //    }
        //    else if (sourceArgument is OutputArgument)
        //    {
        //        OutputArgument outputArgument = (OutputArgument)sourceArgument;
        //        OutputParameter outputParameter = new OutputParameter(outputArgument);
        //        parameter = outputParameter;
        //    }
        //    //else if (sourceArgument is ReferenceArgument)
        //    //{
        //    //    ReferenceArgument referenceArgument = (ReferenceArgument)sourceArgument;
        //    //    ReferenceParameter referenceParameter = new ReferenceParameter(referenceArgument);
        //    //    if (bind)
        //    //    {
        //    //        referenceArgument.Source = referenceParameter;
        //    //    }
        //    //    parameter = referenceParameter;
        //    //}
        //    return parameter;
        //}

        //public static T GetRoot<T>(DependencyObject dependencyObject) where T : class
        //{
        //    T parentDependencyObject;
        //    while ((parentDependencyObject = GetParent<T>(dependencyObject)) != null)
        //    {
        //        dependencyObject = (DependencyObject)(object)parentDependencyObject;
        //    }
        //    return dependencyObject as T;
        //}
    }
}
