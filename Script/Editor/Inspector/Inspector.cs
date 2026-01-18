#nullable enable

using UnityEditor;
using Object = UnityEngine.Object;

namespace Ayla
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class Inspector : Editor
    {
        private InspectorSerializedObject m_TargetObject = null!;

        private void OnEnable()
        {
            m_TargetObject = new InspectorSerializedObject(serializedObject);
        }

        private void OnDisable()
        {
            m_TargetObject.Dispose();
            m_TargetObject = null!;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            m_TargetObject.OnInspectorGUI();
            if (serializedObject.ApplyModifiedProperties())
            {
                m_TargetObject.OnApplyModifiedProperties();
            }
        }
    }
}