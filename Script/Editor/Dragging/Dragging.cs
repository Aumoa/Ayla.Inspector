#if UNITY_EDITOR
#nullable enable
using System;
using UnityEngine;

namespace Ayla.Inspector
{
    public static class Dragging
    {
        private struct InputState
        {
            public int Button;
            public bool Alt;
            public bool Shift;
            public bool Begun;

            public readonly bool Contains(Event current)
            {
                return Button == current.button && Alt == current.alt && Shift == current.shift;
            }
        }

        private static IDragEventHandler? s_CurrentHandler;
        private static DragType s_DragType;
        private static InputState s_InputState;

        public static void Move(IDragEventHandler handler) => InternalReady(handler, DragType.Move);
        public static void Resize(IDragEventHandler handler) => InternalReady(handler, DragType.Resize);
        public static void ResizeLeft(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeLeft);
        public static void ResizeTop(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeTop);
        public static void ResizeRight(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeRight);
        public static void ResizeBottom(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeBottom);
        public static void ResizeLeftTop(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeLeftTop);
        public static void ResizeRightTop(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeRightTop);
        public static void ResizeRightBottom(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeRightBottom);
        public static void ResizeLeftBottom(IDragEventHandler handler) => InternalReady(handler, DragType.ResizeLeftBottom);

        private static void InternalReady(IDragEventHandler handler, DragType type)
        {
            var current = Event.current ?? throw new ArgumentNullException();
            InternalReady(handler, current.button, current.alt, current.shift, type);
        }

        private static void InternalReady(IDragEventHandler handler, int button, bool alt, bool shift, DragType dragType)
        {
            InternalEndDrag();
            s_CurrentHandler = handler;
            s_DragType = dragType;
            s_InputState.Button = button;
            s_InputState.Alt = alt;
            s_InputState.Shift = shift;
            s_InputState.Begun = false;
        }

        private static void InternalEndDrag()
        {
            if (s_CurrentHandler != null)
            {
                if (s_InputState.Begun)
                {
                    s_CurrentHandler.EndDrag();
                    if (s_CurrentHandler is IRepaintHandler repaint)
                    {
                        repaint.Repaint();
                    }
                    s_InputState = default;
                }

                s_CurrentHandler = null;
            }
        }

        public static void HandleEvents()
        {
            if (s_CurrentHandler == null)
            {
                return;
            }

            var current = Event.current;
            if (current == null)
            {
                return;
            }

            if (current.rawType is not (EventType.MouseDown or EventType.MouseDrag or EventType.MouseUp))
            {
                return;
            }

            if (s_InputState.Contains(current) == false)
            {
                return;
            }

            switch (current.rawType)
            {
                case EventType.MouseDrag:
                    if (s_InputState.Begun == false)
                    {
                        s_CurrentHandler.BeginDrag(s_DragType);
                        s_InputState.Begun = true;
                    }

                    s_CurrentHandler.DuringDrag(current.delta);
                    break;
                case EventType.MouseUp:
                    InternalEndDrag();
                    break;
            }
        }
    }
}
#endif