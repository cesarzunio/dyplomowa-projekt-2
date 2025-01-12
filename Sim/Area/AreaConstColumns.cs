using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct AreaConstColumns : IDatabaseColumns<AreaConst, AreaConstColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public AreaConstData* Data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => Data != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        Data = CesMemoryUtility.AllocateCache<AreaConstData>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref Data, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly AreaConst Get(int index) => new()
    {
        Data = Data[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, AreaConst instance)
    {
        Data[index] = instance.Data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        Data[to] = Data[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in AreaConstColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, Data, from.Data);
    }
}
