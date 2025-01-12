using Ces.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;

public struct AreaConst
{
    public AreaConstData Data;
}

public struct AreaConstData : IDisposable
{
    public RawArray<uint> FieldsIndexes;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        FieldsIndexes.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in AreaConstData data)
    {
        BinarySaveUtility.WriteRawArray(in fileStream, data.FieldsIndexes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AreaConstData Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
    {
        FieldsIndexes = BinaryReadUtility.ReadRawArray<uint>(in fileStream, allocator),
    };
}