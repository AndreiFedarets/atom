namespace Atom
{
    internal interface IExecutable
    {
        IScope Execute(IScope scope);
    }
}
