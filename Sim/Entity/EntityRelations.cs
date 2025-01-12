using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct EntityRelations
{
    public EntityRelationsFlags Flags;

    public static EntityRelations Default => new()
    {
        Flags = 0,
    };
}

[StructLayout(LayoutKind.Sequential)]
public struct EntityRelationsFlags
{
    public const ulong ALWAYS_FIGHT = 1ul << 10;

    public ulong Flags;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ulong(EntityRelationsFlags flags)
    {
        return flags.Flags;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator EntityRelationsFlags(ulong flags) => new()
    {
        Flags = flags
    };
}