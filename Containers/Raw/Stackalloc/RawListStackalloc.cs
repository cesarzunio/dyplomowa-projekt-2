using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Ces.Collections
{
    public unsafe struct RawListStackalloc<T> where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction]
        public readonly T* Data;

        public int Count;
        public readonly int Capacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawListStackalloc(T* data, int capacity)
        {
            if (data == null)
                throw new Exception("RawListStackalloc :: Data is null!");

            if (capacity <= 0)
                throw new Exception($"RawListStackalloc :: Capacity ({capacity}) must be positive!");

            Data = data;
            Count = 0;
            Capacity = capacity;
        }

        public readonly ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (CesCollectionsUtility.IsOutOfRange(index, Count))
                    throw new Exception($"RawListStackalloc :: this[] :: Index ({index}) out of range ({Capacity})!");
#endif

                return ref Data[index];
            }
        }

        public readonly ref T this[uint index] => ref this[(int)index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            if (Count == Capacity)
                throw new Exception($"RawListStackalloc :: List is full ({Capacity})!");

            Data[Count++] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => Count = 0;
    }
}