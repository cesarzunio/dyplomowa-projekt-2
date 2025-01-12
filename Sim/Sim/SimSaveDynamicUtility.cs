using Ces.Collections;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

// Data with consts counter-parts (i.e. fields, nodes, edges) does not serialize its length,
// Length is taken directly from consts when loading

public static unsafe class SimSaveDynamicUtility
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void SaveSim(in Sim sim, string path)
    {
        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

        SaveFields(in sim, fileStream);
        SaveAreas(in sim, fileStream);
        SaveRiverPoints(in sim, fileStream);

        SaveNodes(in sim, fileStream);
        SaveEdges(in sim, fileStream);

        SaveEntities(in sim, fileStream);
        SavePops(in sim, fileStream);

        SaveWorkplaces(in sim, fileStream);
        SaveWorkplaceEmps(in sim, fileStream);

        SaveGroundUnits(in sim, fileStream);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveFields(in Sim sim, FileStream fileStream)
    {
        int length = sim.Fields.Table.Length;
        ref readonly var columns = ref sim.Fields.Table.Columns;

        BinarySaveUtility.WriteArraySimple(fileStream, columns.EntityId, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.LandCoverParams, length);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.WaterLevel, length);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveAreas(in Sim sim, FileStream fileStream)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveRiverPoints(in Sim sim, FileStream fileStream)
    {
        int length = sim.RiverPoints.Table.Length;
        ref readonly var columns = ref sim.RiverPoints.Table.Columns;

        BinarySaveUtility.WriteArraySimpleOfSerializables(fileStream, columns.Data, length, &RiverPointData.Serialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveNodes(in Sim sim, FileStream fileStream)
    {
        int length = sim.Nodes.Table.Length;
        ref readonly var columns = ref sim.Nodes.Table.Columns;

        for (int i = 0; i < length; i++)
        {
            fileStream.WriteValue(columns.GroundUnitsIds[i].Set.Count);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveEdges(in Sim sim, FileStream fileStream)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveEntities(in Sim sim, FileStream fileStream)
    {
        int capacityId = sim.Entities.IdData.Capacity;
        int countTable = sim.Entities.Table.Count;

        fileStream.WriteValue(capacityId);
        fileStream.WriteValue(countTable);

        BinarySaveUtility.WriteArraySimple(in fileStream, sim.Entities.IdData.IdToIndex, capacityId);
        BinarySaveUtility.WriteArraySimple(in fileStream, sim.Entities.IdData.IdToUseCount, capacityId);
        BinarySaveUtility.WriteArraySimple(in fileStream, sim.Entities.Table.IndexToId, countTable);

        int lookupMatrixLength = DatabaseLookupMatrixUtility.GetLookupMatrixLength(capacityId);
        BinarySaveUtility.WriteArraySimple(in fileStream, sim.Entities.IdData.LookupMatrix, lookupMatrixLength);

        ref readonly var columns = ref sim.Entities.Table.Columns;

        BinarySaveUtility.WriteArraySimple(in fileStream, columns.MapColor, countTable);
        BinarySaveUtility.WriteArraySimple(in fileStream, columns.DisplayData, countTable);
        BinarySaveUtility.WriteArraySimple(in fileStream, columns.Id, countTable);
        BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.FamilyIds, countTable, &EntityFamilyIds.Serialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SavePops(in Sim sim, FileStream fileStream)
    {
        int capacityId = sim.Pops.IdData.Capacity;
        int countTable = sim.Pops.Table.Count;

        fileStream.WriteValue(capacityId);
        fileStream.WriteValue(countTable);

        BinarySaveUtility.WriteArraySimple(in fileStream, sim.Pops.IdData.IdToIndex, capacityId);
        BinarySaveUtility.WriteArraySimple(in fileStream, sim.Pops.IdData.IdToUseCount, capacityId);
        BinarySaveUtility.WriteArraySimple(in fileStream, sim.Pops.Table.IndexToId, countTable);

        ref readonly var columns = ref sim.Pops.Table.Columns;

        BinarySaveUtility.WriteArraySimple(fileStream, columns.FieldIndex, countTable);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Amount, countTable);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Class, countTable);
        BinarySaveUtility.WriteArraySimple(fileStream, columns.Demographics, countTable);
        BinarySaveUtility.WriteArraySimpleOfRawSets(fileStream, columns.Nationalities, countTable);
        BinarySaveUtility.WriteArraySimpleOfRawSets(fileStream, columns.Religions, countTable);
        BinarySaveUtility.WriteArraySimpleOfRawSets(fileStream, columns.WorkplaceEmploymentsIds, countTable);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveWorkplaces(in Sim sim, FileStream fileStream)
    {
        //var workplacesColumns = sim.Workplaces.Table.Columns;
        //int workplacesLength = sim.Workplaces.Table.Count;

        //BinarySaveUtility.WriteDatabaseLengths(fileStream, in sim.Workplaces);
        //BinarySaveUtility.WriteDatabaseArrays(fileStream, in sim.Workplaces);

        //BinarySaveUtility.WriteArraySimple(fileStream, workplacesColumns.Type, workplacesLength);
        //BinarySaveUtility.WriteArraySimple(fileStream, workplacesColumns.Level, workplacesLength);
        //BinarySaveUtility.WriteArraySimple(fileStream, workplacesColumns.FieldIndex, workplacesLength);
        //BinarySaveUtility.WriteArraySimple(fileStream, workplacesColumns.Owner, workplacesLength);
        //BinarySaveUtility.WriteArraySimpleOfRawSets(fileStream, workplacesColumns.EmploymentsIds, workplacesLength);
    }

    static void SaveWorkplaceEmps(in Sim sim, FileStream fileStream)
    {
        //var workplaceEmpsColumns = sim.WorkplaceEmployments.Table.Columns;
        //int workplaceEmpsLength = sim.WorkplaceEmployments.Table.Count;

        //BinarySaveUtility.WriteDatabaseLengths(fileStream, in sim.WorkplaceEmployments);
        //BinarySaveUtility.WriteDatabaseArrays(fileStream, in sim.WorkplaceEmployments);

        //BinarySaveUtility.WriteArraySimple(fileStream, workplaceEmpsColumns.Id, workplaceEmpsLength);
        //BinarySaveUtility.WriteArraySimple(fileStream, workplaceEmpsColumns.WorkplaceId, workplaceEmpsLength);
        //BinarySaveUtility.WriteArraySimple(fileStream, workplaceEmpsColumns.PopId, workplaceEmpsLength);
        //BinarySaveUtility.WriteArraySimple(fileStream, workplaceEmpsColumns.PopAmount, workplaceEmpsLength);
        //BinarySaveUtility.WriteArraySimple(fileStream, workplaceEmpsColumns.WagePerPop, workplaceEmpsLength);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void SaveGroundUnits(in Sim sim, FileStream fileStream)
    {
        int capacityId = sim.GroundUnits.IdData.Capacity;

        fileStream.WriteValue(capacityId);
        DatabaseSerializeUtility.WriteDatabaseMulTablesCounts(in fileStream, in sim.GroundUnits.Tables, GroundUnitFlags.TABLES_COUNT_TOTAL);

        BinarySaveUtility.WriteArraySimple(in fileStream, sim.GroundUnits.IdData.IdToIndex, capacityId);
        BinarySaveUtility.WriteArraySimple(in fileStream, sim.GroundUnits.IdData.IdToUseCount, capacityId);
        DatabaseSerializeUtility.WriteDatabaseMulTables(in fileStream, in sim.GroundUnits.Tables, GroundUnitFlags.TABLES_COUNT_TOTAL);

        for (int i = 0; i < sim.GroundUnits.TablesAmount; i++)
        {
            ref readonly var table = ref sim.GroundUnits.MapTableIndexToTable(i);

            int countTable = table.Count;
            ref readonly var columns = ref table.Columns;

            BinarySaveUtility.WriteArraySimple(fileStream, columns.Id, countTable);
            BinarySaveUtility.WriteArraySimple(fileStream, columns.EntityId, countTable);
            BinarySaveUtility.WriteArraySimple(fileStream, columns.NodeIndex, countTable);
            BinarySaveUtility.WriteArraySimpleOfSerializables(in fileStream, columns.Movement, countTable, &GroundUnitMovement.Serialize);
        }
    }
}
