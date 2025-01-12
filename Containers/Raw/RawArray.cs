using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Burst;

namespace Ces.Collections
{
    [NoAlias]
    public unsafe struct RawArray<T> : IDisposable
        where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction, NoAlias]
        public T* Data;

        public readonly int Length;
        readonly Allocator _allocator;

        public readonly bool IsCreated => Data != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawArray(Allocator allocator, int length)
        {
            if (allocator == Allocator.None)
            {
                Data = null;
                Length = 0;
                _allocator = allocator;
                return;
            }

            if (length <= 0)
                throw new Exception($"RawArray :: Length ({length}) must be higher than 0!");

            Data = CesMemoryUtility.Allocate<T>(length, allocator);
            Length = length;
            _allocator = allocator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawArray(Allocator allocator, int length, T valueDefault)
        {
            if (allocator == Allocator.None)
            {
                Data = null;
                Length = 0;
                _allocator = allocator;
                return;
            }

            if (length <= 0)
                throw new Exception($"RawArray :: Length ({length}) must be higher than 0!");

            Data = CesMemoryUtility.Allocate(length, allocator, valueDefault);
            Length = length;
            _allocator = allocator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawArray(RawSerializationData<T> serializationData)
        {
            if (serializationData.Allocator == Allocator.None)
            {
                Data = null;
                Length = 0;
                _allocator = serializationData.Allocator;
                return;
            }

            if (serializationData.IsInvalid)
                throw new Exception("RawArray :: SerializationData is invalid!");

            Data = serializationData.Array;
            Length = serializationData.Length;
            _allocator = serializationData.Allocator;
        }

        public static RawArray<T> Null
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
                throw new Exception($"RawArray :: Dispose :: Is not created!");

            CesMemoryUtility.FreeAndNullify(ref Data, _allocator);
        }

        public readonly ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (CesCollectionsUtility.IsOutOfRange(index, Length))
                    throw new Exception($"RawArray :: this[] :: Index ({index}) out of range ({Length})!");
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
            if (CesCollectionsUtility.IsOutOfRange(index, Length))
                throw new Exception($"RawArray :: Ptr :: Index ({index}) out of range ({Length})!");
#endif

            return Data + index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T* Ptr(uint index) => Ptr((int)index);
    }
}