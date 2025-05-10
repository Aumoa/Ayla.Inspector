#if UNITY_EDITOR
#nullable enable

using System;

namespace Ayla.Inspector
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultOrderAttribute : Attribute
    {
        public readonly int Order;

        public DefaultOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
#endif