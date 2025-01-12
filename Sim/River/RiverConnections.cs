using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Ces.Collections;

public unsafe struct RiverConnections
{
    const int CAPACITY = 4;

    public readonly int Length;

    [NativeDisableUnsafePtrRestriction]
    public fixed uint Indexes[CAPACITY];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RiverConnections(RawArray<uint> indexes)
    {
        if (CesCollectionsUtility.ExceedsCapacity(indexes.Length, CAPACITY))
            throw new Exception($"RiverConnections :: Length ({indexes.Length}) exceeds capacity ({CAPACITY})!");

        Length = indexes.Length;

        for (int i = 0; i < indexes.Length; i++)
        {
            Indexes[i] = indexes[i];
        }
    }

    public readonly ref readonly uint this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Length))
                throw new Exception($"RiverConnections :: this[] :: Index ({index}) of out range ({Length})!");
#endif

            return ref Indexes[index];
        }
    }

    public readonly ref readonly uint this[uint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref this[(int)index];
    }
}