using Ces.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public static class RawSetExtensions
{
    public static RawArray<T> ShallowCopy<T>(this ref RawArray<T> original, Allocator allocator) where T : unmanaged
    {
        var copy = new RawArray<T>(allocator, original.Length);

        for (int i = 0; i < original.Length; i++)
        {
            copy[i] = original[i];
        }

        return copy;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeDeep<T>(this ref RawSet<T> rawBag) where T : unmanaged, IDisposable
    {
        for (int i = 0; i < rawBag.Count; i++)
        {
            rawBag[i].Dispose();
        }

        rawBag.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this in RawSet<T> rawSet, T value) where T : unmanaged, IEquatable<T>
    {
        for (int i = 0; i < rawSet.Count; i++)
        {
            if (rawSet[i].Equals(value))
                return i;
        }

        return -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(this in RawSet<T> rawSet, T value) where T : unmanaged, IEquatable<T>
    {
        return IndexOf(in rawSet, value) != -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Remove<T>(this ref RawSet<T> rawSet, T value) where T : unmanaged, IEquatable<T>
    {
        int index = IndexOf(in rawSet, value);

        if (index == -1)
            return false;

        rawSet.RemoveAt(index);
        return true;
    }
}
