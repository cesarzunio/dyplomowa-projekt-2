using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static unsafe class SimSavePersistentUtility
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void SaveSim(in Sim sim, string path)
    {
        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

        SaveFieldsMap(in sim, fileStream);
        SaveFields(in sim, fileStream);
        SaveAreas(in sim, fileStream);

        SaveRivers(in sim, fileStream);
        SaveRiverPoints(in sim, fileStream);

        SaveNodes(in sim, fileStream);
        SaveEdges(in sim, fileStream);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveFieldsMap(in Sim sim, FileStream fileStream)
    {
        fileStream.WriteValue(sim.FieldsMap.TextureSize);

        BinarySaveUtility.WriteArraySimple(fileStream, sim.FieldsMap.Fields, sim.FieldsMap.TextureSize.x * sim.FieldsMap.TextureSize.y);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveFields(in Sim sim, FileStream fileStream)
    {
        int length = sim.FieldsConst.Table.Length;
        ref readonly var columns = ref sim.FieldsConst.Table.Columns;

        fileStream.WriteValue(length);

        BinarySaveUtility.WriteArraySimple(fileStream, columns.AreaIndex, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.NodeIndex, length);

        BinarySaveUtility.WriteArraySimple(fileStream, columns.Soil, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.LandForm, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Elevation, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Surface, length);

        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature0, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature1, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature2, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature3, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature4, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature5, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature6, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature7, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature8, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature9, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature10, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Temperature11, length);

        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall0, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall1, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall2, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall3, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall4, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall5, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall6, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall7, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall8, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall9, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall10, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Rainfall11, length);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveAreas(in Sim sim, in FileStream fileStream)
    {
        int length = sim.AreasConst.Table.Length;
        ref readonly var columns = ref sim.AreasConst.Table.Columns;

        fileStream.WriteValue(length);

        BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.Data, length, &AreaConstData.Serialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveRivers(in Sim sim, in FileStream fileStream)
    {
        int length = sim.RiversConst.Table.Length;
        ref readonly var columns = ref sim.RiversConst.Table.Columns;

        fileStream.WriteValue(length);

        BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.Data, length, &RiverConstData.Serialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveRiverPoints(in Sim sim, in FileStream fileStream)
    {
        int length = sim.RiverPointsConst.Table.Length;
        ref readonly var columns = ref sim.RiverPointsConst.Table.Columns;

        fileStream.WriteValue(length);

        BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.Data, length, &RiverPointConstData.Serialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveNodes(in Sim sim, in FileStream fileStream)
    {
        int length = sim.NodesConst.Table.Length;
        ref readonly var columns = ref sim.NodesConst.Table.Columns;

        fileStream.WriteValue(length);

        BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.CenterUnitSphere, length, &CesWrapper32<double3>.Serialize);
        BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.Data, length, &NodeConstData.Serialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveEdges(in Sim sim, in FileStream fileStream)
    {
        int length = sim.EdgesConst.Table.Length;
        ref readonly var columns = ref sim.EdgesConst.Table.Columns;

        fileStream.WriteValue(length);

        BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.Data, length, &EdgeConstData.Serialize);
    }
}
