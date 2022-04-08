using Atom.Client.VisualStudio.Editors;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Atom.Client.VisualStudio
{
    //Package
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(ClientConstants.PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    //Windows
    //[ProvideToolWindow(typeof(ActionExplorerWindow))]
    //Code Generators----------------------------------------------------------------------------------------------------------------------------------------------
    //Action
    [ProvideCodeGenerator(typeof(ActionDesignerCSharpGenerator), nameof(ActionDesignerCSharpGenerator), "Action CSharp Code Generator", true)]
    [ProvideCodeGenerator(typeof(ActionDesignerVisualBasicGenerator), nameof(ActionDesignerVisualBasicGenerator), "Action VisualBasic Code Generator", true)]
    //Condition
    [ProvideCodeGenerator(typeof(ConditionDesignerCSharpGenerator), nameof(ConditionDesignerCSharpGenerator), "Condition CSharp Code Generator", true)]
    [ProvideCodeGenerator(typeof(ConditionDesignerVisualBasicGenerator), nameof(ConditionDesignerVisualBasicGenerator), "Condition VisualBasic Code Generator", true)]
    //Workflow
    [ProvideCodeGenerator(typeof(WorkflowDesignerCSharpGenerator), nameof(WorkflowDesignerCSharpGenerator), "Workflow CSharp Code Generator", true)]
    [ProvideCodeGenerator(typeof(WorkflowDesignerVisualBasicGenerator), nameof(WorkflowDesignerVisualBasicGenerator), "Workflow VisualBasic Code Generator", true)]
    //Table
    [ProvideCodeGenerator(typeof(TableDesignerCSharpGenerator), nameof(TableDesignerCSharpGenerator), "Data Table CSharp Code Generator", true)]
    [ProvideCodeGenerator(typeof(TableDesignerVisualBasicGenerator), nameof(TableDesignerVisualBasicGenerator), "Data Table VisualBasic Code Generator", true)]
    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Action Design Pane
    [ProvideEditorLogicalView(typeof(ActionDesignerFactory), VisualStudioConstants.LogicalView.Text)]
    [ProvideEditorLogicalView(typeof(ActionDesignerFactory), VisualStudioConstants.LogicalView.Designer)]
    [ProvideEditorFactory(typeof(ActionDesignerFactory), 101)]
    [ProvideEditorExtension(typeof(ActionDesignerFactory), Design.Constants.ActionDesignerDocumentExtension, 32, NameResourceID = 101)]
    //Condition Design Pane
    [ProvideEditorLogicalView(typeof(ConditionDesignerFactory), VisualStudioConstants.LogicalView.Text)]
    [ProvideEditorLogicalView(typeof(ConditionDesignerFactory), VisualStudioConstants.LogicalView.Designer)]
    [ProvideEditorFactory(typeof(ConditionDesignerFactory), 101)]
    [ProvideEditorExtension(typeof(ConditionDesignerFactory), Design.Constants.ConditionDesignerDocumentExtension, 32, NameResourceID = 101)]
    //Workflow Design Pane
    [ProvideEditorLogicalView(typeof(WorkflowDesignerFactory), VisualStudioConstants.LogicalView.Text)]
    [ProvideEditorLogicalView(typeof(WorkflowDesignerFactory), VisualStudioConstants.LogicalView.Designer)]
    [ProvideEditorFactory(typeof(WorkflowDesignerFactory), 103)]
    [ProvideEditorExtension(typeof(WorkflowDesignerFactory), Design.Constants.WorkflowDesignerDocumentExtension, 32, NameResourceID = 103)]
    //DataTable Design Pane
    [ProvideEditorLogicalView(typeof(TableDesignerFactory), VisualStudioConstants.LogicalView.Text)]
    [ProvideEditorLogicalView(typeof(TableDesignerFactory), VisualStudioConstants.LogicalView.Designer)]
    [ProvideEditorFactory(typeof(TableDesignerFactory), 102)]
    [ProvideEditorExtension(typeof(TableDesignerFactory), Design.Constants.TableDesignerDocumentExtension, 32, NameResourceID = 102)]
    public sealed class VSPackage : AsyncPackage, IVisualStudioPackage
    {
        private readonly BootstrapperInitializer _bootstrapperInitializer;
        
        public VSPackage()
        {
            _bootstrapperInitializer = new BootstrapperInitializer();
        }

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();
            _bootstrapperInitializer.Initialize(this);
        }

        void IVisualStudioPackage.AddOptionKey(string name)
        {
            AddOptionKey(name);
        }

        WindowPane IVisualStudioPackage.CreateToolWindow(Type toolWindowType, int id)
        {
            return CreateToolWindow(toolWindowType, id);
        }

        WindowPane IVisualStudioPackage.CreateToolWindow(Type toolWindowType, int id, uint flags)
        {
            return CreateToolWindow(toolWindowType, id, flags);
        }

        int IVisualStudioPackage.CreateToolWindow(ref Guid toolWindowType, int id)
        {
            return CreateToolWindow(ref toolWindowType, id);
        }

        void IVisualStudioPackage.Dispose(bool disposing)
        {
            Dispose(disposing);
            _bootstrapperInitializer.Dispose();
        }

        object IVisualStudioPackage.GetAutomationObject(string name)
        {
            return GetAutomationObject(name);
        }

        object IVisualStudioPackage.GetToolboxItemData(string itemId, DataFormats.Format format)
        {
            return GetToolboxItemData(itemId, format);
        }

        WindowPane IVisualStudioPackage.InstantiateToolWindow(Type toolWindowType)
        {
            return InstantiateToolWindow(toolWindowType);
        }

        void IVisualStudioPackage.OnLoadOptions(string key, Stream stream)
        {
            OnLoadOptions(key, stream);
        }

        void IVisualStudioPackage.OnSaveOptions(string key, Stream stream)
        {
            OnSaveOptions(key, stream);
        }

        int IVisualStudioPackage.QueryClose(out bool canClose)
        {
            return QueryClose(out canClose);
        }

        void IVisualStudioPackage.RegisterEditorFactory(IVsEditorFactory factory)
        {
            RegisterEditorFactory(factory);
        }

        void IVisualStudioPackage.RegisterProjectFactory(IVsProjectFactory factory)
        {
            RegisterProjectFactory(factory);
        }
    }
}
