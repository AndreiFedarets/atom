using Atom.Design.Hosting;
using Atom.Design.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio.Editors
{
    public abstract class ObjectDesignerFactory : IVsEditorFactory, IDisposable
    {
        private readonly IWorkspace _workspace;
        private readonly IViewManager _viewManager;
        private readonly IDesignerSerializer _designerSerializer;
        private readonly Guid _editorFactoryGuid;
        private readonly string _fileExtension;
        private readonly string _contentName;

        public ObjectDesignerFactory(IWorkspace workspace, IDesignerSerializer designerSerializer, IViewManager viewManager, Guid editorFactoryGuid, string fileExtension, string contentName)
        {
            _viewManager = viewManager;
            _workspace = workspace;
            _designerSerializer = designerSerializer;
            _editorFactoryGuid = editorFactoryGuid;
            _fileExtension = fileExtension;
            _contentName = contentName;
        }

        public int CreateEditorInstance(uint grfCreateDoc, string pszMkDocument, string pszPhysicalView, IVsHierarchy pvHier, uint itemid, IntPtr punkDocDataExisting, out IntPtr ppunkDocView, out IntPtr ppunkDocData, out string pbstrEditorCaption, out Guid pguidCmdUI, out int pgrfCDW)
        {
            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            pguidCmdUI = _editorFactoryGuid;
            pgrfCDW = 0;
            pbstrEditorCaption = null;

            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0)
            {
                return VSConstants.E_INVALIDARG;
            }
            if (punkDocDataExisting != IntPtr.Zero)
            {
                return VSConstants.VS_E_INCOMPATIBLEDOCDATA;
            }

            ObjectDesignerPane editor = CreateEditorPane();
            ppunkDocView = Marshal.GetIUnknownForObject(editor);
            ppunkDocData = Marshal.GetIUnknownForObject(editor);
            pbstrEditorCaption = " [Design]";

            return VSConstants.S_OK;
        }

        protected ObjectDesignerPane CreateEditorPane()
        {
            return new ObjectDesignerPane(_workspace, _designerSerializer, _viewManager, _editorFactoryGuid, _fileExtension, _contentName);
        }

        public int MapLogicalView(ref Guid rguidLogicalView, out string pbstrPhysicalView)
        {
            int retval = VSConstants.E_NOTIMPL;
            pbstrPhysicalView = null;
            if (rguidLogicalView.Equals(VSConstants.LOGVIEWID_Designer) || rguidLogicalView.Equals(VSConstants.LOGVIEWID_Primary))
            {
                retval = VSConstants.S_OK;
            }
            return retval;
        }
        
        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            return VSConstants.S_OK;
        }

        public int Close()
        {
            return VSConstants.S_OK;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {

        }
    }
}
