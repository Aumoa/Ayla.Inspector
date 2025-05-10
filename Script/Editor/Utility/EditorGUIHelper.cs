#if UNITY_EDITOR
#nullable enable

using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector
{
    public static partial class EditorGUIHelper
    {
        public static GUIContent TempIconContent(string iconName, string? text = null, string? tooltip = null)
        {
            var iconContent = EditorGUIUtility.IconContent(iconName);
            var content = TempContent(text, tooltip, iconContent.image);
            return content;
        }
    }
}
#endif