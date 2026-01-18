#nullable enable
namespace Ayla
{
    public interface IVerticalSplitBar
    {
        double Position { get; set; }

        void BeginDrag();
        void DuringDrag();
        void EndDrag();
    }
}