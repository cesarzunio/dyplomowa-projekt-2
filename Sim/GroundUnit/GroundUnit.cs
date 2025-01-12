using Ces.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

public struct GroundUnit
{
    public const int UNITS_IDS_ATTACK_CAPACITY_INITIAL = 4;

    public DatabaseId Id;
    public DatabaseId EntityId;

    public uint NodeIndex;
    public uint NodeIndexOld;

    public GroundUnitMovement Movement;

    public RawSet<DatabaseId> UnitsIdsAttacking;
    public RawSet<DatabaseId> UnitsIdsAttackedBy;
}

[StructLayout(LayoutKind.Sequential, Size = 64)]
public struct GroundUnitMovement : IDisposable
{
    public const int NODE_NULL = -1;
    public const int PATH_EDGES_INDEXES_CAPACITY_INITIAL = 32;

    public double DistanceChangePerHour;
    public double DistanceToNext;
    public double DistanceToNextTravelled;

    public RawSet<uint> PathEdgesIndexes;
    public int PathIndexCurrent;

    public int NodeIndexNext;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in GroundUnitMovement groundUnitMovement)
    {
        fileStream.WriteValue(groundUnitMovement.DistanceToNextTravelled);
        BinarySaveUtility.WriteRawSet(fileStream, groundUnitMovement.PathEdgesIndexes);
        fileStream.WriteValue(groundUnitMovement.PathIndexCurrent);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GroundUnitMovement Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
    {
        DistanceToNextTravelled = fileStream.ReadValue<double>(),
        PathEdgesIndexes = BinaryReadUtility.ReadRawSet<uint>(in fileStream, allocator, capacityIfEmpty),
        PathIndexCurrent = fileStream.ReadValue<int>(),
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        PathEdgesIndexes.Dispose();
    }
}