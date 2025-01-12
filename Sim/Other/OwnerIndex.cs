using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct OwnerIndex<T> where T : Enum
{
    public T Type;
    public uint Index;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OwnerIndex(T type, uint index)
    {
        Type = type;
        Index = index;
    }
}
