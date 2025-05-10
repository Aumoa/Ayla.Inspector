#if UNITY_EDITOR
#nullable enable
namespace Ayla.Inspector
{
    public interface IRepaintHandler
    {
        void Repaint();
    }
}
#endif