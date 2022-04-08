namespace Atom
{
    public interface IActionArgumentBinder
    {
        void AutoBindInputArguments(IInstance instance, IAction source);
    }
}
