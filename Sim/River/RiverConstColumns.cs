using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct RiverConstColumns : IDatabaseColumns<RiverConst, RiverConstColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RiverConstData* Data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => Data != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        Data = CesMemoryUtility.AllocateCache<RiverConstData>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref Data, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly RiverConst Get(int index) => new()
    {
        Data = Data[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, RiverConst instance)
    {
        Data[index] = instance.Data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        Data[to] = Data[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in RiverConstColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, Data, from.Data);
    }
}
