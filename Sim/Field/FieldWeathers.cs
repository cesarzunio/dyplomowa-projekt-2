using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Ces.Collections;

public unsafe struct FieldWeathers
{
    const int LENGTH = 12;

    [NativeDisableUnsafePtrRestriction]
    public fixed float MonthToData[LENGTH];

    public readonly float this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, LENGTH))
                throw new Exception($"FieldWeathersExtensions :: this[] :: Index ({index}) out of range ({LENGTH})!");
#endif

            return MonthToData[index];
        }
    }

    public readonly float this[uint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this[(int)index];
    }
}