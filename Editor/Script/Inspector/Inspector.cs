#nullable enable

using UnityEditor;

namespace Ayla
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(object), true)]
    public class Inspector : Editor
    {
        private InspectorSerializedObject m_TargetObject = null!;

        private void OnEnable()
        {
            if (target == null)
            {
                return;
            }

            m_TargetObject = new InspectorSerializedObject(serializedObject);
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            m_TargetObject?.Dispose();
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

        private void OnUndoRedoPerformed()
        {
            if (m_TargetObject != null)
            {
                m_TargetObject.OnApplyModifiedProperties();
                Repaint();
            }
        }
    }
}