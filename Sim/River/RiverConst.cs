using Ces.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;

public struct RiverConst
{
    public RiverConstData Data;
}

public struct RiverConstData : IDisposable
{
    public RawArray<uint> PointsIndexes;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        PointsIndexes.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in RiverConstData data)
    {
        BinarySaveUtility.WriteRawArray(in fileStream, data.PointsIndexes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RiverConstData Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
    {
        PointsIndexes = BinaryReadUtility.ReadRawArray<uint>(in fileStream, allocator),
    };
}