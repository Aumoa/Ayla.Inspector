#nullable enable

using System;

namespace Ayla
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ButtonAttribute : Attribute
    {
    }
}
