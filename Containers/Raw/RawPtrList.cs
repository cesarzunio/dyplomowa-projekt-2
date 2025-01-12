using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Burst.CompilerServices;

namespace Ces.Collections
{
    public unsafe struct RawPtrList : IDisposable
    {
        [NativeDisableUnsafePtrRestriction]
        public IntPtr* Data;

        public int Count;
        int _capacity;
        readonly Allocator _allocator;

        public readonly bool IsCreated => Data != null;
        public static RawPtrList Null => new(Allocator.None, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawPtrList(Allocator allocator, int capacity = 8)
        {
            Data = null;
            Count = 0;
            _capacity = 0;
            _allocator = allocator;

            if (allocator == Allocator.None)
                return;

            if (capacity <= 0)
                throw new Exception($"RawPtrList :: Capacity ({capacity}) must be higher than 0!");

            SetCapacity(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (!IsCreated && _allocator != Allocator.None)
                throw new Exception("RawPtrList :: Is already disposed!");

            CesMemoryUtility.FreeAndNullify(ref Data, _allocator);
        }

        public readonly ref IntPtr this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (CesCollectionsUtility.IsOutOfRange(index, Count))
                    throw new Exception($"RawPtrList :: this[] :: Index ({index}) out of range ({Count})!");
#endif

                return ref Data[index];
            }
        }

        public readonly ref IntPtr this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[(int)index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(void* value)
        {
            ResizeIfFull();

            Data[Count++] = new IntPtr(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(void* value, int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"RawSet :: Insert :: Index ({index}) out of range ({Count})!");
#endif

            ResizeIfFull();

            int elementsToMove = Count++ - index;

            if (Hint.Likely(elementsToMove > 0))
            {
                CesMemoryUtility.ShiftRightByOne(Data + index + 1, elementsToMove);
            }

            Data[index] = new IntPtr(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"RawSet :: RemoveAt :: Index ({index}) out of range ({Count})!");
#endif

            int elementsToMove = --Count - index;

            if (Hint.Likely(elementsToMove > 0))
            {
                CesMemoryUtility.ShiftLeftByOne(Data + index, elementsToMove);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            for (int i = 0, j = Count - 1; i < j; i++, j--)
            {
                (Data[j], Data[i]) = (Data[i], Data[j]);
            }
        }

        void SetCapacity(int capacity)
        {
#if CES_COLLECTIONS_CHECK
            if (capacity <= _capacity)
                throw new Exception($"RawSet :: SetCapacity :: Passed capacity ({capacity}) must be greater than existing capacity ({_capacity})!");
#endif

            var data = CesMemoryUtility.Allocate<IntPtr>(capacity, _allocator);

            if (_capacity > 0)
            {
                CesMemoryUtility.CopyAndFree(_capacity, data, Data, _allocator);
            }

            Data = data;
            _capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ResizeIfFull()
        {
            if (Hint.Unlikely(Count == _capacity))
            {
                SetCapacity(_capacity * 2);
            }
        }
    }
}