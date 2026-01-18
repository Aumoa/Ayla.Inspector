#nullable enable

using System;
using UnityEditor;
using UnityEngine;

namespace Ayla
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

        public readonly struct IndentBuilder : IDisposable
        {
            public readonly int m_Previous;

            public IndentBuilder(int newIndentLevel)
            {
                m_Previous = EditorGUI.indentLevel;
                EditorGUI.indentLevel = newIndentLevel;
            }

            public void Dispose()
            {
                EditorGUI.indentLevel = m_Previous;
            }
        }

        public static IndentBuilder Indent(int plusIndentLevel = 1)
        {
            return new IndentBuilder(EditorGUI.indentLevel + plusIndentLevel);
        }
    }
}