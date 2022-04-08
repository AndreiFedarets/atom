using Atom.Design.Hosting;
using System;
using System.Collections.Generic;

namespace Atom.Design.Services
{
    public sealed class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services;
        private bool _initialized;

        public ServiceProvider()
        {
            _services = new Dictionary<Type, object>();
        }

        public object GetService(Type serviceType)
        {
            if (!_initialized)
            {
                return null;
            }
            object service;
            _services.TryGetValue(serviceType, out service);
            return service;
        }

        private void RegisterService<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public void Initialize(IWorkspace workspace)
        {
            lock (_services)
            {
                if (_initialized)
                {
                    return;
                }

                //TypeService
                TypeService typeService = new TypeService();
                RegisterService<ITypeService>(typeService);

                //TestFrameworkService
                ITestFrameworkService frameworkService = new TestFrameworkService();
                RegisterService<ITestFrameworkService>(frameworkService);

                //AssemblyManager
                AssemblyManager assemblyManager = new AssemblyManager();
                assemblyManager.AddLoader(new Reflection.Binary.AssemblyLoader());
                assemblyManager.AddLoader(new Reflection.Code.AssemblyLoader(workspace));
                RegisterService<IAssemblyManager>(assemblyManager);

                //CodeGenerators
                DesignerCodeGenerator codeGenerator = new DesignerCodeGenerator();
                codeGenerator.AddGenerator<Action>(new ActionDesignerCodeGenerator(typeService));
                codeGenerator.AddGenerator<Workflow>(new WorkflowDesignerCodeGenerator(typeService, frameworkService));
                codeGenerator.AddGenerator<Table>(new TableDesignerCodeGenerator(typeService));
                RegisterService<IDesignerCodeGenerator>(codeGenerator);

                //Validators
                DesignerValidator validator = new DesignerValidator();
                validator.AddValidator<Action>(new ActionDesignerValidator());
                validator.AddValidator<Workflow>(new WorkflowDesignerValidator());
                validator.AddValidator<Table>(new DataTableDesignerValidator());
                RegisterService<IDesignerValidator>(validator);

                //Serializers
                DesignerSerializer serializer = new DesignerSerializer();
                serializer.AddSerializer<Action>(new ActionDesignerSerializer(assemblyManager, typeService));
                serializer.AddSerializer<Workflow>(new WorkflowDesignerSerializer(assemblyManager, typeService));
                serializer.AddSerializer<Table>(new TableDesignerSerializer(typeService));
                RegisterService<IDesignerSerializer>(serializer);

                //ObjectExplorer
                ObjectExplorer objectExplorer = new ObjectExplorer(assemblyManager);
                RegisterService<IObjectExplorer>(objectExplorer);

                //WorkflowDebugger
                IWorkflowDebugger workflowDebugger = new InternalWorkflowDebugger(serializer, workspace);
                RegisterService<IWorkflowDebugger>(workflowDebugger);

                //Services
                Services.Initialize(workspace, objectExplorer, serializer, validator, codeGenerator, typeService);

                _initialized = true;
            }
        }
    }
}
