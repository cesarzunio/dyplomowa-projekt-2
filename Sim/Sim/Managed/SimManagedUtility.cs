using Ces.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Ces.Collections
{
    public static class SimManagedUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateCapacity<TManaged, TDatabase>(ref TManaged[] manageds, in TDatabase database)
        {
            //int lengthManageds = manageds.Length;
            //int lengthDatabase = database.GetIdDataLength();

            //if (lengthManageds != lengthDatabase)
            //{
            //    Array.Resize(ref manageds, lengthDatabase);
            //}
        }
    }
}