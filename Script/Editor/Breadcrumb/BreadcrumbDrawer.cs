#if UNITY_EDITOR
#nullable enable
using UnityEngine;

namespace Ayla.Inspector
{
    public static class BreadcrumbDrawer
    {
        public enum PositionType
        {
            Left,
            Mid
        }

        public const float Height = 20;

        private static bool s_CachedComponents;
        private static GUIStyle s_BreadcrumbLeft = null!;
        private static GUIStyle s_BreadcrumbMid = null!;
        private static GUIStyle s_BreadcrumbLeftBg = null!;
        private static GUIStyle s_BreadcrumbMidBg = null!;

        public static bool Draw(DrawingArgs drawingArgs, PositionType positionType, GUIContent content, out float width)
        {
            InitializeComponents();

            drawingArgs = drawingArgs.FillTop(Height);

            GUIStyle backgroundStyle;
            GUIStyle style;

            switch (positionType)
            {
                case PositionType.Left:
                    backgroundStyle = s_BreadcrumbLeftBg;
                    style = s_BreadcrumbLeft;
                    break;
                case PositionType.Mid:
                    backgroundStyle = s_BreadcrumbMidBg;
                    style = s_BreadcrumbMid;
                    break;
                default:
                    width = 0;
                    return false;
            }

            var image = content.image;
            content.image = null;
            var size = style.CalcSize(content);
            content.image = image;
            if (image != null)
            {
                size.x += size.y;  // assumes square image, constrained by height
            }

            drawingArgs = drawingArgs.FillLeft(size.x).MarginLeft(backgroundStyle.overflow.left);
            var current = Event.current;
            if (current.rawType == EventType.Repaint)
            {
                backgroundStyle.Draw(drawingArgs.DrawingRect, GUIContent.none, 0);
            }

            width = size.x - backgroundStyle.overflow.left;
            return GUI.Button(drawingArgs.DrawingRect, content, style);
        }

        private static void InitializeComponents()
        {
            if (s_CachedComponents == false)
            {
                s_BreadcrumbLeft = "GUIEditor.BreadcrumbLeft";
                s_BreadcrumbMid = "GUIEditor.BreadcrumbMid";
                s_BreadcrumbLeftBg = "GUIEditor.BreadcrumbLeftBackground";
                s_BreadcrumbMidBg = "GUIEditor.BreadcrumbMidBackground";
                s_CachedComponents = true;
            }
        }
    }
}
#endif