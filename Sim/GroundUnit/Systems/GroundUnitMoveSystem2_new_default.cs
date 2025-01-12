//using Ces.Collections;
//using Unity.Jobs;
//using Unity.Burst;
//using Unity.Mathematics;
//using System.Runtime.CompilerServices;

///// <summary>
///// Syncs attack ids, from attacked to defender <br></br>
///// Works on any table, specified by 'GroundUnitsTableIndex'
///// </summary>
//[BurstCompile]
//public unsafe struct GroundUnitMoveSystem2_new_default : IJob
//{
//    public RawPtr<Sim> Sim;
//    public readonly int GroundUnitsTableIndex;

//    [BurstCompile]
//    public void Execute()
//    {
//        ref readonly var groundUnits = ref Sim.Ref.GroundUnits;
//        ref readonly var groundUnitsTableDefault = ref groundUnits.MapTableIndexToTable(GroundUnitsTableIndex);
//        int groundUnitsLength = groundUnitsTableDefault.Count;

//        ref readonly var entities = ref Sim.Ref.Entities;

//        ref readonly var nodes = ref Sim.Ref.Nodes;
//        int nodesLength = Sim.Ref.Nodes.Table.Length;

//        // -----

//        var groundUnitId = new RawSpanReadonly<DatabaseId>(groundUnitsTableDefault.Columns.Id, groundUnitsLength);
//        var groundUnitEntityId = new RawSpanReadonly<DatabaseId>(groundUnitsTableDefault.Columns.EntityId, groundUnitsLength);
//        var groundUnitNodeIndex = new RawSpan<uint>(groundUnitsTableDefault.Columns.NodeIndex, groundUnitsLength);
//        var groundUnitMovement = new RawSpan<GroundUnitMovement>(groundUnitsTableDefault.Columns.Movement, groundUnitsLength);
//        var groundUnitUnitsIdsAttacking = new RawSpan<RawSet<DatabaseId>>(groundUnitsTableDefault.Columns.UnitsIdsAttacking, groundUnitsLength);

//        var nodeIds = new RawSpanReadonly<RawSetWrapper32<DatabaseId>>(nodes.Table.Columns.GroundUnitsIds, nodesLength);

//        // -----

//        for (int groundUnitIndex = 0; groundUnitIndex < groundUnitsLength; groundUnitIndex++)
//        {
//            ref readonly var thisId = ref groundUnitId[groundUnitIndex];
//            ref readonly var thisUnitsIdsAttacking = ref groundUnitUnitsIdsAttacking[groundUnitIndex];

//            for (int i = 0; i < thisUnitsIdsAttacking.Count; i++)
//            {
//                ref readonly var otherIndex = ref groundUnits.MapIdToIndex(thisUnitsIdsAttacking[i]);
//                ref readonly var otherUnitTable = ref groundUnits.MapTableIndexToTable(otherIndex.TableIndex);
//                ref var otherUnitsIdsAttackedBy = ref otherUnitTable.Columns.UnitsIdsAttackedBy[otherIndex.Index];

//                otherUnitsIdsAttackedBy.Add(thisId);
//            }
//        }
//    }
//}
