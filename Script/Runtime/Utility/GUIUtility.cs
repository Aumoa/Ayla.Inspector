#nullable enable

using UnityEngine;

namespace Ayla.Inspector
{
    public static partial class EditorGUIHelper
    {
        private static GUIContent? s_TempContent;

        public static GUIContent TempContent(string? text, string? tooltip = null, Texture? image = null)
        {
            s_TempContent ??= new GUIContent();
            s_TempContent.text = text;
            s_TempContent.tooltip = tooltip;
            s_TempContent.image = image;
            return s_TempContent;
        }

        public static GUIContent TempContent(Texture? image)
            => TempContent(null, null, image);
    }
}
