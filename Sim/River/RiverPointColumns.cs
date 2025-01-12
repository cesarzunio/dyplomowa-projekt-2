using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct RiverPointColumns : IDatabaseColumns<RiverPoint, RiverPointColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RiverPointData* Data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => Data != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        Data = CesMemoryUtility.AllocateCache<RiverPointData>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref Data, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly RiverPoint Get(int index) => new()
    {
        Data = Data[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, RiverPoint instance)
    {
        Data[index] = instance.Data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        Data[to] = Data[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in RiverPointColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, Data, from.Data);
    }
}
