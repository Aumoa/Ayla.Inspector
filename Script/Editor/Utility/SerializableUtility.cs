#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Pool;

namespace Ayla.Inspector
{
    public static class SerializableUtility
    {
        private record SerializableMemberCollection
        {
            public Type Type { get; init; } = null!;

            public MemberInfo[] SerializableMembersFlatten { get; init; } = Array.Empty<MemberInfo>();

            public MemberInfo[] SerializableMembersFull { get; init; } = Array.Empty<MemberInfo>();
        }

        private static readonly Dictionary<Type, SerializableMemberCollection> s_SerializableMemberCollection = new();

        public static MemberInfo[] GetSerializableMembers(this Type targetType)
        {
            var collection = InternalGetSerializableMemberCollection(targetType);
            return collection.SerializableMembersFull;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SerializableMemberCollection InternalGetSerializableMemberCollection(Type targetType)
        {
            if (s_SerializableMemberCollection.TryGetValue(targetType, out var collection) == false)
            {
                var flattenMembers = targetType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(member => member is FieldInfo or PropertyInfo)
                    .Where(member => member.IsDefined(typeof(NonSerializedAttribute)) == false)
                    .Where(p => IsUnitySerializeField(p) || IsNativeSerializeField(p))
                    .ToArray();

                using var scope1 = ListPool<MemberInfo>.Get(out var allMembers);
                allMembers.AddRange(flattenMembers);
                for (Type parent = targetType.BaseType; parent != null && parent != typeof(object); parent = parent.BaseType)
                {
                    var parentCollection = InternalGetSerializableMemberCollection(parent);
                    allMembers.AddRange(parentCollection.SerializableMembersFlatten);
                }

                collection = new SerializableMemberCollection
                {
                    Type = targetType,
                    SerializableMembersFlatten = flattenMembers,
                    SerializableMembersFull = allMembers.ToArray()
                };

                static bool IsUnitySerializeField(MemberInfo member)
                {
                    return member is FieldInfo fieldInfo
                        && member.IsDefined(typeof(NonSerializedAttribute)) == false
                        && (member.IsDefined(typeof(SerializeField)) || fieldInfo.IsPublic);
                }

                static bool IsNativeSerializeField(MemberInfo member)
                {
                    return member is PropertyInfo propertyInfo
                        && propertyInfo.GetCustomAttributes().Any(p => p.GetType().FullName == "NativePropertyAttribute");
                }
            }

            return collection;
        }
    }
}
