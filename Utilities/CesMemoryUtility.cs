using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

public static unsafe class CesMemoryUtility
{
    public const int CACHE_LINE_SIZE = 64;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Allocate<T>(long length, Allocator allocator) where T : unmanaged
    {
        long sizeT = UnsafeUtility.SizeOf<T>() * length;
        int alignOfT = UnsafeUtility.AlignOf<T>();

        var ptr = (T*)UnsafeUtility.Malloc(sizeT, alignOfT, allocator);

        return ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* AllocateCache<T>(long length, Allocator allocator) where T : unmanaged
    {
        long sizeT = UnsafeUtility.SizeOf<T>() * length;
        int alignOfT = math.max(UnsafeUtility.AlignOf<T>(), CACHE_LINE_SIZE);

        var ptr = (T*)UnsafeUtility.Malloc(sizeT, alignOfT, allocator);

        return ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Allocate<T>(long length, Allocator allocator, T valueDefault) where T : unmanaged
    {
        var ptr = Allocate<T>(length, allocator);

        for (int i = 0; i < length; i++)
        {
            ptr[i] = valueDefault;
        }

        return ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* AllocateCache<T>(long length, Allocator allocator, T valueDefault) where T : unmanaged
    {
        var ptr = AllocateCache<T>(length, allocator);

        for (int i = 0; i < length; i++)
        {
            ptr[i] = valueDefault;
        }

        return ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T** AllocatePtrs<T>(long length, Allocator allocator) where T : unmanaged
    {
        long sizePtr = UnsafeUtility.SizeOf<IntPtr>() * length;
        int alignOfPtr = UnsafeUtility.AlignOf<IntPtr>();

        return (T**)UnsafeUtility.Malloc(sizePtr, alignOfPtr, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(long length, T* destination, T* source) where T : unmanaged
    {
        long sizeT = UnsafeUtility.SizeOf<T>() * length;

        UnsafeUtility.MemCpy(destination, source, sizeT);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyAndFree<T>(long length, T* destination, T* source, Allocator allocator) where T : unmanaged
    {
        Copy(length, destination, source);

        UnsafeUtility.Free(source, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ShiftLeftByOne<T>(T* destination, long elementsToMove) where T : unmanaged
    {
        long sizeT = UnsafeUtility.SizeOf<T>() * elementsToMove;

        UnsafeUtility.MemMove(destination, destination + 1, sizeT);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ShiftRightByOne<T>(T* destination, long elementsToMove) where T : unmanaged
    {
        long sizeT = UnsafeUtility.SizeOf<T>() * elementsToMove;

        UnsafeUtility.MemMove(destination, destination - 1, sizeT);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FreeAndNullify<T>(ref T* ptr, Allocator allocator) where T : unmanaged
    {
        UnsafeUtility.Free(ptr, allocator);
        ptr = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FreeAndNullify<T>(ref T** ptr, Allocator allocator) where T : unmanaged
    {
        UnsafeUtility.Free(ptr, allocator);
        ptr = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetSafeSizeT(int stride, long length)
    {
        if (stride <= 0)
            throw new Exception($"BinaryUtility :: GetSafeSizeT :: Stride ({stride}) is lower than 0!");

        long sizeT = stride * length;

        if (sizeT > int.MaxValue)
            throw new Exception($"BinaryUtility :: GetSafeSizeT :: SizeT ({sizeT}) exceeds int.MaxValue!");

        return (int)sizeT;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MemSet<T>(T* array, T value, long length) where T : unmanaged
    {
        for (int i = 0; i < length; i++)
        {
            array[i] = value;
        }
    }
}
