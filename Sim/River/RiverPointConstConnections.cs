using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Ces.Collections;

public unsafe struct RiverPointConstConnections
{
    public const int CAPACITY = 2;

    public readonly int Length;

    [NativeDisableUnsafePtrRestriction]
    public fixed uint RiverPointsIndexes[CAPACITY];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RiverPointConstConnections(int length, uint* riverPointsIndexes)
    {
        if (CesCollectionsUtility.ExceedsCapacity(length, CAPACITY))
            throw new Exception($"RiverPointConnections :: Length ({length}) exceeds capacity ({CAPACITY})!");

        Length = length;

        for (int i = 0; i < length; i++)
        {
            RiverPointsIndexes[i] = riverPointsIndexes[i];
        }
    }

    public readonly ref readonly uint this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Length))
                throw new Exception($"RiverPointConnections :: this[] :: Index ({index}) of out range ({Length})!");
#endif

            return ref RiverPointsIndexes[index];
        }
    }

    public readonly ref readonly uint this[uint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref this[(int)index];
    }
}