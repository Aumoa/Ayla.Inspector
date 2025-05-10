#if UNITY_EDITOR
#nullable enable
using Ayla.Core;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector
{
    public static class VerticalSplitBar
    {
        private class DragEventHandler : IDragEventHandler
        {
            private readonly IVerticalSplitBar m_Context;
            private readonly double m_InitialPosition;
            private double m_Delta;

            public DragEventHandler(IVerticalSplitBar context)
            {
                m_Context = context;
                m_InitialPosition = context.Position;
            }

            public object GetContext()
            {
                return m_Context;
            }

            public void BeginDrag(DragType dragType)
            {
                m_Context.BeginDrag();
            }

            public void DuringDrag(Vector2 delta)
            {
                m_Delta += delta.x;
                m_Context.Position = m_InitialPosition + m_Delta;
                m_Context.DuringDrag();
            }

            public void EndDrag()
            {
                m_Context.EndDrag();
            }
        }

        public const float Width = 2.0f;
        public const float ShadowPixels = VerticalBorder.ShadowPixels;

        public static void Draw(DrawingArgs drawingArgs, IVerticalSplitBar context)
        {
            if (drawingArgs.DrawingRect.width <= 0)
            {
                return;
            }

            VerticalBorder.Draw(drawingArgs);
            EditorGUIUtility.AddCursorRect(drawingArgs.DrawingRect.FillLeft(Width), MouseCursor.SplitResizeLeftRight);

            var current = Event.current;
            if (current != null && drawingArgs.FillLeft(2).Contains(current.mousePosition) && current.rawType == EventType.MouseDown && current.button == 0)
            {
                Dragging.Resize(new DragEventHandler(context));
                current.Use();
            }
        }
    }
}
#endif