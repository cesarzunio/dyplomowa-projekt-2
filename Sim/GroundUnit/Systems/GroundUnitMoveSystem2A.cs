//using Ces.Collections;
//using Unity.Jobs;
//using Unity.Burst;
//using Unity.Mathematics;
//using System.Runtime.CompilerServices;

//[BurstCompile]
//public unsafe struct GroundUnitMoveSystem2A : IJob
//{
//    public RawPtr<Sim> Sim;

//    [BurstCompile]
//    public void Execute()
//    {
//        ref readonly var groundUnits = ref Sim.Ref.GroundUnits;
//        ref readonly int groundUnitsLength = ref groundUnits.Table.Count;

//        // -----

//        var groundUnitId = new RawSpanReadonly<DatabaseId>(groundUnits.Table.Columns.Id, groundUnitsLength);
//        var groundUnitUnitsIdsAttacking = new RawSpanReadonly<RawSet<DatabaseId>>(groundUnits.Table.Columns.UnitsIdsAttacking, groundUnitsLength);
//        var groundUnitUnitsIdsAttackedBy = new RawSpan<RawSet<DatabaseId>>(groundUnits.Table.Columns.UnitsIdsAttackedBy, groundUnitsLength);

//        // -----

//        for (int groundUnitIndex = 0; groundUnitIndex < groundUnitsLength; groundUnitIndex++)
//        {
//            ref readonly var thisId = ref groundUnitId[groundUnitIndex];
//            ref readonly var thisUnitsIdsAttacking = ref groundUnitUnitsIdsAttacking[groundUnitIndex];

//            // -----

//            for (int i = 0; i < thisUnitsIdsAttacking.Count; i++)
//            {
//                ref readonly var otherId = ref thisUnitsIdsAttacking[i];
//                ref readonly var otherIndex = ref groundUnits.MapIdToIndex(otherId).Index;
//                ref var otherUnitsIdsAttackedBy = ref groundUnitUnitsIdsAttackedBy[otherIndex];

//                otherUnitsIdsAttackedBy.Add(thisId);
//            }
//        }
//    }
//}
