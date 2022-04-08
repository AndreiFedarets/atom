using System;

namespace Atom.Client.VisualStudio
{
    public static class ClientConstants
    {
        public const string PackageGuidString = "B8F11126-9A05-424F-823A-F1195F8E96B7";
        public static readonly Guid PackageGuid;

        static ClientConstants()
        {
            PackageGuid = new Guid(PackageGuidString);
        }

        public static class Editors
        {
            public const string ActionContentName = "Atom Action";
            public const string ConditionContentName = "Atom Condition";
            public const string TableContentName = "Atom Data Table";
            public const string WorkflowContentName = "Atom Workflow";
            public const string ActionDesignerFactoryGuidString = "5251AE81-8137-41AF-BD0A-B045DF010756";
            public const string ConditionDesignerFactoryGuidString = "0ECC2851-D0C0-4582-B868-96D80A8CE7F2";
            public const string WorkflowDesignerFactoryGuidString = "983A112E-60FE-422D-B739-19E7862E6578";
            public const string TableDesignerFactoryGuidString = "0A767216-2D28-4E79-A6BE-B70370FEBF4C";
            public static readonly Guid ActionDesignerFactoryGuid;
            public static readonly Guid ConditionDesignerFactoryGuid;
            public static readonly Guid TableDesignerFactoryGuid;
            public static readonly Guid WorkflowDesignerFactoryGuid;

            static Editors()
            {
                ActionDesignerFactoryGuid = new Guid(ActionDesignerFactoryGuidString);
                ConditionDesignerFactoryGuid = new Guid(ConditionDesignerFactoryGuidString);
                TableDesignerFactoryGuid = new Guid(TableDesignerFactoryGuidString);
                WorkflowDesignerFactoryGuid = new Guid(WorkflowDesignerFactoryGuidString);
            }
        }

        public static class Menus
        {
            public static readonly Guid guidAtomMainMenu;
            public const int IDM_VS_MENU_TOOLS_ATOM = 0x1001;
            public const int IDG_VS_MENU_TOOLS_ATOM = 0x2001;
            public const int ShowActionExplorerCommandId = 0x3001;
            public const int ManageProjectModulesCommandId = 0x3002;

            static Menus()
            {
                guidAtomMainMenu = new Guid("D9348BD8-1F34-41F2-8F78-93D8FBB19E4B");
            }
        }
    }
}
