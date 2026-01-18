#nullable enable
using UnityEngine;

namespace Ayla
{
    public interface IDragEventHandler
    {
        object GetContext();

        void BeginDrag(DragType dragType);
        void DuringDrag(Vector2 delta);
        void EndDrag();
    }
}