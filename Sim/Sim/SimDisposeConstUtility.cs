using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ces.Collections;

public static unsafe class SimDisposeConstUtility
{
    public static void DisposeSim(ref Sim sim)
    {
        DisposeFieldsMap(ref sim);
        DisposeFields(ref sim);
        DisposeAreas(ref sim);
        DisposeRivers(ref sim);
        DisposeRiverPoints(ref sim);

        DisposeNodes(ref sim);
        DisposeNodeEdges(ref sim);
    }

    static void DisposeFieldsMap(ref Sim sim)
    {
        sim.FieldsMap.Dispose();
    }

    static void DisposeFields(ref Sim sim)
    {
        int length = sim.FieldsConst.Table.Length;
        ref var columns = ref sim.FieldsConst.Table.Columns;

        sim.FieldsConst.Dispose();
    }

    static void DisposeAreas(ref Sim sim)
    {
        int length = sim.AreasConst.Table.Length;
        ref var columns = ref sim.AreasConst.Table.Columns;

        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.Data, length);

        sim.AreasConst.Dispose();
    }

    static void DisposeRivers(ref Sim sim)
    {
        int length = sim.RiversConst.Table.Length;
        ref var columns = ref sim.RiversConst.Table.Columns;

        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.Data, length);

        sim.RiversConst.Dispose();
    }

    static void DisposeRiverPoints(ref Sim sim)
    {
        int length = sim.RiverPointsConst.Table.Length;
        ref var columns = ref sim.RiverPointsConst.Table.Columns;

        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.Data, length);

        sim.RiverPointsConst.Dispose();
    }

    static void DisposeNodes(ref Sim sim)
    {
        int length = sim.NodesConst.Table.Length;
        ref var columns = ref sim.NodesConst.Table.Columns;

        CesCollectionsUtility.DisposeArraySimpleOfDisposables(columns.Data, length);

        sim.NodesConst.Dispose();
    }

    static void DisposeNodeEdges(ref Sim sim)
    {
        sim.EdgesConst.Dispose();
    }
}
