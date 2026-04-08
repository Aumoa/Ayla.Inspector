#nullable enable

using System;
using System.Linq;
using UnityEditor;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

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
                using var scope1 = ListPool<InspectorMember>.Get(out var children);
                InspectorUtility.GatherInspectorMembers(iterator, m_SerializedObject.targetObjects, children);
                m_Children = children.ToArray();
            }

            return m_Children;
        }
    }
}
