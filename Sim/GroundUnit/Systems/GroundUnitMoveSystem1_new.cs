using Ces.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

/// <summary>
/// Finds units attacked by this unit <br></br>
/// Works on moving units
/// </summary>
[BurstCompile]
public unsafe struct GroundUnitMoveSystem1_new : IJobParallelForBatch
{
    public RawPtr<Sim> Sim;

    [BurstCompile]
    public void Execute(int startIndex, int count)
    {
        ref readonly var database_groundUnits = ref Sim.Ref.GroundUnits;
        ref readonly var table_groundUnits = ref database_groundUnits.MapTableIndexToTable(GroundUnitFlags.TABLE_MOVING);
        int table_groundUnits_count = table_groundUnits.Count;

        ref readonly var database_entities = ref Sim.Ref.Entities;

        ref readonly var database_nodes = ref Sim.Ref.Nodes;
        int table_nodes_count = Sim.Ref.Nodes.Table.Length;

        // -----

        var column_groundUnit_id = new RawSpanReadonly<DatabaseId>(table_groundUnits.Columns.Id, table_groundUnits_count);
        var column_groundUnit_entityId = new RawSpanReadonly<DatabaseId>(table_groundUnits.Columns.EntityId, table_groundUnits_count);
        var column_groundUnit_nodeIndex = new RawSpan<uint>(table_groundUnits.Columns.NodeIndex, table_groundUnits_count);
        var column_groundUnit_movement = new RawSpan<GroundUnitMovement>(table_groundUnits.Columns.Movement, table_groundUnits_count);
        var column_groundUnit_unitsIdsAttacking = new RawSpan<RawSet<DatabaseId>>(table_groundUnits.Columns.UnitsIdsAttacking, table_groundUnits_count);

        var column_node_groundUnitsIds = new RawSpanReadonly<RawSetWrapper32<DatabaseId>>(database_nodes.Table.Columns.GroundUnitsIds, table_nodes_count);

        // -----

        for (int groundUnitIndex = startIndex; groundUnitIndex < startIndex + count; groundUnitIndex++)
        {
            ref readonly var this_id = ref column_groundUnit_id[groundUnitIndex];
            ref readonly var this_entityId = ref column_groundUnit_entityId[groundUnitIndex];
            ref uint this_nodeIndex = ref column_groundUnit_nodeIndex[groundUnitIndex];
            ref var this_movement = ref column_groundUnit_movement[groundUnitIndex];
            ref var this_unitsIdsAttacking = ref column_groundUnit_unitsIdsAttacking[groundUnitIndex];

            ref readonly int this_entityIndex = ref database_entities.MapIdToIndex(this_entityId).Index;

            ref readonly var this_movement_nodeNext_groundUnitsIds = ref column_node_groundUnitsIds[this_movement.NodeIndexNext].Set;

            // attacking ids must be cleared at this point

            for (int i = 0; i < this_movement_nodeNext_groundUnitsIds.Count; i++)
            {
                ref readonly var otherId = ref this_movement_nodeNext_groundUnitsIds[i];
                ref readonly int otherIndex = ref database_groundUnits.MapIdToIndex(otherId).Index;
                ref readonly var otherEntityId = ref column_groundUnit_entityId[otherIndex];

                ref readonly var entitiesRelationsFlags = ref database_entities.MapIdsToLookupValue(this_entityId, otherEntityId).Flags;

                // attack a unit if entities always fight

                if (FlagsUtility.Contains(entitiesRelationsFlags, EntityRelationsFlags.ALWAYS_FIGHT))
                {
                    this_unitsIdsAttacking.Add(otherId);
                }
            }
        }
    }
}
