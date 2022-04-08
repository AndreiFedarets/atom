using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Resources;

namespace Atom.Client.VisualStudio
{
    public interface IVisualStudioPackage : IVsPackage, Microsoft.VisualStudio.OLE.Interop.IServiceProvider, IOleCommandTarget, IVsPersistSolutionOpts, IServiceContainer, System.IServiceProvider, IVsUserSettings, IVsUserSettingsMigration, IVsUserSettingsQuery, IVsToolWindowFactory, IVsToolboxItemProvider
    {
        bool Zombied { get; }
        RegistryKey UserRegistryRoot { get; }
        string UserLocalDataPath { get; }
        string UserDataPath { get; }
        RegistryKey ApplicationRegistryRoot { get; }

        event EventHandler ToolboxInitialized;
        event EventHandler ToolboxUpgraded;

        object CreateInstance(ref Guid clsid, ref Guid iid, Type type);
        ToolWindowPane FindToolWindow(Type toolWindowType, int id, bool create);
        WindowPane FindWindowPane(Type toolWindowType, int id, bool create);
        DialogPage GetDialogPage(Type dialogPageType);
        IVsOutputWindowPane GetOutputPane(Guid page, string caption);
        int GetProviderLocale();
        bool IsLocalService(Type serviceType);
        void ParseToolboxResource(TextReader resourceData, ResourceManager localizedCategories);
        void ParseToolboxResource(TextReader resourceData, Guid packageGuid);
        void ShowOptionPage(Type optionsPageType);
        void AddOptionKey(string name);
        WindowPane CreateToolWindow(Type toolWindowType, int id);
        WindowPane CreateToolWindow(Type toolWindowType, int id, uint flags);
        int CreateToolWindow(ref Guid toolWindowType, int id);
        void Dispose(bool disposing);
        object GetAutomationObject(string name);
        object GetToolboxItemData(string itemId, System.Windows.Forms.DataFormats.Format format);
        WindowPane InstantiateToolWindow(Type toolWindowType);
        void OnLoadOptions(string key, Stream stream);
        void OnSaveOptions(string key, Stream stream);
        int QueryClose(out bool canClose);
        void RegisterEditorFactory(IVsEditorFactory factory);
        void RegisterProjectFactory(IVsProjectFactory factory);
    }
}
