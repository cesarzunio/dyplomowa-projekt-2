using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using UnityEngine;
using Unity.VisualScripting;
using Ces.Collections;

public static unsafe class BinaryReadUtility
{
    const int STRING_BYTES_CAPACITY = 8192;

    public static T ReadValue<T>(this FileStream fileStream) where T : unmanaged
    {
        int sizeOfT = UnsafeUtility.SizeOf<T>();
        var array = stackalloc byte[sizeOfT];

        var span = new Span<byte>(array, sizeOfT);
        int bytesRead = fileStream.Read(span);

        if (bytesRead != sizeOfT)
        {
            throw new Exception("SaveUtility :: ReadValue :: Wrong number of bytes read!");
        }

        return *(T*)array;
    }

    public static string ReadString(this FileStream fileStream)
    {
        int bytesCount = fileStream.ReadValue<int>();

        if (bytesCount == 0)
            return string.Empty;

        if (bytesCount > STRING_BYTES_CAPACITY)
            throw new Exception($"BinaryReadUtility :: ReadString :: String length ({bytesCount}) exceeds max capacity!");

        var byteBuffer = stackalloc byte[bytesCount];

        var span = new Span<byte>(byteBuffer, bytesCount);
        int bytesRead = fileStream.Read(span);

        if (bytesRead != bytesCount)
            throw new Exception("BinaryReadUtility :: ReadString :: Wrong number of bytes read!");

        int charCount = System.Text.Encoding.UTF8.GetCharCount(byteBuffer, bytesCount);
        var charBuffer = stackalloc char[charCount];

        System.Text.Encoding.UTF8.GetChars(byteBuffer, bytesCount, charBuffer, charCount);
        return new string(charBuffer, 0, charCount);
    }

    public static void ReadArraySimple<T>(in FileStream fileStream, int length, T* array) where T : unmanaged
    {
        if (length <= 0)
            return;

        int sizeOfT = UnsafeUtility.SizeOf<T>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        var span = new Span<byte>(array, sizeT);
        int bytesRead = fileStream.Read(span);

        if (bytesRead != sizeT)
            throw new Exception("SaveUtility :: ReadArraySimple :: Wrong number of bytes read!");
    }

    public static T* ReadArraySimple<T>(in FileStream fileStream, int length, Allocator allocator) where T : unmanaged
    {
        if (length <= 0)
            return null;

        int sizeOfT = UnsafeUtility.SizeOf<T>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        var array = CesMemoryUtility.Allocate<T>(length, allocator);

        var span = new Span<byte>(array, sizeT);
        int bytesRead = fileStream.Read(span);

        if (bytesRead != sizeT)
            throw new Exception("SaveUtility :: ReadArraySimple :: Wrong number of bytes read!");

        return array;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RawArray<T> ReadRawArray<T>(in FileStream fileStream, Allocator allocator) where T : unmanaged
    {
        var serializationData = GetRawSerializationData<T>(fileStream, allocator);

        return new RawArray<T>(serializationData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RawArray<T> ReadRawArray<T>(string path, Allocator allocator) where T : unmanaged
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        return ReadRawArray<T>(fileStream, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RawSet<T> ReadRawSet<T>(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) where T : unmanaged
    {
        var serializationData = GetRawSerializationData<T>(fileStream, allocator);

        if (serializationData.Array == null && capacityIfEmpty > 0)
            return new RawSet<T>(allocator, capacityIfEmpty);

        return new RawSet<T>(serializationData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RawSet<T> ReadRawSet<T>(string path, Allocator allocator, int capacityIfEmpty) where T : unmanaged
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        return ReadRawSet<T>(fileStream, allocator, capacityIfEmpty);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static RawSerializationData<T> GetRawSerializationData<T>(in FileStream fileStream, Allocator allocator) where T : unmanaged
    {
        int length = fileStream.ReadValue<int>();

        if (length <= 0)
            return new RawSerializationData<T>(null, 0, Allocator.None);

        var array = ReadArraySimple<T>(fileStream, length, allocator);
        return new RawSerializationData<T>(array, length, allocator);
    }

    public static RawArray<T>* ReadArraySimpleOfRawArrays<T>(in FileStream fileStream, int length, Allocator allocator) where T : unmanaged
    {
        if (length <= 0)
            return null;

        int sizeOfT = UnsafeUtility.SizeOf<RawArray<T>>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        var array = CesMemoryUtility.Allocate<RawArray<T>>(length, allocator);

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadRawArray<T>(fileStream, allocator);
        }

        return array;
    }

    public static void ReadArraySimpleOfRawArrays<T>(in FileStream fileStream, int length, Allocator allocator, RawArray<T>* array) where T : unmanaged
    {
        if (length <= 0)
            return; // should throw maybe ?

        int sizeOfT = UnsafeUtility.SizeOf<RawArray<T>>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadRawArray<T>(fileStream, allocator);
        }
    }

    public static RawArray<RawArray<T>> ReadRawArrayOfRawArrays<T>(in FileStream fileStream, Allocator allocator) where T : unmanaged
    {
        int length = fileStream.ReadValue<int>();

        var array = new RawArray<RawArray<T>>(allocator, length);

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadRawArray<T>(fileStream, allocator);
        }

        return array;
    }

    public static RawSet<T>* ReadArraySimpleOfRawSets<T>(in FileStream fileStream, int length, Allocator allocator, int capacityIfEmpty) where T : unmanaged
    {
        if (length <= 0)
            return null;

        int sizeOfT = UnsafeUtility.SizeOf<RawSet<T>>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        var array = CesMemoryUtility.Allocate<RawSet<T>>(length, allocator);

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadRawSet<T>(fileStream, allocator, capacityIfEmpty);
        }

        return array;
    }

    public static void ReadArraySimpleOfRawSets<T>(in FileStream fileStream, int length, Allocator allocator, RawSet<T>* array, int capacityIfEmpty) where T : unmanaged
    {
        if (length <= 0)
            return; // should throw maybe ?

        int sizeOfT = UnsafeUtility.SizeOf<RawSet<T>>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadRawSet<T>(fileStream, allocator, capacityIfEmpty);
        }
    }

    public static void ReadArraySimpleOfSerializables<T>(
        in FileStream fileStream, int length, Allocator allocator, T* array,
        int capacityIfEmpty, delegate*<in FileStream, Allocator, int, T> deserializeFunc)
        where T : unmanaged
    {
        if (length <= 0)
            return; // should throw maybe ?

        int sizeOfT = UnsafeUtility.SizeOf<RawSet<T>>();
        int sizeT = CesMemoryUtility.GetSafeSizeT(sizeOfT, length);

        for (int i = 0; i < length; i++)
        {
            array[i] = deserializeFunc(in fileStream, allocator, capacityIfEmpty);
        }
    }

    public static T[] ReadArrayManagedOfSerializables<T>(in FileStream fileStream, int length, delegate*<in FileStream, T> deserializeFunc)
    {
        if (length <= 0)
            throw new Exception($"BinaryReadUtility :: ReadArrayManagedOfSerializables :: Length ({length}) is 0!");

        var array = new T[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = deserializeFunc(in fileStream);
        }

        return array;
    }
}
