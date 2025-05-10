#if UNITY_EDITOR
#nullable enable

using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NameAttribute : Attribute
    {
        public readonly string Name;

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
#endif