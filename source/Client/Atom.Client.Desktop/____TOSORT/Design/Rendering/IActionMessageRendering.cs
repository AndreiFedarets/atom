namespace Atom.Rendering
{
    public interface IActionMessageRendering
    {
        IBlockCollection RenderActionMessage(IActionInstance action);
    }
}
