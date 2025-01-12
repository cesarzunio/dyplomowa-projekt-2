using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class FlagsUtility
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(byte flagA, byte flagB)
    {
        return (flagA & flagB) > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(uint flagA, uint flagB)
    {
        return (flagA & flagB) > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(ulong flagA, ulong flagB)
    {
        return (flagA & flagB) > 0;
    }
}
