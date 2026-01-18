using UnityEditor;

namespace Ayla
{
    public class InspectorObjectHideFlags : InspectorSerializedProperty
    {
        public InspectorObjectHideFlags(SerializedProperty serializedProperty) : base(serializedProperty)
        {
        }

        public override bool IsReadOnly => false;

        public override void OnInspectorGUI()
        {
        }

        public static bool IsThat(SerializedProperty property)
        {
            return property.propertyPath == "m_ObjectHideFlags";
        }
    }
}
