using Accessibility;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Interop.Native.Accessible
{
    public class AccessibleObject : AccessibleObjectCollection, IAccessibleObject
    {
        private const string NotSupportedString = "[NOT SUPPORTED]";

        #region Fields

        private readonly IAccessible _accessible;
        private readonly AccessibleProperties _properties;

        #endregion

        public AccessibleObject(IAccessible accessible)
            : base(accessible)
        {
            _properties = new AccessibleProperties();
            _accessible = accessible;
            InitializeProperties();
        }

        public AccessibleObject(IntPtr parentHandle, string className, string caption)
            : base(NativeMethods.FindAcessibleObject(parentHandle, className, caption) as IAccessible)
        {
        }

        private void InitializeProperties()
        {
            _properties.AddProperty("get_Name", new StringProperty(GetName, true, NotSupportedString));
            _properties.AddProperty("get_ClassName", new StringProperty(GetClassName, true, NotSupportedString));
            _properties.AddProperty("get_Parent", new AccessibleObjectProperty(GetParent, true, null));
            _properties.AddProperty("get_DefaultAction", new StringProperty(GetDefaultAction, true, NotSupportedString));
            _properties.AddProperty("get_Description", new StringProperty(GetDescription, true, NotSupportedString));
            _properties.AddProperty("get_Bounds", new RectangleProperty(GetBounds, true, Rectangle.Empty));
            _properties.AddProperty("get_WindowHandle", new IntPtrProperty(GetWindowHandle, true, IntPtr.Zero));
            _properties.AddProperty("get_Role", new AccessibleRoleProperty(GetRole, true, AccessibleRole.None));
            _properties.AddProperty("get_State", new AccessibleStatesProperty(GetState, true, AccessibleStates.None));
            _properties.AddProperty("get_Value", new StringProperty(GetValue, true, string.Empty));
        }

        #region properties

        public string Name
        {
            get { return _properties.GetPropertyValue<string>(MethodBase.GetCurrentMethod().Name); }
        }

        public string ClassName
        {
            get { return _properties.GetPropertyValue<string>(MethodBase.GetCurrentMethod().Name); }
        }

        public IAccessibleObject Parent
        {
            get { return _properties.GetPropertyValue<IAccessibleObject>(MethodBase.GetCurrentMethod().Name); }
        }

        public string DefaultAction
        {
            get { return _properties.GetPropertyValue<string>(MethodBase.GetCurrentMethod().Name); }
        }

        public string Description
        {
            get { return _properties.GetPropertyValue<string>(MethodBase.GetCurrentMethod().Name); }
        }

        public Rectangle Bounds
        {
            get { return _properties.GetPropertyValue<Rectangle>(MethodBase.GetCurrentMethod().Name); }
        }

        public IntPtr WindowHandle
        {
            get { return _properties.GetPropertyValue<IntPtr>(MethodBase.GetCurrentMethod().Name); }
        }

        public AccessibleRole Role
        {
            get { return _properties.GetPropertyValue<AccessibleRole>(MethodBase.GetCurrentMethod().Name); }
        }

        public AccessibleStates State
        {
            get { return _properties.GetPropertyValue<AccessibleStates>(MethodBase.GetCurrentMethod().Name); }
        }

        public string Value
        {
            get { return _properties.GetPropertyValue<string>(MethodBase.GetCurrentMethod().Name); }
        }

        #endregion

        #region private methods

        private Rectangle GetBounds()
        {
            int left;
            int top;
            int width;
            int height;
            _accessible.accLocation(out left, out top, out width, out height, Constants.CHILDID_SELF);
            return new Rectangle(left, top, width, height);
        }

        private string GetClassName()
        {
            StringBuilder classNameBuilder = new StringBuilder(1024);
            NativeMethods.GetClassName(WindowHandle, classNameBuilder, classNameBuilder.Capacity);
            return classNameBuilder.ToString();
        }

        private string GetName()
        {
            return _accessible.accName;
        }

        private IAccessibleObject GetParent()
        {
            IAccessible parent = _accessible.accParent as IAccessible;
            return parent == null ? null : new AccessibleObject(parent);
        }

        private string GetDefaultAction()
        {
            return _accessible.accDefaultAction[Constants.CHILDID_SELF];
        }

        private IntPtr GetWindowHandle()
        {
            IntPtr handle = IntPtr.Zero;
            NativeMethods.WindowFromAccessibleObject(_accessible, ref handle);
            return handle;
        }

        private string GetDescription()
        {
            return _accessible.accDescription[Constants.CHILDID_SELF];
        }

        private AccessibleRole GetRole()
        {
            return (AccessibleRole)_accessible.accRole[Constants.CHILDID_SELF];
        }

        private AccessibleStates GetState()
        {
            return (AccessibleStates)_accessible.accState[Constants.CHILDID_SELF];
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public string GetValue()
        {
            return _accessible.accValue[Constants.CHILDID_SELF];
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public void SetValue(string value)
        {
            _accessible.accValue[Constants.CHILDID_SELF] = value;
        }

        #endregion

        public void Do()
        {
            try
            {
                _accessible.accDoDefaultAction(Constants.CHILDID_SELF);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        public void Highlight()
        {
            try
            {
                Rectangle bounds = Bounds;
                IntPtr deskDc = NativeMethods.GetDC(IntPtr.Zero);
                using (Graphics graphics = Graphics.FromHdc(deskDc))
                {
                    graphics.DrawRectangle(new Pen(Brushes.Red, 3), bounds.Left, bounds.Top, bounds.Width, bounds.Height);
                }
                NativeMethods.ReleaseDC(deskDc);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        public void Unhighlight()
        {
            NativeMethods.InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
        }

        public override void Refresh()
        {
            base.Refresh();
            _properties.Clear();
        }
    }
}
