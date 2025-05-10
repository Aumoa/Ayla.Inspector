#nullable enable
#if UNITY_EDITOR

using System;
using Ayla.Core;
using UnityEditor;
using UnityEngine.Pool;

namespace Ayla.Inspector
{
    public class InspectorSerializedProperty : InspectorMember
    {
        private readonly SerializedProperty m_SerializedProperty;
        private InspectorMember[]? m_Children;

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

        public override bool IsReadOnly => m_SerializedProperty.editable;

        public override void OnInspectorGUI()
        {
            using (GUIScope.Disabled(IsReadOnly == false))
            {
                EditorGUILayout.PropertyField(m_SerializedProperty, false);

                if (m_SerializedProperty.isExpanded)
                {
                    using var scope1 = EditorGUIScopes.Indent();
                    foreach (var child in GetChildren(false))
                    {
                        child.OnInspectorGUI();
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
                int initial = iterator.depth;
                iterator.Next(true);
                if (initial == iterator.depth)
                {
                    m_Children = Array.Empty<InspectorMember>();
                    return m_Children;
                }

                int depth = iterator.depth;
                using var scope1 = ListPool<InspectorMember>.Get(out var children);
                while (depth == iterator.depth)
                {
                    children.Add(new InspectorSerializedProperty(iterator.Copy()));
                    if (iterator.NextVisible(false) == false)
                    {
                        break;
                    }
                }

                m_Children = children.ToArray();
            }

            return m_Children;
        }
    }
}
#endif
