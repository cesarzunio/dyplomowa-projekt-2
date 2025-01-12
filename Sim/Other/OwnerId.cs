using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ces.Collections;

[StructLayout(LayoutKind.Sequential)]
public struct OwnerId<T> where T : Enum
{
    public T Type;
    public DatabaseId Id;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OwnerId(T type, DatabaseId id)
    {
        Type = type;
        Id = id;
    }

    public static OwnerId<T> Invalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(default, DatabaseId.Invalid);
    }
}
