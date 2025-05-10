#if UNITY_EDITOR
#nullable enable
using UnityEngine;

namespace Ayla.Inspector
{
    public interface IDragEventHandler
    {
        object GetContext();

        void BeginDrag(DragType dragType);
        void DuringDrag(Vector2 delta);
        void EndDrag();
    }
}
#endif