using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct GroundUnitColumns : IDatabaseColumns<GroundUnit, GroundUnitColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public DatabaseId* Id;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public DatabaseId* EntityId;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public uint* NodeIndex;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public uint* NodeIndexOld;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public GroundUnitMovement* Movement;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RawSet<DatabaseId>* UnitsIdsAttacking;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RawSet<DatabaseId>* UnitsIdsAttackedBy;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => Id != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        Id = CesMemoryUtility.AllocateCache<DatabaseId>(capacity, allocator);
        EntityId = CesMemoryUtility.AllocateCache<DatabaseId>(capacity, allocator);
        NodeIndex = CesMemoryUtility.AllocateCache<uint>(capacity, allocator);
        NodeIndexOld = CesMemoryUtility.AllocateCache<uint>(capacity, allocator);
        Movement = CesMemoryUtility.AllocateCache<GroundUnitMovement>(capacity, allocator);
        UnitsIdsAttacking = CesMemoryUtility.AllocateCache<RawSet<DatabaseId>>(capacity, allocator);
        UnitsIdsAttackedBy = CesMemoryUtility.AllocateCache<RawSet<DatabaseId>>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref Id, allocator);
        CesMemoryUtility.FreeAndNullify(ref EntityId, allocator);
        CesMemoryUtility.FreeAndNullify(ref NodeIndex, allocator);
        CesMemoryUtility.FreeAndNullify(ref NodeIndexOld, allocator);
        CesMemoryUtility.FreeAndNullify(ref Movement, allocator);
        CesMemoryUtility.FreeAndNullify(ref UnitsIdsAttacking, allocator);
        CesMemoryUtility.FreeAndNullify(ref UnitsIdsAttackedBy, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly GroundUnit Get(int index) => new()
    {
        Id = Id[index],
        EntityId = EntityId[index],
        NodeIndex = NodeIndex[index],
        NodeIndexOld = NodeIndexOld[index],
        Movement = Movement[index],
        UnitsIdsAttacking = UnitsIdsAttacking[index],
        UnitsIdsAttackedBy = UnitsIdsAttackedBy[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, GroundUnit instance)
    {
        Id[index] = instance.Id;
        EntityId[index] = instance.EntityId;
        NodeIndex[index] = instance.NodeIndex;
        NodeIndexOld[index] = instance.NodeIndexOld;
        Movement[index] = instance.Movement;
        UnitsIdsAttacking[index] = instance.UnitsIdsAttacking;
        UnitsIdsAttackedBy[index] = instance.UnitsIdsAttackedBy;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        Id[to] = Id[from];
        EntityId[to] = EntityId[from];
        NodeIndex[to] = NodeIndex[from];
        NodeIndexOld[to] = NodeIndexOld[from];
        Movement[to] = Movement[from];
        UnitsIdsAttacking[to] = UnitsIdsAttacking[from];
        UnitsIdsAttackedBy[to] = UnitsIdsAttackedBy[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in GroundUnitColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, Id, from.Id);
        CesMemoryUtility.Copy(capacity, EntityId, from.EntityId);
        CesMemoryUtility.Copy(capacity, NodeIndex, from.NodeIndex);
        CesMemoryUtility.Copy(capacity, NodeIndexOld, from.NodeIndexOld);
        CesMemoryUtility.Copy(capacity, Movement, from.Movement);
        CesMemoryUtility.Copy(capacity, UnitsIdsAttacking, from.UnitsIdsAttacking);
        CesMemoryUtility.Copy(capacity, UnitsIdsAttackedBy, from.UnitsIdsAttackedBy);
    }
}
