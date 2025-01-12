using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor.Compilation;
using UnityEngine;
using Ces.Collections;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;

public static unsafe class SimLoadPersistentUtility
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static RawPtr<Sim> LoadSim(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var sim = new Sim();

        LoadFieldsMap(ref sim, fileStream, allocator);
        LoadFields(ref sim, fileStream, allocator);
        LoadAreas(ref sim, fileStream, allocator);

        LoadRivers(ref sim, fileStream, allocator);
        LoadRiverPoints(ref sim, fileStream, allocator);

        LoadNodes(ref sim, fileStream, allocator);
        LoadNodeEdges(ref sim, fileStream, allocator);

        return new RawPtr<Sim>(allocator, sim);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadFieldsMap(ref Sim sim, FileStream fileStream, Allocator allocator)
    {
        var textureSize = fileStream.ReadValue<int2>();
        sim.FieldsMap = new FieldsMap(allocator, textureSize);

        BinaryReadUtility.ReadArraySimple(fileStream, textureSize.x * textureSize.y, sim.FieldsMap.Fields);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadFields(ref Sim sim, FileStream fileStream, Allocator allocator)
    {
        int length = fileStream.ReadValue<int>();
        sim.FieldsConst = new DatabaseStatic<FieldConst, FieldConstColumns>(allocator, length);
        ref var fieldsColumns = ref sim.FieldsConst.Table.Columns;

        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.AreaIndex);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.NodeIndex);

        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Soil);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.LandForm);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Elevation);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Surface);

        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature0);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature1);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature2);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature3);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature4);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature5);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature6);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature7);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature8);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature9);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature10);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Temperature11);

        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall0);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall1);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall2);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall3);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall4);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall5);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall6);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall7);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall8);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall9);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall10);
        BinaryReadUtility.ReadArraySimple(fileStream, length, fieldsColumns.Rainfall11);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadAreas(ref Sim sim, FileStream fileStream, Allocator allocator)
    {
        int length = fileStream.ReadValue<int>();
        sim.AreasConst = new DatabaseStatic<AreaConst, AreaConstColumns>(allocator, length);
        ref var columns = ref sim.AreasConst.Table.Columns;

        BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, length, allocator, columns.Data, -1, &AreaConstData.Deserialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadRivers(ref Sim sim, FileStream fileStream, Allocator allocator)
    {
        int length = fileStream.ReadValue<int>();
        sim.RiversConst = new DatabaseStatic<RiverConst, RiverConstColumns>(allocator, length);
        ref var columns = ref sim.RiversConst.Table.Columns;

        BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, length, allocator, columns.Data, -1, &RiverConstData.Deserialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadRiverPoints(ref Sim sim, FileStream fileStream, Allocator allocator)
    {
        int length = fileStream.ReadValue<int>();
        sim.RiverPointsConst = new DatabaseStatic<RiverPointConst, RiverPointConstColumns>(allocator, length);
        ref var columns = ref sim.RiverPointsConst.Table.Columns;

        BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, length, allocator, columns.Data, -1, &RiverPointConstData.Deserialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadNodes(ref Sim sim, FileStream fileStream, Allocator allocator)
    {
        int length = fileStream.ReadValue<int>();
        sim.NodesConst = new DatabaseStatic<NodeConst, NodeConstColumns>(allocator, length);
        ref var columns = ref sim.NodesConst.Table.Columns;

        BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, length, allocator, columns.CenterUnitSphere, -1, &CesWrapper32<double3>.Deserialize);
        BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, length, allocator, columns.Data, -1, &NodeConstData.Deserialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadNodeEdges(ref Sim sim, FileStream fileStream, Allocator allocator)
    {
        int length = fileStream.ReadValue<int>();
        sim.EdgesConst = new DatabaseStatic<EdgeConst, EdgeColumns>(allocator, length);
        ref var columns = ref sim.EdgesConst.Table.Columns;

        BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, length, allocator, columns.Data, -1, &EdgeConstData.Deserialize);
    }
}
