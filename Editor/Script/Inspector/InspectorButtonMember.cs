#nullable enable

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Ayla
{
    public class InspectorButtonMember : InspectorMember
    {
        private readonly MethodInfo m_MethodInfo;
        private readonly object[] m_Callers;
        private readonly string m_Name;

        public InspectorButtonMember(object[] callers, MethodInfo methodInfo)
        {
            m_MethodInfo = methodInfo;
            m_Callers = callers;
            m_Name = ObjectNames.NicifyVariableName(methodInfo.Name);
        }

        public override bool IsReadOnly => false;

        public override InspectorMember[] GetChildren(bool recurse)
        {
            return Array.Empty<InspectorMember>();
        }

        public override void OnApplyModifiedProperties()
        {
        }

        public override void OnInspectorGUI()
        {
            using (GUIScope.Disabled(IsReadOnly))
            {
                if (GUILayout.Button(m_Name))
                {
                    foreach (var caller in m_Callers)
                    {
                        try
                        {
                            m_MethodInfo.Invoke(caller, null);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
            }
        }
    }
}
