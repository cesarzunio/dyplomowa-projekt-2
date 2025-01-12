using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct NodeConstColumns : IDatabaseColumns<NodeConst, NodeConstColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public CesWrapper32<double3>* CenterUnitSphere;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public NodeConstData* Data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => CenterUnitSphere != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        CenterUnitSphere = CesMemoryUtility.AllocateCache<CesWrapper32<double3>>(capacity, allocator);
        Data = CesMemoryUtility.AllocateCache<NodeConstData>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref CenterUnitSphere, allocator);
        CesMemoryUtility.FreeAndNullify(ref Data, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly NodeConst Get(int index) => new()
    {
        CenterUnitSphere = CenterUnitSphere[index],
        Data = Data[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, NodeConst instance)
    {
        CenterUnitSphere[index] = instance.CenterUnitSphere;
        Data[index] = instance.Data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        CenterUnitSphere[to] = CenterUnitSphere[from];
        Data[to] = Data[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in NodeConstColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, CenterUnitSphere, from.CenterUnitSphere);
        CesMemoryUtility.Copy(capacity, Data, from.Data);
    }
}
