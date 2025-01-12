using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ces.Collections;

public static unsafe class SimDisposeDynamicUtility
{
    public static void DisposeSim(ref Sim sim)
    {
        DisposeFields(ref sim);
        DisposeRiverPoints(ref sim);

        DisposeNodes(ref sim);

        DisposeEntities(ref sim);
        DisposePops(ref sim);

        DisposeGroundUnits(ref sim);
    }

    static void DisposeFields(ref Sim sim)
    {
        ref var columns = ref sim.Fields.Table.Columns;
        int length = sim.Fields.Table.Length;

        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.PopsIds, length);

        sim.Fields.Dispose();
    }

    static void DisposeRiverPoints(ref Sim sim)
    {
        ref var columns = ref sim.RiverPoints.Table.Columns;
        int length = sim.RiverPoints.Table.Length;

        sim.RiverPoints.Dispose();
    }

    static void DisposeNodes(ref Sim sim)
    {
        ref var columns = ref sim.Nodes.Table.Columns;
        int length = sim.Nodes.Table.Length;

        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.GroundUnitsIds, length);

        sim.Nodes.Dispose();
    }

    static void DisposeEntities(ref Sim sim)
    {
        int length = sim.Entities.Table.Count;
        ref var columns = ref sim.Entities.Table.Columns;

        sim.Entities.Dispose();
    }

    static void DisposePops(ref Sim sim)
    {
        ref var columns = ref sim.Pops.Table.Columns;
        int length = sim.Pops.Table.Count;

        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.Nationalities, length);
        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.Religions, length);
        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.WorkplaceEmploymentsIds, length);

        sim.Pops.Dispose();
    }

    static void DisposeGroundUnits(ref Sim sim)
    {
        for (int i = 0; i < sim.GroundUnits.TablesAmount; i++)
        {
            ref var table = ref sim.GroundUnits.MapTableIndexToTable(i);

            int countTable = table.Count;
            ref var columns = ref table.Columns;

            CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.UnitsIdsAttacking, countTable);
            CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.UnitsIdsAttackedBy, countTable);
            CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.Movement, countTable);
        }

        sim.GroundUnits.Dispose();
    }
}
