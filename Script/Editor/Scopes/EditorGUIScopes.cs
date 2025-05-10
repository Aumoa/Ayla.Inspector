#if UNITY_EDITOR
#nullable enable

using System;
using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector
{
    public static class EditorGUIScopes
    {
        public readonly struct VerticalBuilder : IDisposable
        {
            public void Dispose()
            {
                EditorGUILayout.EndVertical();
            }
        }

        public static VerticalBuilder Vertical(out Rect outputRect, params GUILayoutOption[] options)
        {
            outputRect = EditorGUILayout.BeginVertical(options);
            return new VerticalBuilder();
        }

        public readonly struct HorizontalBuilder : IDisposable
        {
            public void Dispose()
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        public static HorizontalBuilder Horizontal(out Rect outputRect, params GUILayoutOption[] options)
        {
            outputRect = EditorGUILayout.BeginHorizontal(options);
            return new HorizontalBuilder();
        }
    }
}
#endif