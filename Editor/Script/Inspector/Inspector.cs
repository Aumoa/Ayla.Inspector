#nullable enable

using UnityEditor;

namespace Ayla
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(object), true)]
    public class Inspector : Editor
    {
        private const string MissingScriptMessage =
            "This object cannot be inspected because its script is missing or failed to load. " +
            "Restore the script or remove the broken object reference.";

        private InspectorSerializedObject? m_TargetObject;

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
            m_TargetObject = null;
        }

        public override void OnInspectorGUI()
        {
            if (m_TargetObject == null)
            {
                EditorGUILayout.HelpBox(MissingScriptMessage, MessageType.Error);
                return;
            }

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
