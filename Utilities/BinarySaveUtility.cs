using System;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.UIElements;
using Ces.Collections;

public static unsafe class BinarySaveUtility
{
    const int STRING_BYTES_CAPACITY = 8192;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteValue<T>(this FileStream fileStream, T value) where T : unmanaged
    {
        int sizeOfT = UnsafeUtility.SizeOf<T>();
        var array = stackalloc byte[sizeOfT];

        *(T*)array = value;

        fileStream.Write(new ReadOnlySpan<byte>(array, sizeOfT));
    }

    public static void WriteString(this FileStream fileStream, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            fileStream.WriteValue(0);
            return;
        }

        int bytesCount = System.Text.Encoding.UTF8.GetByteCount(value);
        fileStream.WriteValue(bytesCount);

        if (bytesCount > STRING_BYTES_CAPACITY)
            throw new Exception($"BinarySaveUtility :: WriteString :: Passed string ({bytesCount}) exceeds max capacity!");

        var buffer = stackalloc byte[bytesCount];

        fixed (char* valuePtr = value)
        {
            System.Text.Encoding.UTF8.GetBytes(valuePtr, value.Length, buffer, bytesCount);
        }

        fileStream.Write(new ReadOnlySpan<byte>(buffer, bytesCount));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteArraySimple<T>(in FileStream fileStream, T* array, int length) where T : unmanaged
    {
        if (length <= 0)
            return;

        int sizeOfT = UnsafeUtility.SizeOf<T>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        var span = new ReadOnlySpan<byte>(array, sizeT);
        fileStream.Write(span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteRawArray<T>(in FileStream fileStream, RawArray<T> rawArray) where T : unmanaged
    {
        int length = rawArray.Length;
        var data = rawArray.Data;

        fileStream.WriteValue(length);

        WriteArraySimple(fileStream, data, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteRawSet<T>(in FileStream fileStream, RawSet<T> rawSet) where T : unmanaged
    {
        int length = rawSet.Count;
        var data = rawSet.Data;

        fileStream.WriteValue(length);

        WriteArraySimple(fileStream, data, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteRawArrayOfRawArrays<T>(in FileStream fileStream, RawArray<RawArray<T>> rawArrayOfRawArrays) where T : unmanaged
    {
        fileStream.WriteValue(rawArrayOfRawArrays.Length);

        WriteArraySimpleOfRawArrays(fileStream, rawArrayOfRawArrays.Data, rawArrayOfRawArrays.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteArraySimpleOfRawArrays<T>(in FileStream fileStream, RawArray<T>* arrayOfRawArrays, int length) where T : unmanaged
    {
        for (int i = 0; i < length; i++)
        {
            WriteRawArray(fileStream, arrayOfRawArrays[i]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteArraySimpleOfRawSets<T>(in FileStream fileStream, RawSet<T>* arrayOfRawSets, int length) where T : unmanaged
    {
        for (int i = 0; i < length; i++)
        {
            WriteRawSet(fileStream, arrayOfRawSets[i]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteArraySimpleOfSerializables<T>(in FileStream fileStream, T* array, int length, delegate*<in FileStream, in T, void> serializeFunc)
        where T : unmanaged
    {
        for (int i = 0; i < length; i++)
        {
            serializeFunc(in fileStream, in array[i]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteArrayManagedOfSerializables<T>(in FileStream fileStream, T[] array, delegate*<in FileStream, in T, void> serializeFunc)
    {
        for (int i = 0; i < array.Length; i++)
        {
            serializeFunc(in fileStream, in array[i]);
        }
    }
}