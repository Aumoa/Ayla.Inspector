using System;
using System.Diagnostics;

namespace Ayla;

[DebuggerDisplay("{ToString()}")]
public abstract class InspectorMember : IDisposable
{
    ~InspectorMember()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public abstract bool IsReadOnly { get; }

    public abstract void OnInspectorGUI();

    public abstract void OnApplyModifiedProperties();

    public abstract InspectorMember[] GetChildren(bool recurse);
}