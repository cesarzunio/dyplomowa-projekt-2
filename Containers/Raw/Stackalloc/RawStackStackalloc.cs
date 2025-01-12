using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public unsafe struct RawStackStackalloc<T> where T : unmanaged
{
    [NativeDisableUnsafePtrRestriction]
    readonly T* _stack;

    int _count;
    readonly int _capacity;

    public readonly int Count => _count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RawStackStackalloc(T* stackPtr, int capacity)
    {
        _stack = stackPtr;
        _count = 0;
        _capacity = capacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T value)
    {
#if CES_COLLECTIONS_CHECK
        if (Count == _capacity)
            throw new Exception("RawStackStackalloc :: Add :: RawBag is full!");
#endif

        _stack[_count++] = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryPop(out T value)
    {
        if (_count == 0)
        {
            value = default;
            return false;
        }

        value = Pop();
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Pop()
    {
#if CES_COLLECTIONS_CHECK
        if (_count == 0)
            throw new Exception("RawStackStackalloc :: Add :: RawBag is full!");
#endif

        return _stack[--_count];
    }
}
