#nullable enable

using System;

namespace Ayla
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