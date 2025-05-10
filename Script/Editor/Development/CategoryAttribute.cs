#if UNITY_EDITOR
#nullable enable

using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CategoryAttribute : Attribute
    {
        public readonly string Category;

        public CategoryAttribute(string category)
        {
            Category = category;
        }
    }
}
#endif