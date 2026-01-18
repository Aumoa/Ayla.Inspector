#nullable enable
#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine.Pool;

namespace Ayla
{
    public class InspectorSerializedObject : InspectorMember
    {
        private readonly SerializedObject m_SerializedObject;
        private InspectorMember[]? m_Children;

        public InspectorSerializedObject(SerializedObject serializedObject)
        {
            m_SerializedObject = serializedObject;
        }

        public override string ToString()
        {
            return $"{string.Join(", ", m_SerializedObject.targetObjects.Select(p => p.name))}";
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

        public override bool IsReadOnly => true;

        public override void OnInspectorGUI()
        {
            foreach (var child in GetChildren(false))
            {
                child.OnInspectorGUI();
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
                var iterator = m_SerializedObject.GetIterator();
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
                    if (InspectorMonoScript.IsThat(iterator))
                    {
                        children.Add(new InspectorMonoScript(iterator.Copy()));
                    }
                    else if (InspectorObjectHideFlags.IsThat(iterator))
                    {
                        children.Add(new InspectorObjectHideFlags(iterator.Copy()));
                    }
                    else
                    {
                        children.Add(new InspectorSerializedProperty(iterator.Copy()));
                    }

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
