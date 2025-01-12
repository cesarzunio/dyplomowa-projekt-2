using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct PopColumns : IDatabaseColumns<Pop, PopColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public uint* FieldIndex;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Amount;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public PopClass* Class;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public PopDemographics* Demographics;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public PopEducations* Educations;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RawSet<DatabaseId>* Nationalities;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RawSet<DatabaseId>* Religions;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public RawSet<DatabaseId>* WorkplaceEmploymentsIds;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => FieldIndex != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        FieldIndex = CesMemoryUtility.AllocateCache<uint>(capacity, allocator);
        Amount = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Class = CesMemoryUtility.AllocateCache<PopClass>(capacity, allocator);
        Demographics = CesMemoryUtility.AllocateCache<PopDemographics>(capacity, allocator);
        Educations = CesMemoryUtility.AllocateCache<PopEducations>(capacity, allocator);
        Nationalities = CesMemoryUtility.AllocateCache<RawSet<DatabaseId>>(capacity, allocator);
        Religions = CesMemoryUtility.AllocateCache<RawSet<DatabaseId>>(capacity, allocator);
        WorkplaceEmploymentsIds = CesMemoryUtility.AllocateCache<RawSet<DatabaseId>>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref FieldIndex, allocator);
        CesMemoryUtility.FreeAndNullify(ref Amount, allocator);
        CesMemoryUtility.FreeAndNullify(ref Class, allocator);
        CesMemoryUtility.FreeAndNullify(ref Demographics, allocator);
        CesMemoryUtility.FreeAndNullify(ref Educations, allocator);
        CesMemoryUtility.FreeAndNullify(ref Nationalities, allocator);
        CesMemoryUtility.FreeAndNullify(ref Religions, allocator);
        CesMemoryUtility.FreeAndNullify(ref WorkplaceEmploymentsIds, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Pop Get(int index) => new()
    {
        FieldIndex = FieldIndex[index],
        Amount = Amount[index],
        Class = Class[index],
        Demographics = Demographics[index],
        Educations = Educations[index],
        Nationalities = Nationalities[index],
        Religions = Religions[index],
        WorkplaceEmploymentsIds = WorkplaceEmploymentsIds[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, Pop instance)
    {
        FieldIndex[index] = instance.FieldIndex;
        Amount[index] = instance.Amount;
        Class[index] = instance.Class;
        Demographics[index] = instance.Demographics;
        Educations[index] = instance.Educations;
        Nationalities[index] = instance.Nationalities;
        Religions[index] = instance.Religions;
        WorkplaceEmploymentsIds[index] = instance.WorkplaceEmploymentsIds;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        FieldIndex[to] = FieldIndex[from];
        Amount[to] = Amount[from];
        Class[to] = Class[from];
        Demographics[to] = Demographics[from];
        Educations[to] = Educations[from];
        Nationalities[to] = Nationalities[from];
        Religions[to] = Religions[from];
        WorkplaceEmploymentsIds[to] = WorkplaceEmploymentsIds[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in PopColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, FieldIndex, from.FieldIndex);
        CesMemoryUtility.Copy(capacity, Amount, from.Amount);
        CesMemoryUtility.Copy(capacity, Class, from.Class);
        CesMemoryUtility.Copy(capacity, Demographics, from.Demographics);
        CesMemoryUtility.Copy(capacity, Educations, from.Educations);
        CesMemoryUtility.Copy(capacity, Nationalities, from.Nationalities);
        CesMemoryUtility.Copy(capacity, Religions, from.Religions);
        CesMemoryUtility.Copy(capacity, WorkplaceEmploymentsIds, from.WorkplaceEmploymentsIds);
    }
}
