using Atom.Design;
using Atom.Design.Hosting;
using Atom.Design.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Windows.Forms.Integration;

namespace Atom.Client.VisualStudio.Editors
{
    public sealed class ObjectDesignerPane : WindowPane, IOleCommandTarget, IVsPersistDocData, IPersistFileFormat // IVsToolboxUser
    {
        private const uint FileFormat = 0;
        private readonly string _fileExtension;
        private readonly Guid _editorFactoryGuid;
        private readonly IWorkspace _workspace;
        private readonly IDesignerSerializer _designerSerializer;
        private readonly IViewManager _viewManager;
        private readonly string _contentName;
        private IObjectDesigner _objectDesigner;
        private ElementHost _documentControl;
        private string _fileName;
        private volatile bool _documentLoading;
        private volatile bool _documentSaving;
        private volatile bool _hasChanges;

        public ObjectDesignerPane(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager, Guid editorFactoryGuid, string fileExtension, string contentName)
        {
            _workspace = workspace;
            _designerSerializer = designerSerializer;
            _viewManager = viewManager;
            _editorFactoryGuid = editorFactoryGuid;
            _fileExtension = fileExtension;
            _contentName = contentName;
            _documentLoading = false;
            _documentSaving = false;
            _hasChanges = false;
            _documentControl = new ElementHost();
        }

        public bool IsDocumentLoading
        {
            get { return _documentLoading; }
        }
        
        public sealed override System.Windows.Forms.IWin32Window Window
        {
            get { return _documentControl; }
        }

        private IObjectDesigner LoadDesigner(string fileFullName)
        {
            IObjectDesigner objectDesigner = null;
            ISolution solution = _workspace.Solution;
            if (solution != null)
            {
                IDocument document = solution.FindDocument(fileFullName);
                if (document == null)
                {
                    solution.Reload();
                    document = solution.FindDocument(fileFullName);
                }
                if (document != null)
                {
                    objectDesigner = _designerSerializer.Read(document);
                }
            }
            return objectDesigner;
        }

        private bool SaveDesigner(string fileFullName)
        {
            //TODO: handle fileFullName
            if (_objectDesigner != null)
            {
                return _designerSerializer.Write(_objectDesigner);
            }
            return false;
        }

        protected sealed override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_documentControl != null)
                    {
                        _documentControl.Dispose();
                        _documentControl = null;
                    }
                    GC.SuppressFinalize(this);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        
        #region IVsPersistDocData

        public int GetGuidEditorType(out Guid pClassID)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return GetClassID(out pClassID);
        }

        public int IsDocDataDirty(out int pfDirty)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return IsDirty(out pfDirty);
        }

        public int SetUntitledDocPath(string pszDocDataPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return InitNew(FileFormat);
        }

        public int LoadDocData(string pszMkDocument)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return Load(pszMkDocument, 0, 0);
        }

        public int SaveDocData(VSSAVEFLAGS dwSave, out string pbstrMkDocumentNew, out int pfSaveCanceled)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            pbstrMkDocumentNew = null;
            pfSaveCanceled = 0;
            int result = VSConstants.S_OK;

            switch (dwSave)
            {
                case VSSAVEFLAGS.VSSAVE_Save:
                case VSSAVEFLAGS.VSSAVE_SilentSave:
                    {
                        IVsQueryEditQuerySave2 queryEditQuerySave = (IVsQueryEditQuerySave2)GetService(typeof(SVsQueryEditQuerySave));
                        uint querySaveResult = 0;
                        result = queryEditQuerySave.QuerySaveFile( _fileName, 0, null, out querySaveResult);
                        if (ErrorHandler.Failed(result))
                        {
                            return result;
                        }
                        switch ((tagVSQuerySaveResult)querySaveResult)
                        {
                            case tagVSQuerySaveResult.QSR_NoSave_Cancel:
                                {
                                    pfSaveCanceled = ~0;
                                }
                                break;
                            case tagVSQuerySaveResult.QSR_SaveOK:
                                {
                                    IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                                    result = uiShell.SaveDocDataToFile(dwSave, this, _fileName, out pbstrMkDocumentNew, out pfSaveCanceled);
                                    if (ErrorHandler.Failed(result))
                                    {
                                        return result;
                                    }
                                }
                                break;
                            case tagVSQuerySaveResult.QSR_ForceSaveAs:
                                {
                                    IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                                    result = uiShell.SaveDocDataToFile(VSSAVEFLAGS.VSSAVE_SaveAs, this, _fileName, out pbstrMkDocumentNew, out pfSaveCanceled);
                                    if (ErrorHandler.Failed(result))
                                    {
                                        return result;
                                    }
                                }
                                break;
                            case tagVSQuerySaveResult.QSR_NoSave_Continue:
                                break;
                            default:
                                return VSConstants.S_FALSE;
                        }
                    }
                    break;
                case VSSAVEFLAGS.VSSAVE_SaveAs:
                case VSSAVEFLAGS.VSSAVE_SaveCopyAs:
                    {
                        //TODO: I don't understand why its needed
                        if (!string.Equals(Path.GetExtension(_fileName), _fileExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            _fileName += _fileExtension;
                        }
                        // Call the shell to do the save for us
                        IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                        result = uiShell.SaveDocDataToFile(dwSave, this, _fileName, out pbstrMkDocumentNew, out pfSaveCanceled);
                        if (ErrorHandler.Failed(result))
                        {
                            return result;
                        }
                    }
                    break;
            }
            return result;
        }

        public int Close()
        {
            Dispose();
            return VSConstants.S_OK;
        }

        public int OnRegisterDocData(uint docCookie, IVsHierarchy pHierNew, uint itemidNew)
        {
            return VSConstants.S_OK;
        }

        public int RenameDocData(uint grfAttribs, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int IsDocDataReloadable(out int pfReloadable)
        {
            // Allow file to be reloaded
            pfReloadable = 1;
            return VSConstants.S_OK;
        }

        public int ReloadDocData(uint grfFlags)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return Load(null, grfFlags, 0);
        }

        #endregion

        #region IPersistFileFormat

        public int GetClassID(out Guid pClassID)
        {
            pClassID = _editorFactoryGuid;
            return VSConstants.S_OK;
        }

        public int IsDirty(out int pfIsDirty)
        {
            pfIsDirty = _hasChanges ? 1 : 0;
            return VSConstants.S_OK;
        }

        public int InitNew(uint nFormatIndex)
        {
            if (nFormatIndex != FileFormat)
            {
                throw new ArgumentException("Unknown format");
            }
            _hasChanges = false;
            return VSConstants.S_OK;
        }

        public int Load(string pszFilename, uint grfMode, int fReadOnly)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _documentLoading = true;
            try
            {
                //2 cases:
                //1. loading (pszFilename != null)
                //2. reloading (pszFilename == null && _fileName != null)
                if (string.IsNullOrEmpty(pszFilename) && string.IsNullOrEmpty(_fileName))
                {
                    throw new ArgumentNullException("pszFilename");
                }
                if (!string.IsNullOrEmpty(pszFilename))
                {
                    _fileName = pszFilename;
                }
                IVsUIShell vsUiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                if (vsUiShell != null)
                {
                    vsUiShell.SetWaitCursor();
                }
                _objectDesigner = LoadDesigner(_fileName);
                if (_objectDesigner == null)
                {
                    return VSConstants.E_FAIL;
                }
                _documentControl.Child = _viewManager.CreateDesignerView(_objectDesigner);
                INotifyDocumentChanged notifyDocumentChanged = _documentControl.Child as INotifyDocumentChanged;
                if (notifyDocumentChanged != null)
                {
                    notifyDocumentChanged.DocumentChanged += OnDocumentChanged;
                }
                int result = _documentControl.Child != null ? VSConstants.S_OK : VSConstants.E_FAIL;
                if (ErrorHandler.Failed(result))
                {
                    return result;
                }
                _hasChanges = false;
                result = NotifyDocChanged();
                if (ErrorHandler.Failed(result))
                {
                    return result;
                }
            }
            finally
            {
                _documentLoading = false;
            }
            return VSConstants.S_OK;
        }

        private void OnDocumentChanged(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _hasChanges = true;
            NotifyDocChanged();
        }

        public int Save(string pszFilename, int fRemember, uint nFormatIndex)
        {
            _documentSaving = true;
            int result = VSConstants.S_OK;
            try
            {
                if (string.IsNullOrEmpty(pszFilename) && string.IsNullOrEmpty(_fileName))
                {
                    throw new ArgumentNullException("pszFilename");
                }
                //Case 1: saving to the same file
                if (string.IsNullOrEmpty(pszFilename) || string.Equals(pszFilename, _fileName, StringComparison.OrdinalIgnoreCase))
                {
                    result = SaveDesigner(_fileName) ? VSConstants.S_OK : VSConstants.E_FAIL;
                }
                //Case 2: save to another file
                else
                {
                    //Case 2.1: save to another file and rememeber
                    if (fRemember != 0)
                    {
                        _fileName = pszFilename;
                        result = SaveDesigner(_fileName) ? VSConstants.S_OK : VSConstants.E_FAIL;
                    }
                    //Case 2.2: save to another file as a copy
                    else
                    {
                        result = SaveDesigner(pszFilename) ? VSConstants.S_OK : VSConstants.E_FAIL;
                    }
                }
            }
            finally
            {
                _documentSaving = false;
                _hasChanges = false;
            }
            return result;
        }

        public int SaveCompleted(string pszFilename)
        {
            if (_documentSaving)
            {
                return VSConstants.S_FALSE;
            }
            return VSConstants.S_OK;
        }

        public int GetCurFile(out string ppszFilename, out uint pnFormatIndex)
        {
            pnFormatIndex = FileFormat;
            ppszFilename = _fileName;
            return VSConstants.S_OK;
        }

        public int GetFormatList(out string ppszFormatList)
        {
            string formatList = string.Format("{2} (*{0}){1}*{0}{1}{1}", _fileExtension, System.Environment.NewLine, _contentName);
            ppszFormatList = formatList;
            return VSConstants.S_OK;
        }

        private int NotifyDocChanged()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (string.IsNullOrEmpty(_fileName))
            {
                return VSConstants.S_FALSE;
            }

            IVsRunningDocumentTable runningDocumentTable = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));

            uint docCookie;
            IVsHierarchy hierarchy;
            uint itemID;
            IntPtr docData;
            int result = runningDocumentTable.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_ReadLock, _fileName, out hierarchy, out itemID, out docData, out docCookie);
            if (ErrorHandler.Failed(result))
            {
                return result;
            }

            result = runningDocumentTable.NotifyDocumentChanged(docCookie, (uint)__VSRDTATTRIB.RDTA_DocDataReloaded);

            // We have to unlock the document even if the previous call failed.
            runningDocumentTable.UnlockDocument((uint)_VSRDTFLAGS.RDT_ReadLock, docCookie);

            return result;
        }

        #endregion

        #region IOleCommandTarget

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return (int)(Microsoft.VisualStudio.OLE.Interop.Constants.OLECMDERR_E_NOTSUPPORTED);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            return (int)(Microsoft.VisualStudio.OLE.Interop.Constants.OLECMDERR_E_NOTSUPPORTED);
        }

        #endregion
    }
}
