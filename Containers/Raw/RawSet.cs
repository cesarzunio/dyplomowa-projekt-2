using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Burst.CompilerServices;
using Unity.Burst;

namespace Ces.Collections
{
    [NoAlias]
    public unsafe struct RawSet<T> : IDisposable where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction, NoAlias]
        public T* Data;

        public int Count;
        int _capacity;
        readonly Allocator _allocator;

        public readonly bool IsCreated => Data != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSet(Allocator allocator, int capacity = 4)
        {
            Data = null;
            Count = 0;
            _capacity = 0;
            _allocator = allocator;

            if (allocator == Allocator.None)
                return;

            if (capacity <= 0)
                throw new Exception($"RawSet :: Capacity ({capacity}) must be higher than 0!");

            SetCapacity(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSet(RawSerializationData<T> serializationData)
        {
            if (serializationData.Allocator == Allocator.None)
            {
                Data = null;
                Count = 0;
                _capacity = 0;
                _allocator = serializationData.Allocator;
                return;
            }

            if (serializationData.IsInvalid)
                throw new Exception($"RawSet :: SerializationData is invalid ({serializationData.Array == null}, {serializationData.Length}, {(int)serializationData.Allocator})!");

            Data = serializationData.Array;
            Count = serializationData.Length;
            _capacity = serializationData.Length;
            _allocator = serializationData.Allocator;
        }

        public static RawSet<T> Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(Allocator.None, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (_allocator == Allocator.None)
                return;

            if (!IsCreated)
                throw new Exception($"RawSet :: Is already disposed!");

            CesMemoryUtility.FreeAndNullify(ref Data, _allocator);
        }

        public readonly ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (CesCollectionsUtility.IsOutOfRange(index, Count))
                    throw new Exception($"RawSet :: this[] :: Index ({index}) out of range ({Count})!");
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
        public readonly T* Ptr(int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"RawSet :: Ptr :: Index ({index}) out of range ({Count})!");
#endif

            return Data + index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T* Ptr(uint index)
        {
            return Ptr((int)index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T value)
        {
            ResizeIfFull();

            Data[Count++] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(T value, int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"RawSet :: Insert :: Index ({index}) out of range ({Count})!");
#endif

            ResizeIfFull();

            int elementsToMove = Count - index;

            if (elementsToMove > 0)
            {
                CesMemoryUtility.ShiftRightByOne(Data + index + 1, elementsToMove);
            }

            Data[index] = value;
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"RawSet :: RemoveAt :: Index ({index}) out of range ({Count})!");
#endif

            int indexLast = --Count;

            if (Hint.Likely(index != indexLast))
            {
                Data[index] = Data[indexLast];
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

            var data = CesMemoryUtility.Allocate<T>(capacity, _allocator);

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
                SetCapacity(CesCollectionsUtility.CapacityUp(_capacity));
            }
        }
    }

    // --------------------------------------------------------------------------
    // --------------------------------------------------------------------------
    // --------------------------------------------------------------------------
    // --------------------------------------------------------------------------
    // --------------------------------------------------------------------------

    public unsafe struct RawSet<T0, T1> : IDisposable
        where T0 : unmanaged
        where T1 : unmanaged
    {
        [NativeDisableUnsafePtrRestriction]
        public T0* Data0;

        [NativeDisableUnsafePtrRestriction]
        public T1* Data1;

        public int Count;
        int _capacity;
        readonly Allocator _allocator;

        public readonly bool IsCreated => Data0 != null;
        public static RawSet<T0, T1> Null => new(Allocator.None, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSet(Allocator allocator, int capacity = 8)
        {
            Data0 = null;
            Data1 = null;
            Count = 0;
            _capacity = 0;
            _allocator = allocator;

            if (allocator == Allocator.None)
                return;

            if (capacity <= 0)
                throw new Exception($"RawSet :: Capacity ({capacity}) must be higher than 0!");

            SetCapacity(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSet(RawSerializationData<T0, T1> serializationData)
        {
            if (serializationData.Allocator == Allocator.None)
            {
                Data0 = null;
                Data1 = null;
                Count = 0;
                _capacity = 0;
                _allocator = serializationData.Allocator;
                return;
            }

            if (serializationData.IsInvalid)
                throw new Exception($"RawSet :: SerializationData is invalid ({serializationData.Array0 == null}, {serializationData.Array1 == null}, {serializationData.Length}, {(int)serializationData.Allocator})!");

            Data0 = serializationData.Array0;
            Data1 = serializationData.Array1;
            Count = serializationData.Length;
            _capacity = serializationData.Length;
            _allocator = serializationData.Allocator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (!IsCreated && _allocator != Allocator.None)
                throw new Exception("RawSet :: Is already disposed!");

            CesMemoryUtility.FreeAndNullify(ref Data0, _allocator);
            CesMemoryUtility.FreeAndNullify(ref Data1, _allocator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ref T0 GetData0(int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception("RawSet :: GetData0 :: Index out of range!");
#endif

            return ref Data0[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ref T0 GetData0(uint index)
        {
            return ref GetData0((int)index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ref T1 GetData1(int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception("RawSet :: GetData1 :: Index out of range!");
#endif

            return ref Data1[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ref T1 GetData1(uint index)
        {
            return ref GetData1((int)index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T0 value0, T1 value1)
        {
            ResizeIfFull();

            Data0[Count] = value0;
            Data1[Count] = value1;

            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(T0 value0, T1 value1, int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"RawSet :: Insert :: Index ({index}) out of range ({Count})!");
#endif

            ResizeIfFull();

            int elementsToMove = Count - index;

            if (elementsToMove > 0)
            {
                CesMemoryUtility.ShiftRightByOne(Data0 + index + 1, elementsToMove);
                CesMemoryUtility.ShiftRightByOne(Data1 + index + 1, elementsToMove);
            }

            Data0[index] = value0;
            Data1[index] = value1;

            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, Count))
                throw new Exception($"RawSet :: RemoveAt :: Index ({index}) out of range ({Count})!");
#endif

            int indexLast = --Count;

            if (Hint.Likely(index != indexLast))
            {
                Data0[index] = Data0[indexLast];
                Data1[index] = Data1[indexLast];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
        {
            for (int i = 0, j = Count - 1; i < j; i++, j--)
            {
                (Data0[j], Data0[i]) = (Data0[i], Data0[j]);
            }

            for (int i = 0, j = Count - 1; i < j; i++, j--)
            {
                (Data1[j], Data1[i]) = (Data1[i], Data1[j]);
            }
        }

        void SetCapacity(int capacity)
        {
#if CES_COLLECTIONS_CHECK
            if (capacity <= _capacity)
                throw new Exception($"RawSet :: SetCapacity :: Passed capacity ({capacity}) must be greater than existing capacity ({_capacity})!");
#endif

            var data0 = CesMemoryUtility.Allocate<T0>(capacity, _allocator);
            var data1 = CesMemoryUtility.Allocate<T1>(capacity, _allocator);

            if (_capacity > 0)
            {
                CesMemoryUtility.CopyAndFree(_capacity, data0, Data0, _allocator);
                CesMemoryUtility.CopyAndFree(_capacity, data1, Data1, _allocator);
            }

            Data0 = data0;
            Data1 = data1;

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