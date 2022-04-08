namespace Atom.Client.VisualStudio
{
    public static class VisualStudioConstants
    {
        public static class LogicalView
        {
            public const string Primary = "00000000-0000-0000-0000-000000000000";
            public const string Any = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";
            public const string Debugging = "7651A700-06E5-11D1-8EBD-00A0C90F26EA";
            public const string Code = "7651A701-06E5-11D1-8EBD-00A0C90F26EA";
            public const string Designer = "7651A702-06E5-11D1-8EBD-00A0C90F26EA";
            public const string Text = "7651A703-06E5-11D1-8EBD-00A0C90F26EA";
            public const string UserChoose = "7651A704-06E5-11D1-8EBD-00A0C90F26EA";
            public const string ProjectSpecific = "80A3471A-6B87-433E-A75A-9D461DE0645F";
        }

        public static class TemplateVariables
        {
            public const string AtomDirVariableName = "$atomdir$";
        }
    }
}
