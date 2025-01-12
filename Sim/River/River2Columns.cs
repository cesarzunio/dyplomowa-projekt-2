using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Ces.Collections;

public unsafe struct River2Columns : IDatabaseColumns<River2, River2Columns>
{
    [NativeDisableUnsafePtrRestriction]
    public bool* Enabled;

    [NativeDisableUnsafePtrRestriction]
    public RawArray<uint>* PointNodeIndexes;

    [NativeDisableUnsafePtrRestriction]
    public RawArray<RiverPointData>* PointDatas;

    [NativeDisableUnsafePtrRestriction]
    public RiverConnections* StartsFrom;

    [NativeDisableUnsafePtrRestriction]
    public RiverConnections* EndsInto;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => Enabled != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        Enabled = CesMemoryUtility.Allocate<bool>(capacity, allocator);
        PointNodeIndexes = CesMemoryUtility.Allocate<RawArray<uint>>(capacity, allocator);
        PointDatas = CesMemoryUtility.Allocate<RawArray<RiverPointData>>(capacity, allocator);
        StartsFrom = CesMemoryUtility.Allocate<RiverConnections>(capacity, allocator);
        EndsInto = CesMemoryUtility.Allocate<RiverConnections>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref Enabled, allocator);
        CesMemoryUtility.FreeAndNullify(ref PointNodeIndexes, allocator);
        CesMemoryUtility.FreeAndNullify(ref PointDatas, allocator);
        CesMemoryUtility.FreeAndNullify(ref StartsFrom, allocator);
        CesMemoryUtility.FreeAndNullify(ref EndsInto, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly River2 Get(int index) => new()
    {
        Enabled = Enabled[index],
        PointNodeIndexes = PointNodeIndexes[index],
        PointDatas = PointDatas[index],
        StartsFrom = StartsFrom[index],
        EndsInto = EndsInto[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, River2 instance)
    {
        Enabled[index] = instance.Enabled;
        PointNodeIndexes[index] = instance.PointNodeIndexes;
        PointDatas[index] = instance.PointDatas;
        StartsFrom[index] = instance.StartsFrom;
        EndsInto[index] = instance.EndsInto;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        Enabled[to] = Enabled[from];
        PointNodeIndexes[to] = PointNodeIndexes[from];
        PointDatas[to] = PointDatas[from];
        StartsFrom[to] = StartsFrom[from];
        EndsInto[to] = EndsInto[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in River2Columns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, Enabled, from.Enabled);
        CesMemoryUtility.Copy(capacity, PointNodeIndexes, from.PointNodeIndexes);
        CesMemoryUtility.Copy(capacity, PointDatas, from.PointDatas);
        CesMemoryUtility.Copy(capacity, StartsFrom, from.StartsFrom);
        CesMemoryUtility.Copy(capacity, EndsInto, from.EndsInto);
    }
}
