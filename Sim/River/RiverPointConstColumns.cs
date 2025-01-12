using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct RiverPointConstColumns : IDatabaseColumns<RiverPointConst, RiverPointConstColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RiverPointConstData* Data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => Data != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        Data = CesMemoryUtility.AllocateCache<RiverPointConstData>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref Data, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly RiverPointConst Get(int index) => new()
    {
        Data = Data[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, RiverPointConst instance)
    {
        Data[index] = instance.Data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        Data[to] = Data[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in RiverPointConstColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, Data, from.Data);
    }
}
