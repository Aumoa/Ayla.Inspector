using System;

namespace Ayla;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class MetaAttribute : Attribute
{
}