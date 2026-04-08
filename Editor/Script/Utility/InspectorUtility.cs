#nullable enable

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Ayla
{
    internal static class InspectorUtility
    {
        private static class Caches
        {
            private static readonly Dictionary<Type, MemberInfo[]> s_AllMembers = new();

            public static MemberInfo[] GetAllMembers(Type type)
            {
                if (s_AllMembers.TryGetValue(type, out var cached))
                {
                    return cached;
                }

                using var scope1 = ListPool<MemberInfo>.Get(out var members);
                var originalType = type;

                while (type != null && type != typeof(object))
                {
                    members.AddRange(type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                    type = type.BaseType;
                }

                cached = members.ToArray();
                s_AllMembers.Add(originalType, cached);
                return cached;
            }

            public readonly struct SpecialMember
            {
                public readonly MemberInfo Member;
                public readonly Attribute Attr;

                public SpecialMember(MemberInfo member, Attribute attr)
                {
                    Member = member;
                    Attr = attr;
                }
            }

            private static readonly Dictionary<Type, SpecialMember[]> s_SpecialMembers = new();

            public static SpecialMember[] GetSpecialMembers(Type type)
            {
                if (s_SpecialMembers.TryGetValue(type, out var specialMembers))
                {
                    return specialMembers;
                }

                using var scope1 = ListPool<SpecialMember>.Get(out var output);
                foreach (var member in GetAllMembers(type))
                {
                    bool isMethod = member is MethodInfo;
                    if (isMethod && member.GetCustomAttribute<ButtonAttribute>() is { } buttonAttribute)
                    {
                        output.Add(new SpecialMember(member, buttonAttribute));
                        continue;
                    }
                }

                specialMembers = output.ToArray();
                s_SpecialMembers.Add(type, specialMembers);
                return specialMembers;
            }
        }

        public static void GatherInspectorMembers<T>(SerializedProperty iterator, T[] boxedValue, List<InspectorMember> output) where T : class
        {
            int initial = iterator.depth;
            Type? targetType = GetSingleType(boxedValue);

            iterator.Next(true);
            if (initial == iterator.depth)
            {
                return;
            }

            GatherSerializedProperties(iterator, output);
            if (targetType != null)
            {
                GatherSpecialProperties(boxedValue, targetType, output);
            }

            return;

            static Type? GetSingleType(T[] objects)
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

        public static void GatherSerializedProperties(SerializedProperty iterator, List<InspectorMember> output)
        {
            int depth = iterator.depth;
            while (depth == iterator.depth)
            {
                if (InspectorMonoScript.IsThat(iterator))
                {
                    output.Add(new InspectorMonoScript(iterator.Copy()));
                }
                else if (InspectorObjectHideFlags.IsThat(iterator))
                {
                    output.Add(new InspectorObjectHideFlags(iterator.Copy()));
                }
                else
                {
                    output.Add(new InspectorSerializedProperty(iterator.Copy()));
                }

                if (iterator.NextVisible(false) == false)
                {
                    break;
                }
            }
        }

        public static void GatherSpecialProperties<T>(T[] callers, Type objectType, List<InspectorMember> output) where T : class
        {
            object[]? objectCallers = null;

            foreach (var member in Caches.GetSpecialMembers(objectType))
            {
                switch (member.Attr)
                {
                    case ButtonAttribute:
                        output.Add(new InspectorButtonMember(ObjectCallers(), (MethodInfo)member.Member));
                        break;
                }
            }

            return;

            object[] ObjectCallers()
            {
                if (typeof(T) == typeof(object))
                {
                    return callers;
                }

                if (objectCallers == null)
                {
                    objectCallers = new object[callers.Length];
                    for (int i = 0; i < callers.Length; ++i)
                    {
                        objectCallers[i] = callers[i];
                    }
                }

                return objectCallers;
            }
        }
    }
}
