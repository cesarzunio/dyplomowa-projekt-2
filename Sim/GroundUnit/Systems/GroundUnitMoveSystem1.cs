//using Ces.Collections;
//using Unity.Jobs;
//using Unity.Burst;
//using Unity.Mathematics;
//using System.Runtime.CompilerServices;

//[BurstCompile]
//public unsafe struct GroundUnitMoveSystem1 : IJobParallelForBatch
//{
//    public RawPtr<Sim> Sim;

//    [BurstCompile]
//    public void Execute(int startIndex, int count)
//    {
//        ref readonly var groundUnits = ref Sim.Ref.GroundUnits;
//        ref readonly int groundUnitsLength = ref groundUnits.Table.Count;

//        ref readonly var entities = ref Sim.Ref.Entities;
//        ref readonly int entitiesLength = ref entities.Table.Count;

//        ref readonly var nodes = ref Sim.Ref.Nodes;
//        ref readonly int nodesLength = ref Sim.Ref.Nodes.Table.Length;

//        ref readonly var edges = ref Sim.Ref.EdgesConst;
//        ref readonly int edgesLength = ref edges.Table.Length;

//        ref readonly var fields = ref Sim.Ref.Fields;
//        ref readonly int fieldsLength = ref fields.Table.Length;

//        ref readonly var riverPoints = ref Sim.Ref.RiverPoints;
//        ref readonly int riverPointsLength = ref riverPoints.Table.Length;

//        // -----

//        var groundUnitId = new RawSpanReadonly<DatabaseId>(groundUnits.Table.Columns.Id, groundUnitsLength);
//        var groundUnitEntityId = new RawSpanReadonly<DatabaseId>(groundUnits.Table.Columns.EntityId, groundUnitsLength);
//        var groundUnitNodeIndex = new RawSpan<uint>(groundUnits.Table.Columns.NodeIndex, groundUnitsLength);
//        var groundUnitMovement = new RawSpan<GroundUnitMovement>(groundUnits.Table.Columns.Movement, groundUnitsLength);
//        var groundUnitUnitsIdsAttackedByThis = new RawSpan<RawSet<DatabaseId>>(groundUnits.Table.Columns.UnitsIdsAttacking, groundUnitsLength);
//        var groundUnitUnitsIdsAttackingThis = new RawSpan<RawSet<DatabaseId>>(groundUnits.Table.Columns.UnitsIdsAttackedBy, groundUnitsLength);

//        var entityIdsToAttack = new RawSpanReadonly<RawSetWrapper32<DatabaseId>>(entities.Table.Columns.IdsToAttack, entitiesLength);

//        var nodeIds = new RawSpanReadonly<RawSetWrapper32<DatabaseId>>(nodes.Table.Columns.GroundUnitsIds, nodesLength);

//        var edgeData = new RawSpanReadonly<EdgeConstData>(edges.Table.Columns.Data, edgesLength);

//        // -----

//        for (int groundUnitIndex = startIndex; groundUnitIndex < startIndex + count; groundUnitIndex++)
//        {
//            ref readonly var thisId = ref groundUnitId[groundUnitIndex];
//            ref readonly var thisEntityId = ref groundUnitEntityId[groundUnitIndex];
//            ref uint thisNodeIndex = ref groundUnitNodeIndex[groundUnitIndex];
//            ref var thisMovement = ref groundUnitMovement[groundUnitIndex];
//            ref var thisUnitsIdsAttackedByThis = ref groundUnitUnitsIdsAttackedByThis[groundUnitIndex];
//            ref var thisUnitsIdsAttackingThis = ref groundUnitUnitsIdsAttackingThis[groundUnitIndex];

//            ref readonly int thisEntityIndex = ref entities.MapIdToIndex(thisEntityId).Index;
//            ref readonly var thisEntityGroundUnitsIdsToAttack = ref entityIdsToAttack[thisEntityIndex].Set;

//            // -----

//            thisUnitsIdsAttackedByThis.Clear();
//            thisUnitsIdsAttackingThis.Clear();

//            if (thisMovement.NodeIndexNext == GroundUnitMovement.NODE_NULL) // not moving
//                continue;

//            ref readonly var nodeNextGroundUnitsIds = ref nodeIds[thisMovement.NodeIndexNext].Set;

//            for (int i = 0; i < nodeNextGroundUnitsIds.Count; i++)
//            {
//                ref readonly var otherId = ref nodeNextGroundUnitsIds[i];
//                ref readonly int otherIndex = ref groundUnits.MapIdToIndex(otherId).Index;
//                ref readonly var otherEntityId = ref groundUnitEntityId[otherIndex];

//                ref readonly var entitiesRelationsFlags = ref entities.MapIdsToLookupValue(thisEntityId, otherEntityId).Flags;

//                // attack a ground unit if entities always fight OR if this entity has the unit in its target set

//                // TODO co jesli ta jednostka idzie na entity które ma j¹ w celach ?? tzn ze jest atakowana ale sama nie atakuje, nie powinna móc siê ruszyæ

//                if (FlagsUtility.Contains(entitiesRelationsFlags, EntityRelationsFlags.ALWAYS_FIGHT) || thisEntityGroundUnitsIdsToAttack.Contains(otherId))
//                {
//                    thisUnitsIdsAttackedByThis.Add(otherId);
//                }
//            }

//            // TODO unit musi wiedziec czy jest atakowany zanim sie ruszy, bo go zwalnia czy cos ????
//            // trzeba to wyjebac do job'a po job'ie mapuj¹cym atakuj¹cych do broni¹cych

//            if (thisUnitsIdsAttackedByThis.Count > 0) // dont move if attacking any
//                continue;

//            thisMovement.DistanceToNextTravelled += thisMovement.DistanceChangePerHour;

//            double distanceToNextYet = thisMovement.DistanceToNext - thisMovement.DistanceToNextTravelled;

//            if (distanceToNextYet > 0.0) // destination not reached
//                continue;

//            thisNodeIndex = (uint)thisMovement.NodeIndexNext;

//            if (thisMovement.PathIndexCurrent >= thisMovement.PathEdgesIndexes.Count) // reached final destination
//            {
//                thisMovement.PathIndexCurrent = GroundUnitMovement.NODE_NULL;
//                thisMovement.NodeIndexNext = GroundUnitMovement.NODE_NULL;
//                thisMovement.DistanceToNextTravelled = 0.0;
//                continue;
//            }

//            ref readonly uint thisEdgeCurrentIndex = ref thisMovement.PathEdgesIndexes[thisMovement.PathIndexCurrent];
//            ref readonly var edgeCurrentNodesIndexes = ref edgeData[thisEdgeCurrentIndex].NodesIndexes;

//            thisMovement.NodeIndexNext = (int)EdgeUtility.GetNodeIndexOther(edgeCurrentNodesIndexes, thisNodeIndex);
//            thisMovement.DistanceToNext = edgeData[thisEdgeCurrentIndex].DistanceGround;
//            thisMovement.DistanceToNextTravelled = -distanceToNextYet;
//        }
//    }
//}
