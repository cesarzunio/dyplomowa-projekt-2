using Ces.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;

public struct NodeConst
{
    public CesWrapper32<double3> CenterUnitSphere;
    public NodeConstData Data;
}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct NodeConstData : IDisposable
{
    public RawArray<uint> EdgesIndexes;
    public OwnerIndex<NodeConstOwnerType> Owner;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        EdgesIndexes.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in NodeConstData data)
    {
        BinarySaveUtility.WriteRawArray(in fileStream, data.EdgesIndexes);
        fileStream.WriteValue(data.Owner);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NodeConstData Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
    {
        EdgesIndexes = BinaryReadUtility.ReadRawArray<uint>(in fileStream, allocator),
        Owner = fileStream.ReadValue<OwnerIndex<NodeConstOwnerType>>(),
    };
}

[StructLayout(LayoutKind.Sequential)]
public struct NodeConstOwner
{
    public NodeConstOwnerType Type;
    public uint Index;
}

public enum NodeConstOwnerType : uint
{
    Field = 0,
    River = 10,
}