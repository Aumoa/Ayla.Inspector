#nullable enable

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Pool;

namespace Ayla
{
    public class InspectorSerializedProperty : InspectorMember
    {
        private readonly SerializedProperty m_SerializedProperty;
        private InspectorMember[]? m_Children;
        private ReorderableList? m_ReorderableList;

        public InspectorSerializedProperty(SerializedProperty serializedProperty)
        {
            m_SerializedProperty = serializedProperty;
        }

        protected SerializedProperty Current => m_SerializedProperty;

        public override string ToString()
        {
            return m_SerializedProperty.name;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_Children != null)
                {
                    foreach (var child in m_Children)
                    {
                        child.Dispose();
                    }
                }

                m_Children = null;
            }

            base.Dispose(disposing);
        }

        public override bool IsReadOnly => !m_SerializedProperty.editable;

        private static GUILayoutOption? s_ArraySizeWidthCache;

        public override void OnInspectorGUI()
        {
            using (GUIScope.Disabled(IsReadOnly))
            {
                bool isArray = m_SerializedProperty.isArray && m_SerializedProperty.propertyType != SerializedPropertyType.String;
                if (isArray)
                {
                    using (EditorGUIScope.Horizontal())
                    {
                        m_SerializedProperty.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(m_SerializedProperty.isExpanded, m_SerializedProperty.displayName);
                        var arraySize = m_SerializedProperty.arraySize;
                        using (GUIScope.Changed())
                        {
                            s_ArraySizeWidthCache ??= GUILayout.Width(50);
                            arraySize = EditorGUILayout.IntField(arraySize, s_ArraySizeWidthCache);
                            if (GUI.changed)
                            {
                                m_SerializedProperty.arraySize = arraySize;
                            }
                        }
                    }

                    if (m_SerializedProperty.isExpanded)
                    {
                        GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

                        try
                        {
                            if (m_ReorderableList == null)
                            {
                                m_ReorderableList = new ReorderableList(m_SerializedProperty.serializedObject, m_SerializedProperty, true, false, true, true)
                                {
                                    elementHeightCallback = index =>
                                    {
                                        using (EditorGUIScope.Indent())
                                        {
                                            var property = m_SerializedProperty.GetArrayElementAtIndex(index);
                                            return EditorGUI.GetPropertyHeight(property, true);
                                        }
                                    },
                                    drawElementCallback = (rect, index, _, _) =>
                                    {
                                        using (EditorGUIScope.Indent())
                                        using (EditorGUIScope.IndentLabelWidth())
                                        {
                                            var property = m_SerializedProperty.GetArrayElementAtIndex(index);
                                            EditorGUI.PropertyField(rect, property, true);
                                        }
                                    }
                                };
                            }
                            m_ReorderableList.DoLayoutList();
                        }
                        finally
                        {
                            EditorGUILayout.EndFoldoutHeaderGroup();
                        }
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(m_SerializedProperty, false);
                    if (m_SerializedProperty.isExpanded)
                    {
                        using var scope1 = EditorGUIScope.Indent();
                        foreach (var child in GetChildren(false))
                        {
                            child.OnInspectorGUI();
                        }
                    }
                }
            }
        }

        public override void OnApplyModifiedProperties()
        {
            if (m_Children != null)
            {
                foreach (var child in m_Children)
                {
                    child.OnApplyModifiedProperties();
                }
            }
        }

        public override InspectorMember[] GetChildren(bool recurse)
        {
            if (m_Children == null)
            {
                var iterator = m_SerializedProperty.Copy();
                using var scope1 = ListPool<InspectorMember>.Get(out var children);
                InspectorUtility.GatherInspectorMembers(iterator, new object[] { iterator.boxedValue }, children);
                m_Children = children.ToArray();
            }

            return m_Children;
        }
    }
}