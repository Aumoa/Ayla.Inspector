using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Ayla;

public class InspectorMonoScript : InspectorSerializedProperty
{
    private GUIContent? m_Label;

    public InspectorMonoScript(SerializedProperty serializedProperty) : base(serializedProperty)
    {
    }

    public override bool IsReadOnly => false;

    public override void OnInspectorGUI()
    {
        using (GUIScope.Disabled(IsReadOnly == false))
        {
            m_Label ??= new GUIContent("Ayla Script");
            EditorGUILayout.PropertyField(Current, m_Label, false);
        }
    }

    public static bool IsThat(SerializedProperty property)
    {
        return PPTR(property.type) == "MonoScript" && property.propertyPath == "m_Script";
    }

    private static string? PPTR(string input)
    {
        var typename = s_PPtrRegex.Match(input);
        if (typename.Success)
        {
            return typename.Groups[1].Value;
        }
        else
        {
            return null;
        }
    }

    private static readonly Regex s_PPtrRegex = new(@"PPtr<([^<>]+(?:<[^<>]+>)?)>");
}
