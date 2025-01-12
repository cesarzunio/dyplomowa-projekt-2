using Ces.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

public struct RiverPointConst
{
    public RiverPointConstData Data;
}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct RiverPointConstData : IDisposable
{
    public uint RiverIndex;
    public uint NodeIndex;
    public int StartsFromFieldIndex;

    public RiverPointConstConnections ConnectionsFrom;
    public RiverPointConstConnections ConnectionsTo;

    public RawArray<uint> CatchmentFieldsIndexes;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        CatchmentFieldsIndexes.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in RiverPointConstData data)
    {
        BinarySaveUtility.WriteValue(fileStream, data.RiverIndex);
        BinarySaveUtility.WriteValue(fileStream, data.NodeIndex);
        BinarySaveUtility.WriteValue(fileStream, data.StartsFromFieldIndex);

        BinarySaveUtility.WriteValue(fileStream, data.ConnectionsFrom);
        BinarySaveUtility.WriteValue(fileStream, data.ConnectionsTo);

        BinarySaveUtility.WriteRawArray(in fileStream, data.CatchmentFieldsIndexes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RiverPointConstData Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
    {
        RiverIndex = fileStream.ReadValue<uint>(),
        NodeIndex = fileStream.ReadValue<uint>(),
        StartsFromFieldIndex = fileStream.ReadValue<int>(),

        ConnectionsFrom = fileStream.ReadValue<RiverPointConstConnections>(),
        ConnectionsTo = fileStream.ReadValue<RiverPointConstConnections>(),

        CatchmentFieldsIndexes = BinaryReadUtility.ReadRawArray<uint>(in fileStream, allocator),
    };
}
