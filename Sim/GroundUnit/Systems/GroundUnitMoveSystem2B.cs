//using Ces.Collections;
//using Unity.Jobs;
//using Unity.Burst;
//using Unity.Mathematics;
//using System.Runtime.CompilerServices;

///// <summary>
///// Single <br></br>
///// Updates unit node indexes, removes unit id from old set, adds unit id to new set <br></br>
///// </summary>
//[BurstCompile]
//public unsafe struct GroundUnitMoveSystem2B : IJob
//{
//    public RawPtr<Sim> Sim;

//    [BurstCompile]
//    public void Execute()
//    {
//        ref readonly var groundUnits = ref Sim.Ref.GroundUnits;
//        ref readonly int groundUnitsLength = ref groundUnits.Table.Count;

//        ref readonly var nodes = ref Sim.Ref.Nodes;
//        ref readonly int nodesLength = ref Sim.Ref.Nodes.Table.Length;

//        // -----

//        var groundUnitId = new RawSpanReadonly<DatabaseId>(groundUnits.Table.Columns.Id, groundUnitsLength);
//        var groundUnitNodeIndex = new RawSpanReadonly<uint>(groundUnits.Table.Columns.NodeIndex, groundUnitsLength);
//        var groundUnitNodeIndexOld = new RawSpan<uint>(groundUnits.Table.Columns.NodeIndexOld, groundUnitsLength);

//        var nodeGroundUnitsIds = new RawSpan<RawSetWrapper32<DatabaseId>>(nodes.Table.Columns.GroundUnitsIds, nodesLength);

//        // -----

//        for (int groundUnitIndex = 0; groundUnitIndex < groundUnitsLength; groundUnitIndex++)
//        {
//            ref readonly var thisId = ref groundUnitId[groundUnitIndex];
//            ref readonly uint thisNodeIndex = ref groundUnitNodeIndex[groundUnitIndex];
//            ref uint thisNodeIndexOld = ref groundUnitNodeIndexOld[groundUnitIndex];

//            if (thisNodeIndex == thisNodeIndexOld)
//                continue;

//            ref var groundUnitsIds = ref nodeGroundUnitsIds[thisNodeIndex].Set;
//            ref var groundUnitsIdsOld = ref nodeGroundUnitsIds[thisNodeIndexOld].Set;

//            groundUnitsIdsOld.Remove(thisId);
//            groundUnitsIds.Add(thisId);

//            thisNodeIndexOld = thisNodeIndex;
//        }
//    }
//}
