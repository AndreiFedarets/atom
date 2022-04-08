namespace Atom
{
    public interface IActionInstance : IDataSource
    {
        IArgumentCollection Arguments { get; }

        ActionInstanceMetadata GetMetadata();
    }
}
