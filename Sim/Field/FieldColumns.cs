using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct FieldColumns : IDatabaseColumns<Field, FieldColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public DatabaseId* EntityId;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public FieldLandCover* LandCover;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public FieldLandCoverParams* LandCoverParams;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* WaterLevel;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RawSet<DatabaseId>* PopsIds;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => EntityId != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        EntityId = CesMemoryUtility.AllocateCache<DatabaseId>(capacity, allocator);
        LandCover = CesMemoryUtility.AllocateCache<FieldLandCover>(capacity, allocator);
        LandCoverParams = CesMemoryUtility.AllocateCache<FieldLandCoverParams>(capacity, allocator);
        WaterLevel = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        PopsIds = CesMemoryUtility.AllocateCache<RawSet<DatabaseId>>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref EntityId, allocator);
        CesMemoryUtility.FreeAndNullify(ref LandCover, allocator);
        CesMemoryUtility.FreeAndNullify(ref LandCoverParams, allocator);
        CesMemoryUtility.FreeAndNullify(ref WaterLevel, allocator);
        CesMemoryUtility.FreeAndNullify(ref PopsIds, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Field Get(int index) => new()
    {
        EntityId = EntityId[index],
        LandCover = LandCover[index],
        LandCoverParams = LandCoverParams[index],
        WaterLevel = WaterLevel[index],
        PopsIds = PopsIds[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, Field instance)
    {
        EntityId[index] = instance.EntityId;
        LandCover[index] = instance.LandCover;
        LandCoverParams[index] = instance.LandCoverParams;
        WaterLevel[index] = instance.WaterLevel;
        PopsIds[index] = instance.PopsIds;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        EntityId[to] = EntityId[from];
        LandCover[to] = LandCover[from];
        LandCoverParams[to] = LandCoverParams[from];
        WaterLevel[to] = WaterLevel[from];
        PopsIds[to] = PopsIds[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in FieldColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, EntityId, from.EntityId);
        CesMemoryUtility.Copy(capacity, LandCover, from.LandCover);
        CesMemoryUtility.Copy(capacity, LandCoverParams, from.LandCoverParams);
        CesMemoryUtility.Copy(capacity, WaterLevel, from.WaterLevel);
        CesMemoryUtility.Copy(capacity, PopsIds, from.PopsIds);
    }
}
