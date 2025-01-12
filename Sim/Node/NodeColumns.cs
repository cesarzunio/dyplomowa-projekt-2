using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct NodeColumns : IDatabaseColumns<Node, NodeColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public bool* Enabled;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RawSetWrapper32<DatabaseId>* GroundUnitsIds;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => Enabled != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        Enabled = CesMemoryUtility.AllocateCache<bool>(capacity, allocator);
        GroundUnitsIds = CesMemoryUtility.AllocateCache<RawSetWrapper32<DatabaseId>>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref Enabled, allocator);
        CesMemoryUtility.FreeAndNullify(ref GroundUnitsIds, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Node Get(int index) => new()
    {
        Enabled = Enabled[index],
        GroundUnitsIds = GroundUnitsIds[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, Node instance)
    {
        Enabled[index] = instance.Enabled;
        GroundUnitsIds[index] = instance.GroundUnitsIds;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        Enabled[to] = Enabled[from];
        GroundUnitsIds[to] = GroundUnitsIds[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in NodeColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, Enabled, from.Enabled);
        CesMemoryUtility.Copy(capacity, GroundUnitsIds, from.GroundUnitsIds);
    }
}
