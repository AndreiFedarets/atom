using Atom.Design.Hosting;
using Atom.Design.Services;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;

namespace Atom.Client.VisualStudio
{
    internal sealed class MenuCommandManager : IMenuCommandManager
    {
        private readonly IMenuCommandService _commandService;
        private readonly List<MenuCommand> _commands;
        private readonly EnvDTE.DTE _dte;
        private readonly EnvDTE.CommandEvents _commandEvents;
        private readonly IWorkspace _workspace;
        private readonly IWorkflowDebugger _debugger;

        public MenuCommandManager(IServiceProvider serviceProvider, IWorkspace workspace, IWorkflowDebugger debugger)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _commands = new List<MenuCommand>();
            _commandService = serviceProvider.GetService<IMenuCommandService>();
            _dte = serviceProvider.GetService<EnvDTE.DTE>();
            _commandEvents = ((EnvDTE80.Events2)_dte.Events).CommandEvents;
            _workspace = workspace;
            _debugger = debugger;
        }

        public void Initialize()
        {
            {
                CommandID id = new CommandID(ClientConstants.Menus.guidAtomMainMenu, ClientConstants.Menus.ShowActionExplorerCommandId);
                MenuCommand command = new MenuCommand(new EventHandler(ShowActionExplorerCallback), id);
                _commandService.AddCommand(command);
                _commands.Add(command);
            }
            {
                CommandID id = new CommandID(ClientConstants.Menus.guidAtomMainMenu, ClientConstants.Menus.ManageProjectModulesCommandId);
                MenuCommand command = new MenuCommand(new EventHandler(ManageProjectModulesCallback), id);
                _commandService.AddCommand(command);
                _commands.Add(command);
            }
            _commandEvents.BeforeExecute += OnBeforeCommandExecute;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.MenuCommandsPackage.OutputCommandString(System.String)")]
        private void ShowActionExplorerCallback(object caller, EventArgs args)
        {
            
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "Microsoft.Samples.VisualStudio.MenuCommands.MenuCommandsPackage.OutputCommandString(System.String)")]
        private void ManageProjectModulesCallback(object caller, EventArgs args)
        {
        }

        private void OnBeforeCommandExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            EnvDTE80.Commands2 commands = null;
            EnvDTE.Command command = null;
            try
            {
                if (string.IsNullOrEmpty(guid))
                {
                    return;
                }
                commands = (EnvDTE80.Commands2)_dte.Commands;
                command = commands.Item(guid, id);
                if (command == null)
                {
                    return;
                }
                if (string.Equals(command.Name, "Debug.Start", StringComparison.OrdinalIgnoreCase))
                {
                    if (_dte.Debugger.CurrentMode == EnvDTE.dbgDebugMode.dbgDesignMode)
                    {
                        cancelDefault = StartDebugging();
                    }
                }
                else if (string.Equals(command.Name, "Debug.StartWithoutDebugging", StringComparison.OrdinalIgnoreCase))
                {
                    cancelDefault = StartExecution();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Releaser.Release(ref command);
                Releaser.Release(ref commands);
            }
        }

        private bool StartDebugging()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IDocument activeDocument = GetActiveDesignerCodeDocument();
            activeDocument = activeDocument.Designer ?? activeDocument;
            return _debugger.StartDebugging(activeDocument);
        }

        private bool StartExecution()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IDocument activeDocument = GetActiveDesignerCodeDocument();
            activeDocument = activeDocument.Designer ?? activeDocument;
            return _debugger.StartExecution(activeDocument);
        }

        private IDocument GetActiveDesignerCodeDocument()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            EnvDTE.Document nativeDocument = null;
            EnvDTE.ProjectItem nativeProjectItem = null;
            EnvDTE.Properties nativeProperties = null;
            EnvDTE.Property nativeProperty = null;
            IDocument document = null;
            try
            {
                nativeDocument = _dte.ActiveDocument;
                if (nativeDocument == null)
                {
                    return null;
                }
                nativeProjectItem = nativeDocument.ProjectItem;
                //nativeProjectItem = FindCodedProjectItem(nativeProjectItem);
                if (nativeProjectItem == null)
                {
                    return null;
                }
                nativeProperties = nativeProjectItem.Properties;
                if (nativeProperties == null)
                {
                    return null;
                }
                nativeProperty = nativeProperties.Item("FullPath");
                if (nativeProperty == null)
                {
                    return null;
                }
                string fileFullName = (string)nativeProperty.Value;
                document = _workspace.Solution.FindDocument(fileFullName);
            }
            finally
            {
                Releaser.Release(ref nativeProperty);
                Releaser.Release(ref nativeProperties);
                Releaser.Release(ref nativeProjectItem);
                Releaser.Release(ref nativeDocument);
            }
            return document;
        }

    }
}
