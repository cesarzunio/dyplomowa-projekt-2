using Ces.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;
using Unity.Mathematics;

[NoAlias]
public unsafe struct FieldConstColumns : IDatabaseColumns<FieldConst, FieldConstColumns>
{
    [NativeDisableUnsafePtrRestriction, NoAlias]
    public uint* AreaIndex;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public uint* NodeIndex;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public FieldSoil* Soil;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public FieldLandForm* LandForm;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Elevation;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Surface;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature0;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature1;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature2;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature3;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature4;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature5;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature6;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature7;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature8;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature9;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature10;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Temperature11;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall0;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall1;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall2;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall3;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall4;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall5;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall6;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall7;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall8;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall9;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall10;

    [NativeDisableUnsafePtrRestriction, NoAlias]
    public float* Rainfall11;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsCreated() => AreaIndex != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Allocate(Allocator allocator, int capacity)
    {
        AreaIndex = CesMemoryUtility.AllocateCache<uint>(capacity, allocator);
        NodeIndex = CesMemoryUtility.AllocateCache<uint>(capacity, allocator);
        Soil = CesMemoryUtility.AllocateCache<FieldSoil>(capacity, allocator);
        LandForm = CesMemoryUtility.AllocateCache<FieldLandForm>(capacity, allocator);
        Elevation = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Surface = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature0 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature1 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature2 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature3 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature4 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature5 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature6 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature7 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature8 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature9 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature10 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Temperature11 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall0 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall1 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall2 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall3 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall4 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall5 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall6 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall7 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall8 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall9 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall10 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
        Rainfall11 = CesMemoryUtility.AllocateCache<float>(capacity, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose(Allocator allocator)
    {
        CesMemoryUtility.FreeAndNullify(ref AreaIndex, allocator);
        CesMemoryUtility.FreeAndNullify(ref NodeIndex, allocator);
        CesMemoryUtility.FreeAndNullify(ref Soil, allocator);
        CesMemoryUtility.FreeAndNullify(ref LandForm, allocator);
        CesMemoryUtility.FreeAndNullify(ref Elevation, allocator);
        CesMemoryUtility.FreeAndNullify(ref Surface, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature0, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature1, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature2, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature3, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature4, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature5, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature6, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature7, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature8, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature9, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature10, allocator);
        CesMemoryUtility.FreeAndNullify(ref Temperature11, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall0, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall1, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall2, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall3, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall4, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall5, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall6, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall7, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall8, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall9, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall10, allocator);
        CesMemoryUtility.FreeAndNullify(ref Rainfall11, allocator);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly FieldConst Get(int index) => new()
    {
        AreaIndex = AreaIndex[index],
        NodeIndex = NodeIndex[index],
        Soil = Soil[index],
        LandForm = LandForm[index],
        Elevation = Elevation[index],
        Surface = Surface[index],
        Temperature0 = Temperature0[index],
        Temperature1 = Temperature1[index],
        Temperature2 = Temperature2[index],
        Temperature3 = Temperature3[index],
        Temperature4 = Temperature4[index],
        Temperature5 = Temperature5[index],
        Temperature6 = Temperature6[index],
        Temperature7 = Temperature7[index],
        Temperature8 = Temperature8[index],
        Temperature9 = Temperature9[index],
        Temperature10 = Temperature10[index],
        Temperature11 = Temperature11[index],
        Rainfall0 = Rainfall0[index],
        Rainfall1 = Rainfall1[index],
        Rainfall2 = Rainfall2[index],
        Rainfall3 = Rainfall3[index],
        Rainfall4 = Rainfall4[index],
        Rainfall5 = Rainfall5[index],
        Rainfall6 = Rainfall6[index],
        Rainfall7 = Rainfall7[index],
        Rainfall8 = Rainfall8[index],
        Rainfall9 = Rainfall9[index],
        Rainfall10 = Rainfall10[index],
        Rainfall11 = Rainfall11[index],
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Set(int index, FieldConst instance)
    {
        AreaIndex[index] = instance.AreaIndex;
        NodeIndex[index] = instance.NodeIndex;
        Soil[index] = instance.Soil;
        LandForm[index] = instance.LandForm;
        Elevation[index] = instance.Elevation;
        Surface[index] = instance.Surface;
        Temperature0[index] = instance.Temperature0;
        Temperature1[index] = instance.Temperature1;
        Temperature2[index] = instance.Temperature2;
        Temperature3[index] = instance.Temperature3;
        Temperature4[index] = instance.Temperature4;
        Temperature5[index] = instance.Temperature5;
        Temperature6[index] = instance.Temperature6;
        Temperature7[index] = instance.Temperature7;
        Temperature8[index] = instance.Temperature8;
        Temperature9[index] = instance.Temperature9;
        Temperature10[index] = instance.Temperature10;
        Temperature11[index] = instance.Temperature11;
        Rainfall0[index] = instance.Rainfall0;
        Rainfall1[index] = instance.Rainfall1;
        Rainfall2[index] = instance.Rainfall2;
        Rainfall3[index] = instance.Rainfall3;
        Rainfall4[index] = instance.Rainfall4;
        Rainfall5[index] = instance.Rainfall5;
        Rainfall6[index] = instance.Rainfall6;
        Rainfall7[index] = instance.Rainfall7;
        Rainfall8[index] = instance.Rainfall8;
        Rainfall9[index] = instance.Rainfall9;
        Rainfall10[index] = instance.Rainfall10;
        Rainfall11[index] = instance.Rainfall11;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Move(int from, int to)
    {
        AreaIndex[to] = AreaIndex[from];
        NodeIndex[to] = NodeIndex[from];
        Soil[to] = Soil[from];
        LandForm[to] = LandForm[from];
        Elevation[to] = Elevation[from];
        Surface[to] = Surface[from];
        Temperature0[to] = Temperature0[from];
        Temperature1[to] = Temperature1[from];
        Temperature2[to] = Temperature2[from];
        Temperature3[to] = Temperature3[from];
        Temperature4[to] = Temperature4[from];
        Temperature5[to] = Temperature5[from];
        Temperature6[to] = Temperature6[from];
        Temperature7[to] = Temperature7[from];
        Temperature8[to] = Temperature8[from];
        Temperature9[to] = Temperature9[from];
        Temperature10[to] = Temperature10[from];
        Temperature11[to] = Temperature11[from];
        Rainfall0[to] = Rainfall0[from];
        Rainfall1[to] = Rainfall1[from];
        Rainfall2[to] = Rainfall2[from];
        Rainfall3[to] = Rainfall3[from];
        Rainfall4[to] = Rainfall4[from];
        Rainfall5[to] = Rainfall5[from];
        Rainfall6[to] = Rainfall6[from];
        Rainfall7[to] = Rainfall7[from];
        Rainfall8[to] = Rainfall8[from];
        Rainfall9[to] = Rainfall9[from];
        Rainfall10[to] = Rainfall10[from];
        Rainfall11[to] = Rainfall11[from];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Copy(in FieldConstColumns from, int capacity)
    {
        CesMemoryUtility.Copy(capacity, AreaIndex, from.AreaIndex);
        CesMemoryUtility.Copy(capacity, NodeIndex, from.NodeIndex);
        CesMemoryUtility.Copy(capacity, Soil, from.Soil);
        CesMemoryUtility.Copy(capacity, LandForm, from.LandForm);
        CesMemoryUtility.Copy(capacity, Elevation, from.Elevation);
        CesMemoryUtility.Copy(capacity, Surface, from.Surface);
        CesMemoryUtility.Copy(capacity, Temperature0, from.Temperature0);
        CesMemoryUtility.Copy(capacity, Temperature1, from.Temperature1);
        CesMemoryUtility.Copy(capacity, Temperature2, from.Temperature2);
        CesMemoryUtility.Copy(capacity, Temperature3, from.Temperature3);
        CesMemoryUtility.Copy(capacity, Temperature4, from.Temperature4);
        CesMemoryUtility.Copy(capacity, Temperature5, from.Temperature5);
        CesMemoryUtility.Copy(capacity, Temperature6, from.Temperature6);
        CesMemoryUtility.Copy(capacity, Temperature7, from.Temperature7);
        CesMemoryUtility.Copy(capacity, Temperature8, from.Temperature8);
        CesMemoryUtility.Copy(capacity, Temperature9, from.Temperature9);
        CesMemoryUtility.Copy(capacity, Temperature10, from.Temperature10);
        CesMemoryUtility.Copy(capacity, Temperature11, from.Temperature11);
        CesMemoryUtility.Copy(capacity, Rainfall0, from.Rainfall0);
        CesMemoryUtility.Copy(capacity, Rainfall1, from.Rainfall1);
        CesMemoryUtility.Copy(capacity, Rainfall2, from.Rainfall2);
        CesMemoryUtility.Copy(capacity, Rainfall3, from.Rainfall3);
        CesMemoryUtility.Copy(capacity, Rainfall4, from.Rainfall4);
        CesMemoryUtility.Copy(capacity, Rainfall5, from.Rainfall5);
        CesMemoryUtility.Copy(capacity, Rainfall6, from.Rainfall6);
        CesMemoryUtility.Copy(capacity, Rainfall7, from.Rainfall7);
        CesMemoryUtility.Copy(capacity, Rainfall8, from.Rainfall8);
        CesMemoryUtility.Copy(capacity, Rainfall9, from.Rainfall9);
        CesMemoryUtility.Copy(capacity, Rainfall10, from.Rainfall10);
        CesMemoryUtility.Copy(capacity, Rainfall11, from.Rainfall11);
    }
}
