using Ces.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static unsafe class EntityUtility
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool IsDeepChild(in Sim sim, DatabaseId childToCheck, DatabaseId parentToCheck)
    {
        int childIndex = sim.Entities.MapIdToIndex(childToCheck).Index;
        var parentId = sim.Entities.Table.Columns.FamilyIds[childIndex].ParentId;

        if (parentId.IsInvalid)
            return false;

        return parentId == parentToCheck || IsDeepChild(in sim, parentId, parentToCheck);
    }
}