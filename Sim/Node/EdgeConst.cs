using Ces.Collections;
using Unity.Mathematics;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Collections;

public struct EdgeConst
{
    public EdgeConstData Data;
}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct EdgeConstData
{
    public uint2 NodesIndexes;
    public double DistanceGround;
    public double DistanceAir; // TODO recreate at load ?
    public int CrossedRiverPointIndex;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in EdgeConstData data)
    {
        fileStream.WriteValue(data.NodesIndexes);
        fileStream.WriteValue(data.DistanceGround);
        fileStream.WriteValue(data.DistanceAir);
        fileStream.WriteValue(data.CrossedRiverPointIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EdgeConstData Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
    {
        NodesIndexes = fileStream.ReadValue<uint2>(),
        DistanceGround = fileStream.ReadValue<double>(),
        DistanceAir = fileStream.ReadValue<double>(),  // TODO recreate at load ?
        CrossedRiverPointIndex = fileStream.ReadValue<int>(),
    };
}