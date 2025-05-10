#if UNITY_EDITOR
#nullable enable
using UnityEngine;

namespace Ayla.Inspector
{
    public interface IEditableObject
    {
        Object GetObject();
    }
}
#endif