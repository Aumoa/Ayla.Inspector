#if UNITY_EDITOR
#nullable enable
namespace Ayla.Inspector
{
    public enum DragType
    {
        Move,
        Resize,
        ResizeLeft,
        ResizeTop,
        ResizeRight,
        ResizeBottom,
        ResizeLeftTop,
        ResizeRightTop,
        ResizeRightBottom,
        ResizeLeftBottom
    }
}
#endif