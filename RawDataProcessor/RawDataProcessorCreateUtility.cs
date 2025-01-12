using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using Ces.Collections;
using System.Runtime.CompilerServices;

public static unsafe class RawDataProcessorCreateUtility
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillFieldsMap(ref Sim sim, RawDataProcessorLoadUtility.FieldsMap fieldsMap, Allocator allocator)
    {
        long fieldsMapLength = fieldsMap.TextureSize.x * fieldsMap.TextureSize.y;
        long sizeT = UnsafeUtility.SizeOf<uint>() * fieldsMapLength;

        var fields = CesMemoryUtility.Allocate<uint>(fieldsMapLength, allocator);
        UnsafeUtility.MemCpy(fields, fieldsMap.Fields, sizeT);

        sim.FieldsMap = new FieldsMap(allocator, fieldsMap.TextureSize, fields);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillFields(
        ref Sim sim,
        RawDataProcessorLoadUtility.Fields fields,
        RawArray<uint> fieldsNodesIndexes,
        RawArray<float> fieldsElevations,
        RawArray<RawDataProcessorLoadUtility.LandForm> fieldsLandForms,
        RawArray<RawDataProcessorLoadUtility.SoilType> fieldsSoilTypes,
        RawArray<float> fieldsSurfaces,
        RawArray<RawDataProcessorLoadUtility.FieldLandCoverParams> fieldsLandCoverParams,
        RawArray<RawDataProcessorLoadUtility.FieldWeathers> fieldsTemperatures,
        RawArray<RawDataProcessorLoadUtility.FieldWeathers> fieldsRainfalls,
        Allocator allocator)
    {
        if (fields.Length != fieldsNodesIndexes.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsNodesIndexes: {fieldsNodesIndexes.Length}");

        if (fields.Length != fieldsElevations.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsElevations: {fieldsElevations.Length}");

        if (fields.Length != fieldsLandForms.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsLandForms: {fieldsLandForms.Length}");

        if (fields.Length != fieldsSoilTypes.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsSoilTypes: {fieldsSoilTypes.Length}");

        if (fields.Length != fieldsSurfaces.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsSurfaces: {fieldsSurfaces.Length}");

        if (fields.Length != fieldsLandCoverParams.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsLandCoverParams: {fieldsLandCoverParams.Length}");

        if (fields.Length != fieldsTemperatures.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsTemperatures: {fieldsTemperatures.Length}");

        if (fields.Length != fieldsRainfalls.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: fields: {fields.Length}, fieldsRainfalls: {fieldsRainfalls.Length}");

        sim.FieldsConst = new DatabaseStatic<FieldConst, FieldConstColumns>(allocator, fields.Length);
        sim.Fields = new DatabaseStatic<Field, FieldColumns>(allocator, fields.Length);

        for (int i = 0; i < fields.Length; i++)
        {
            var fieldConst = new FieldConst
            {
                AreaIndex = default,
                NodeIndex = fieldsNodesIndexes[i],

                Soil = GetFieldSoil(fieldsSoilTypes[i]),
                LandForm = GetFieldLandForm(fieldsLandForms[i]),
                Elevation = fieldsElevations[i],
                Surface = fieldsSurfaces[i],

                Temperature0 = fieldsTemperatures[i].Weather0,
                Temperature1 = fieldsTemperatures[i].Weather1,
                Temperature2 = fieldsTemperatures[i].Weather2,
                Temperature3 = fieldsTemperatures[i].Weather3,
                Temperature4 = fieldsTemperatures[i].Weather4,
                Temperature5 = fieldsTemperatures[i].Weather5,
                Temperature6 = fieldsTemperatures[i].Weather6,
                Temperature7 = fieldsTemperatures[i].Weather7,
                Temperature8 = fieldsTemperatures[i].Weather8,
                Temperature9 = fieldsTemperatures[i].Weather9,
                Temperature10 = fieldsTemperatures[i].Weather10,
                Temperature11 = fieldsTemperatures[i].Weather11,

                Rainfall0 = fieldsRainfalls[i].Weather0,
                Rainfall1 = fieldsRainfalls[i].Weather1,
                Rainfall2 = fieldsRainfalls[i].Weather2,
                Rainfall3 = fieldsRainfalls[i].Weather3,
                Rainfall4 = fieldsRainfalls[i].Weather4,
                Rainfall5 = fieldsRainfalls[i].Weather5,
                Rainfall6 = fieldsRainfalls[i].Weather6,
                Rainfall7 = fieldsRainfalls[i].Weather7,
                Rainfall8 = fieldsRainfalls[i].Weather8,
                Rainfall9 = fieldsRainfalls[i].Weather9,
                Rainfall10 = fieldsRainfalls[i].Weather10,
                Rainfall11 = fieldsRainfalls[i].Weather11,
            };

            var field = new Field
            {
                EntityId = DatabaseId.Invalid,

                LandCover = FieldLandCover.None,
                LandCoverParams = GetFieldLandCoverParams(fieldsLandCoverParams[i]),
                WaterLevel = fields.IsLand[i] ? 0f : 1f,

                PopsIds = new RawSet<DatabaseId>(allocator),
            };

            float temperatureSum =
                fieldConst.Temperature0 +
                fieldConst.Temperature1 +
                fieldConst.Temperature2 +
                fieldConst.Temperature3 +
                fieldConst.Temperature4 +
                fieldConst.Temperature5 +
                fieldConst.Temperature6 +
                fieldConst.Temperature7 +
                fieldConst.Temperature8 +
                fieldConst.Temperature9 +
                fieldConst.Temperature10 +
                fieldConst.Temperature11;

            field.LandCoverParams.Temperature = temperatureSum / 12;

            sim.FieldsConst.Set(i, fieldConst);
            sim.Fields.Set(i, field);
        }

        // ---

        static FieldSoil GetFieldSoil(RawDataProcessorLoadUtility.SoilType soilType) => (FieldSoil)(byte)soilType;

        static FieldLandForm GetFieldLandForm(RawDataProcessorLoadUtility.LandForm landForm) => landForm switch
        {
            RawDataProcessorLoadUtility.LandForm.None => FieldLandForm.None,
            _ => (FieldLandForm)((byte)landForm - 1) // original shifted down by one
        };

        static FieldLandCoverParams GetFieldLandCoverParams(RawDataProcessorLoadUtility.FieldLandCoverParams fieldsLandCoverParams) => new()
        {
            Wetness = fieldsLandCoverParams.Wetness,
            Vegetation = fieldsLandCoverParams.Vegetation,
            Cultivation = fieldsLandCoverParams.Cultivation,
            Desertification = fieldsLandCoverParams.Desertification,
            Glaciation = fieldsLandCoverParams.Glaciation,
            Buildings = fieldsLandCoverParams.Buildings,
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillAreas(ref Sim sim, RawArray<RawArray<uint>> areas, Allocator allocator)
    {
        sim.AreasConst = new DatabaseStatic<AreaConst, AreaConstColumns>(allocator, areas.Length);

        ref var fieldAreaIndex = ref sim.FieldsConst.Table.Columns.AreaIndex;

        for (int i = 0; i < areas.Length; i++)
        {
            sim.AreasConst.Set(i, new AreaConst
            {
                Data = new AreaConstData
                {
                    FieldsIndexes = areas[i].ShallowCopy(allocator),
                }
            });

            for (int j = 0; j < areas[i].Length; j++)
            {
                fieldAreaIndex[areas[i][j]] = (uint)i;
            }
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillRivers(
        ref Sim sim, RawArray<RawDataProcessorLoadUtility.River> rivers, RawArray<RawDataProcessorLoadUtility.RiverPoint> riverPoints,
        RawArray<RawArray<uint>> riverPointsCatchments, Allocator allocator)
    {
        if (riverPoints.Length != riverPointsCatchments.Length)
            throw new Exception($"RawDataCreateUtility :: FillFields :: riverPoints: {riverPoints.Length}, riverPointsCatchments: {riverPointsCatchments.Length}");

        sim.RiversConst = new DatabaseStatic<RiverConst, RiverConstColumns>(allocator, rivers.Length);

        for (int i = 0; i < rivers.Length; i++)
        {
            sim.RiversConst.Set(i, new RiverConst
            {
                Data = new RiverConstData
                {
                    PointsIndexes = rivers[i].RiverPointsIndexes.ShallowCopy(allocator),
                }
            });
        }

        sim.RiverPointsConst = new DatabaseStatic<RiverPointConst, RiverPointConstColumns>(allocator, riverPoints.Length);
        sim.RiverPoints = new DatabaseStatic<RiverPoint, RiverPointColumns>(allocator, riverPoints.Length);

        for (int i = 0; i < riverPoints.Length; i++)
        {
            sim.RiverPointsConst.Set(i, new RiverPointConst
            {
                Data = new RiverPointConstData
                {
                    RiverIndex = riverPoints[i].RiverIndex,
                    NodeIndex = riverPoints[i].NodeIndex,
                    StartsFromFieldIndex = riverPoints[i].StartsFromFieldIndex,

                    ConnectionsFrom = new RiverPointConstConnections(riverPoints[i].StartsFrom.Length, riverPoints[i].StartsFrom.Data),
                    ConnectionsTo = new RiverPointConstConnections(riverPoints[i].EndsInto.Length, riverPoints[i].EndsInto.Data),

                    CatchmentFieldsIndexes = riverPointsCatchments[i].ShallowCopy(allocator),
                },
            });

            sim.RiverPoints.Set(i, new RiverPoint
            {
                Data = new RiverPointData
                {
                    WaterAmount = 1f,
                    WaterAmountSource = 0f,
                },
            });
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillNodes(ref Sim sim, RawDataProcessorLoadUtility.NodesFinals nodes, Allocator allocator)
    {
        sim.NodesConst = new DatabaseStatic<NodeConst, NodeConstColumns>(allocator, nodes.Length);
        sim.Nodes = new DatabaseStatic<Node, NodeColumns>(allocator, nodes.Length);

        for (int i = 0; i < nodes.Length; i++)
        {
            sim.NodesConst.Set(i, new NodeConst
            {
                CenterUnitSphere = math.normalize(GeoUtilitiesDouble.GeoCoordToUnitSphere(nodes.GeoCoord[i])),
                Data = new NodeConstData
                {
                    EdgesIndexes = nodes.EdgesIndexes[i].ShallowCopy(allocator),
                    Owner = GetNodeOwner(nodes.Owner[i]),
                },
            });

            sim.Nodes.Set(i, new Node
            {
                Enabled = true,
                GroundUnitsIds = RawSetWrapper32<DatabaseId>.Null,
            });
        }

        // ---

        static OwnerIndex<NodeConstOwnerType> GetNodeOwner(RawDataProcessorLoadUtility.NodeOwner nodeOwner) => *(OwnerIndex<NodeConstOwnerType>*)&nodeOwner;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillEdges(ref Sim sim, RawDataProcessorLoadUtility.EdgesFinals edges, Allocator allocator)
    {
        sim.EdgesConst = new DatabaseStatic<EdgeConst, EdgeColumns>(allocator, edges.Length);

        for (int i = 0; i < edges.Length; i++)
        {
            sim.EdgesConst.Set(i, new EdgeConst
            {
                Data = new EdgeConstData
                {
                    NodesIndexes = edges.NodesIndexes[i],
                    DistanceGround = edges.DistanceGround[i],
                    DistanceAir = edges.DistanceAir[i],
                    CrossedRiverPointIndex = edges.CrossedRiverPointIndex[i],
                }
            });
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillEntities(ref Sim sim, RawArray<RawArray<uint>> entities, Allocator allocator)
    {
        //sim.Entities = new Database<Entity, EntityColumns>(allocator, entities.Length);
        sim.Entities = new DatabaseLookup<Entity, EntityColumns, EntityRelations>(allocator, entities.Length, EntityRelations.Default);
        //sim.Entities = new DatabaseLookup<Entity, EntityColumns, EntityRelations>(allocator, entities.Length, entities.Length, EntityRelations.Default);

        for (int i = 0; i < entities.Length; i++)
        {
            var entityIdIndexPair = sim.Entities.AddAndCreateId(new Entity
            {
                MapColor = new Color32(128, 0, 128, 255),
                DisplayData = new EntityDisplayData
                {
                    NameFull = $"Name_full_default_{i}",
                    NameShort = $"NULL_{i}",

                    MapColor = new Color32(128, 0, 128, 255),
                }
            });

            sim.Entities.Table.Columns.Id[entityIdIndexPair.Index] = entityIdIndexPair.Id;

            sim.Entities.Table.Columns.FamilyIds[entityIdIndexPair.Index] = new EntityFamilyIds
            {
                ParentId = DatabaseId.Invalid,
                ChildrenIds = RawSet<DatabaseId>.Null,
            };

            for (int j = 0; j < entities[i].Length; j++)
            {
                sim.Fields.Table.Columns.EntityId[entities[i][j]] = entityIdIndexPair.Id;
            }
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillEntitiesManageds(in Sim sim, in SimManaged simManaged)
    {
        simManaged.Entities = new EntityManaged[sim.Entities.IdData.Capacity];

        for (int i = 0; i < sim.Entities.IdData.Capacity; i++)
        {
            var id = new DatabaseId(i);
            simManaged.Entities[i].Enabled = sim.Entities.TryMapIdToIndex(id, out _);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void FillPops(ref Sim sim, RawArray<float> fieldsPops, Allocator allocator)
    {
        sim.Pops = new Database<Pop, PopColumns>(allocator, fieldsPops.Length);

        ref var fieldPopsIds = ref sim.Fields.Table.Columns.PopsIds;

        for (int i = 0; i < fieldsPops.Length; i++)
        {
            if (fieldsPops[i] == 0f)
                continue;

            var popIdIndexPair = sim.Pops.AddAndCreateId(new Pop
            {
                FieldIndex = (uint)i,
                Amount = fieldsPops[i],
                Class = PopClass.NONE,

                Demographics = default,
                Nationalities = RawSet<DatabaseId>.Null,
                Religions = RawSet<DatabaseId>.Null,

                WorkplaceEmploymentsIds = RawSet<DatabaseId>.Null,
            });

            fieldPopsIds[i].Add(popIdIndexPair.Id);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void InitializeOthers(ref Sim sim, Allocator allocator)
    {
        InitializeWorkplaces(ref sim, allocator);
        InitializeGroundUnits(ref sim, allocator);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void InitializeWorkplaces(ref Sim sim, Allocator allocator)
    {
        //sim.Workplaces = new Database2<Workplace, WorkplaceColumns>(allocator, 0);
        //sim.WorkplaceEmployments = new Database2<WorkplaceEmployment, WorkplaceEmploymentColumns>(allocator, 0);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void InitializeGroundUnits(ref Sim sim, Allocator allocator)
    {
        sim.GroundUnits = new DatabaseMul<GroundUnit, GroundUnitColumns>(allocator, 0, GroundUnitFlags.TABLES_COUNT_TOTAL);
    }
}
