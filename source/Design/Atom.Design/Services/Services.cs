using Atom.Design.Hosting;

namespace Atom.Design.Services
{
    public static class Services
    {
        public static IWorkspace Workspace { get; private set; }

        public static IObjectExplorer ObjectExplorer { get; private set; }

        public static IDesignerSerializer Serializer { get; private set; }

        public static IDesignerValidator Validator { get; private set; }

        public static IDesignerCodeGenerator CodeGenerator { get; private set; }

        public static ITypeService TypeService { get; private set; }

        public static void Initialize(IWorkspace workspace, IObjectExplorer objectExplorer, IDesignerSerializer serializer, IDesignerValidator validator, IDesignerCodeGenerator codeGenerator, ITypeService typeService)
        {
            Workspace = workspace;
            ObjectExplorer = objectExplorer;
            Serializer = serializer;
            Validator = validator;
            CodeGenerator = codeGenerator;
            TypeService = typeService;
        }
    }
}
