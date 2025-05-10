#if UNITY_EDITOR
#nullable enable
namespace Ayla.Inspector
{
    public interface IVerticalSplitBar
    {
        double Position { get; set; }

        void BeginDrag();
        void DuringDrag();
        void EndDrag();
    }
}
#endif