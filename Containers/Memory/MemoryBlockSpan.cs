using System.Runtime.CompilerServices;
using System;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Ces.Collections
{
    public unsafe struct MemoryBlockSpan<T> where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction]
        public readonly T* Data;

        public int Count;
        public readonly int Capacity;
        public readonly int SpanIndex;

        public readonly bool IsCreated => Data != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemoryBlockSpan(byte* data, int capacity, int spanIndex)
        {
            Data = (T*)data;
            Count = 0;
            Capacity = capacity;
            SpanIndex = spanIndex;
        }

        public static MemoryBlockSpan<T> Null => new(null, -1, -1);

        public readonly ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (CesCollectionsUtility.IsOutOfRange(index, Count))
                    throw new Exception($"MemoryBlockSpan :: this[] :: Index ({index}) out of range ({Count})!");
#endif

                return ref Data[index];
            }
        }

        public readonly ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[(int)index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
#if CES_COLLECTIONS_CHECK
            if (Count == Capacity)
                throw new Exception($"MemoryBlockSpan :: Add :: Span ({Count}) is full ({Capacity})!");
#endif

            Data[Count++] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryNotAdd(T value)
        {
            if (Count == Capacity)
                return false;

            Data[Count++] = value;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"MemoryBlockSpan :: RemoveAt :: Index ({index}) out of range ({Count})!");
#endif

            int indexLast = --Count;

            if (Hint.Likely(index != indexLast))
            {
                Data[index] = Data[indexLast];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Count = 0;
        }
    }
}