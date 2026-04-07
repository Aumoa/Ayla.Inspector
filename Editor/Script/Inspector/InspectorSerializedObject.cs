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
                int initial = iterator.depth;
                Type? targetType = GetSingleType(m_SerializedObject.targetObjects);

                iterator.Next(true);
                if (initial == iterator.depth)
                {
                    m_Children = Array.Empty<InspectorMember>();
                    return m_Children;
                }

                using var scope1 = ListPool<InspectorMember>.Get(out var children);
                InspectorUtility.GatherSerializedProperties(iterator, children);
                if (targetType != null)
                {
                    InspectorUtility.GatherSpecialProperties(m_SerializedObject.targetObjects, targetType, children);
                }
                m_Children = children.ToArray();
            }

            return m_Children;

            static Type? GetSingleType(Object[] objects)
            {
                if (objects.Length == 0)
                {
                    return null;
                }

                if (objects[0] == null)
                {
                    return null;
                }

                var type = objects[0].GetType();
                for (int i = 1; i < objects.Length; ++i)
                {
                    if (objects[i].GetType() != type)
                    {
                        return null;
                    }
                }

                return type;
            }
        }
    }
}
