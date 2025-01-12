using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using Ces.Collections;
using Unity.Jobs;
using System;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

public static unsafe class SimLoadDynamicUtility
{
    const int FIELDS_POPS_CAPACITY = 8;
    const int NODES_GROUND_UNITS_CAPACITY = 4;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void LoadSim(ref Sim sim, string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        LoadFields(ref sim, fileStream, allocator);
        LoadAreas(ref sim, fileStream, allocator);
        LoadRiverPoints(ref sim, fileStream, allocator);

        LoadNodes(ref sim, fileStream, allocator);
        LoadEdges(ref sim, fileStream);

        LoadEntities(ref sim, fileStream, allocator);
        LoadPops(ref sim, fileStream, allocator);

        LoadWorkplaces(ref sim, fileStream, allocator);
        LoadWorkplaceEmps(ref sim, fileStream, allocator);

        LoadGroundUnits(ref sim, fileStream, allocator);

        // ---

        RecreateFields(ref sim);
        RecreateNodes(ref sim);
        RecreateEntities(ref sim);
        RecreateGroundUnits(ref sim);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadFields(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        int length = sim.FieldsConst.Table.Length;
        sim.Fields = new DatabaseStatic<Field, FieldColumns>(allocator, length);
        ref var columns = ref sim.Fields.Table.Columns;

        BinaryReadUtility.ReadArraySimple(fileStream, length, columns.EntityId);
        BinaryReadUtility.ReadArraySimple(fileStream, length, columns.LandCoverParams);
        BinaryReadUtility.ReadArraySimple(fileStream, length, columns.WaterLevel);

        for (int i = 0; i < length; i++)
        {
            columns.PopsIds[i] = new RawSet<DatabaseId>(allocator, FIELDS_POPS_CAPACITY);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void RecreateFields(ref Sim sim)
    {
        int length = sim.Fields.Table.Length;
        ref var columns = ref sim.Fields.Table.Columns;

        for (int i = 0; i < length; i++)
        {
            columns.LandCover[i] = FieldUtility.CalculateFieldLandCover(in sim.Fields.Table.Columns.LandCoverParams[i]);
        }

        int lengthPops = sim.Pops.Table.Count;

        for (int i = 0; i < lengthPops; i++)
        {
            ref readonly uint fieldIndex = ref sim.Pops.Table.Columns.FieldIndex[i];

            // this is probably not needed, every pop will have its id
            // TODO: cache Id field within Pop

            if (!sim.Pops.TryMapIndexToId(i, out var id))
            {
                id = sim.Pops.CreateIdFromIndex(i);
            }

            columns.PopsIds[fieldIndex].Add(id);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadAreas(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadRiverPoints(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        int length = sim.RiverPointsConst.Table.Length;
        sim.RiverPoints = new DatabaseStatic<RiverPoint, RiverPointColumns>(allocator, length);
        ref var columns = ref sim.RiverPoints.Table.Columns;

        BinaryReadUtility.ReadArraySimpleOfSerializables(fileStream, length, allocator, columns.Data, -1, &RiverPointData.Deserialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadNodes(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        int length = sim.NodesConst.Table.Length;
        sim.Nodes = new DatabaseStatic<Node, NodeColumns>(allocator, length);
        ref var columns = ref sim.Nodes.Table.Columns;

        string debug = "";

        for (int i = 0; i < length; i++)
        {
            int groundUnitsCapacity = fileStream.ReadValue<int>();

            if (i % 100_000 == 0)
            {
                debug += $"{groundUnitsCapacity} ";
            }

            groundUnitsCapacity = math.max(groundUnitsCapacity, NODES_GROUND_UNITS_CAPACITY);

            columns.GroundUnitsIds[i] = new RawSetWrapper32<DatabaseId>(allocator, groundUnitsCapacity);
        }

        //Debug.Log(debug);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void RecreateNodes(ref Sim sim)
    {
        //int lengthGroundUnits = sim.GroundUnits.Table.Count;
        //ref var columns = ref sim.Nodes.Table.Columns;

        //for (int i = 0; i < lengthGroundUnits; i++)
        //{
        //    ref readonly uint nodeIndex = ref sim.GroundUnits.Table.Columns.NodeIndex[i];
        //    ref readonly var id = ref sim.GroundUnits.Table.Columns.Id[i];

        //    columns.GroundUnitsIds[nodeIndex].Set.Add(id);
        //}
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadEdges(ref Sim sim, in FileStream fileStream)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadEntities(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        int capacityId = fileStream.ReadValue<int>();
        int countTable = fileStream.ReadValue<int>();

        sim.Entities = new DatabaseLookup<Entity, EntityColumns, EntityRelations>(allocator, capacityId, countTable, EntityRelations.Default);

        BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, sim.Entities.IdData.IdToIndex);
        BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, sim.Entities.IdData.IdToUseCount);
        BinaryReadUtility.ReadArraySimple(in fileStream, countTable, sim.Entities.Table.IndexToId);

        sim.Entities.IdData.RecreateIdStack();

        int lookupMatrixLength = DatabaseLookupMatrixUtility.GetLookupMatrixLength(capacityId);
        BinaryReadUtility.ReadArraySimple(in fileStream, lookupMatrixLength, sim.Entities.IdData.LookupMatrix);

        ref var columns = ref sim.Entities.Table.Columns;

        BinaryReadUtility.ReadArraySimple(in fileStream, countTable, columns.MapColor);
        BinaryReadUtility.ReadArraySimple(in fileStream, countTable, columns.DisplayData);
        BinaryReadUtility.ReadArraySimple(in fileStream, countTable, columns.Id);
        BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, countTable, allocator, columns.FamilyIds, 4, &EntityFamilyIds.Deserialize);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void RecreateEntities(ref Sim sim)
    {
        int length = sim.Entities.Table.Count;
        ref var columns = ref sim.Entities.Table.Columns;

        for (int i = 0; i < length; i++)
        {
            columns.FamilyIds[i].ChildrenIds.Clear();
        }

        for (int i = 0; i < length; i++)
        {
            ref readonly var id = ref columns.Id[i];
            ref readonly var parentId = ref columns.FamilyIds[i].ParentId;

            if (parentId.IsInvalid)
                continue;

            if (!sim.Entities.TryMapIdToIndex(parentId, out int parentIndex))
                throw new Exception($"SimLoadDynamicUtility :: RecreateEntities :: Entity with given id ({parentId.Value}) does not exist!");

            columns.FamilyIds[parentIndex].ChildrenIds.Add(id);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadPops(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        int capacityId = fileStream.ReadValue<int>();
        int countTable = fileStream.ReadValue<int>();

        sim.Pops = new Database<Pop, PopColumns>(allocator, capacityId, countTable);

        BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, sim.Pops.IdData.IdToIndex);
        BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, sim.Pops.IdData.IdToUseCount);
        BinaryReadUtility.ReadArraySimple(in fileStream, countTable, sim.Pops.Table.IndexToId);

        sim.Pops.IdData.RecreateIdStack();

        ref var columns = ref sim.Pops.Table.Columns;

        BinaryReadUtility.ReadArraySimple(fileStream, countTable, columns.FieldIndex);
        BinaryReadUtility.ReadArraySimple(fileStream, countTable, columns.Amount);
        BinaryReadUtility.ReadArraySimple(fileStream, countTable, columns.Class);
        BinaryReadUtility.ReadArraySimple(fileStream, countTable, columns.Demographics);
        BinaryReadUtility.ReadArraySimpleOfRawSets(fileStream, countTable, allocator, columns.Nationalities, 4);
        BinaryReadUtility.ReadArraySimpleOfRawSets(fileStream, countTable, allocator, columns.Religions, 4);
        BinaryReadUtility.ReadArraySimpleOfRawSets(fileStream, countTable, allocator, columns.WorkplaceEmploymentsIds, 4);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadWorkplaces(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        //BinaryReadUtility.ReadDatabaseLengths(fileStream, out int idsLength, out int length);
        //BinaryReadUtility.ReadDatabaseArrays(fileStream, idsLength, length, allocator, out var idToIndex, out var idToUseCount, out var indexToId);

        //var idDataSerializationData = new DatabaseIdDataSerializationData(idsLength, idToIndex, idToUseCount, allocator);
        //var tableSerializationData = new DatabaseTableSerializationData(length, indexToId, allocator);

        //sim.Workplaces = new Database2<Workplace, WorkplaceColumns>(idDataSerializationData, tableSerializationData);
        //ref var workplacesColumns = ref sim.Workplaces.Table.Columns;

        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplacesColumns.Type);
        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplacesColumns.Level);
        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplacesColumns.FieldIndex);
        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplacesColumns.Owner);
        //BinaryReadUtility.ReadArraySimpleOfRawSets(fileStream, length, allocator, workplacesColumns.EmploymentsIds, true);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadWorkplaceEmps(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        //BinaryReadUtility.ReadDatabaseLengths(fileStream, out int idsLength, out int length);
        //BinaryReadUtility.ReadDatabaseArrays(fileStream, idsLength, length, allocator, out var idToIndex, out var idToUseCount, out var indexToId);

        //var idDataSerializationData = new DatabaseIdDataSerializationData(idsLength, idToIndex, idToUseCount, allocator);
        //var tableSerializationData = new DatabaseTableSerializationData(length, indexToId, allocator);

        //sim.WorkplaceEmployments = new Database2<WorkplaceEmployment, WorkplaceEmploymentColumns>(idDataSerializationData, tableSerializationData);
        //ref var workplaceEmpsColumns = ref sim.WorkplaceEmployments.Table.Columns;

        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplaceEmpsColumns.Id);
        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplaceEmpsColumns.WorkplaceId);
        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplaceEmpsColumns.PopId);
        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplaceEmpsColumns.PopAmount);
        //BinaryReadUtility.ReadArraySimple(fileStream, length, workplaceEmpsColumns.WagePerPop);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void LoadGroundUnits(ref Sim sim, in FileStream fileStream, Allocator allocator)
    {
        int capacityId = fileStream.ReadValue<int>();
        var countsTables = stackalloc int[GroundUnitFlags.TABLES_COUNT_TOTAL];
        BinaryReadUtility.ReadArraySimple(in fileStream, GroundUnitFlags.TABLES_COUNT_TOTAL, countsTables);

        sim.GroundUnits = new DatabaseMul<GroundUnit, GroundUnitColumns>(allocator, capacityId, GroundUnitFlags.TABLES_COUNT_TOTAL, countsTables);

        BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, sim.GroundUnits.IdData.IdToIndex);
        BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, sim.GroundUnits.IdData.IdToUseCount);
        DatabaseSerializeUtility.ReadDatabaseMulTables(in fileStream, in sim.GroundUnits.Tables, GroundUnitFlags.TABLES_COUNT_TOTAL);

        sim.GroundUnits.IdData.RecreateIdStack();

        for (int i = 0; i < GroundUnitFlags.TABLES_COUNT_TOTAL; i++)
        {
            ref var table = ref sim.GroundUnits.MapTableIndexToTable(i);

            int countTable = table.Count;
            ref var columns = ref table.Columns;

            BinaryReadUtility.ReadArraySimple(fileStream, countTable, columns.Id);
            BinaryReadUtility.ReadArraySimple(fileStream, countTable, columns.EntityId);
            BinaryReadUtility.ReadArraySimple(fileStream, countTable, columns.NodeIndex);
            BinaryReadUtility.ReadArraySimpleOfSerializables(in fileStream, countTable, allocator, columns.Movement, GroundUnitMovement.PATH_EDGES_INDEXES_CAPACITY_INITIAL, &GroundUnitMovement.Deserialize);

            for (int j = 0; j < countTable; j++)
            {
                columns.UnitsIdsAttacking[j] = new RawSet<DatabaseId>(allocator, GroundUnit.UNITS_IDS_ATTACK_CAPACITY_INITIAL);
                columns.UnitsIdsAttackedBy[j] = new RawSet<DatabaseId>(allocator, GroundUnit.UNITS_IDS_ATTACK_CAPACITY_INITIAL);
            }
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void RecreateGroundUnits(ref Sim sim)
    {
        for (int i = 0; i < sim.GroundUnits.TablesAmount; i++)
        {
            ref var table = ref sim.GroundUnits.MapTableIndexToTable(i);

            ref var columns = ref table.Columns;
            int length = table.Count;

            ref readonly var columnsEdgesConst = ref sim.EdgesConst.Table.Columns;

            for (int j = 0; j < length; j++)
            {
                columns.NodeIndexOld[j] = columns.NodeIndex[j];

                ref var movement = ref columns.Movement[j];
                ref readonly var edgeData = ref columnsEdgesConst.Data[columns.NodeIndex[j]];

                // TODO: recalculate this somehow
                movement.DistanceChangePerHour = 1.0;

                if (movement.PathEdgesIndexes.Count == 0)
                {
                    movement.NodeIndexNext = -1;
                    movement.DistanceToNext = 0.0;
                }
                else
                {
                    movement.NodeIndexNext = (int)EdgeUtility.GetNodeIndexOther(edgeData.NodesIndexes, columns.NodeIndex[j]);
                    movement.DistanceToNext = edgeData.DistanceGround - movement.DistanceToNextTravelled;
                }
            }
        }
    }
}
