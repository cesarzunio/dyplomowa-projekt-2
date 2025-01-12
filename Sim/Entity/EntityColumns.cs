using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[NoAlias]
public unsafe struct EntityColumns : IDatabaseColumns<Entity, EntityColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public Color32* MapColor;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public EntityDisplayData* DisplayData;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public DatabaseId* Id;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public EntityFamilyIds* FamilyIds;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public EntityFamilyDataEconomy* FamilyDataEconomy;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public EntityFamilyDataAdministration* FamilyDataAdministration;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public EntityFamilyDataSecurity* FamilyDataSecurity;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public EntityFamilyDataPolitics* FamilyDataPolitics;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => MapColor != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        MapColor = CesMemoryUtility.AllocateCache<Color32>(capacity, allocator, default);
        DisplayData = CesMemoryUtility.AllocateCache<EntityDisplayData>(capacity, allocator, default);
        Id = CesMemoryUtility.AllocateCache<DatabaseId>(capacity, allocator, default);
        FamilyIds = CesMemoryUtility.AllocateCache<EntityFamilyIds>(capacity, allocator, default);
        FamilyDataEconomy = CesMemoryUtility.AllocateCache<EntityFamilyDataEconomy>(capacity, allocator, default);
        FamilyDataAdministration = CesMemoryUtility.AllocateCache<EntityFamilyDataAdministration>(capacity, allocator, default);
        FamilyDataSecurity = CesMemoryUtility.AllocateCache<EntityFamilyDataSecurity>(capacity, allocator, default);
        FamilyDataPolitics = CesMemoryUtility.AllocateCache<EntityFamilyDataPolitics>(capacity, allocator, default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref MapColor, allocator);
        CesMemoryUtility.FreeAndNullify(ref DisplayData, allocator);
        CesMemoryUtility.FreeAndNullify(ref Id, allocator);
        CesMemoryUtility.FreeAndNullify(ref FamilyIds, allocator);
        CesMemoryUtility.FreeAndNullify(ref FamilyDataEconomy, allocator);
        CesMemoryUtility.FreeAndNullify(ref FamilyDataAdministration, allocator);
        CesMemoryUtility.FreeAndNullify(ref FamilyDataSecurity, allocator);
        CesMemoryUtility.FreeAndNullify(ref FamilyDataPolitics, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Entity Get(int index) => new()
    {
        MapColor = MapColor[index],
        DisplayData = DisplayData[index],
        Id = Id[index],
        FamilyIds = FamilyIds[index],
        FamilyDataEconomy = FamilyDataEconomy[index],
        FamilyDataAdministration = FamilyDataAdministration[index],
        FamilyDataSecurity = FamilyDataSecurity[index],
        FamilyDataPolitics = FamilyDataPolitics[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, Entity instance)
    {
        MapColor[index] = instance.MapColor;
        DisplayData[index] = instance.DisplayData;
        Id[index] = instance.Id;
        FamilyIds[index] = instance.FamilyIds;
        FamilyDataEconomy[index] = instance.FamilyDataEconomy;
        FamilyDataAdministration[index] = instance.FamilyDataAdministration;
        FamilyDataSecurity[index] = instance.FamilyDataSecurity;
        FamilyDataPolitics[index] = instance.FamilyDataPolitics;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        MapColor[to] = MapColor[from];
        DisplayData[to] = DisplayData[from];
        Id[to] = Id[from];
        FamilyIds[to] = FamilyIds[from];
        FamilyDataEconomy[to] = FamilyDataEconomy[from];
        FamilyDataAdministration[to] = FamilyDataAdministration[from];
        FamilyDataSecurity[to] = FamilyDataSecurity[from];
        FamilyDataPolitics[to] = FamilyDataPolitics[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in EntityColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, MapColor, from.MapColor);
        CesMemoryUtility.Copy(capacity, DisplayData, from.DisplayData);
        CesMemoryUtility.Copy(capacity, Id, from.Id);
        CesMemoryUtility.Copy(capacity, FamilyIds, from.FamilyIds);
        CesMemoryUtility.Copy(capacity, FamilyDataEconomy, from.FamilyDataEconomy);
        CesMemoryUtility.Copy(capacity, FamilyDataAdministration, from.FamilyDataAdministration);
        CesMemoryUtility.Copy(capacity, FamilyDataSecurity, from.FamilyDataSecurity);
        CesMemoryUtility.Copy(capacity, FamilyDataPolitics, from.FamilyDataPolitics);
    }
}
