using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Ces.Collections
{
    public unsafe struct MemoryBlock : IDisposable
    {
        [NativeDisableUnsafePtrRestriction]
        byte** _indexToArray;

        [NativeDisableUnsafePtrRestriction]
        bool* _indexToUsed;

        int _count;
        int _capacity;
        int _lastReleasedIndex;

        readonly int _arraySize;
        readonly Allocator _allocator;

        CesSpinLock _lock;

        public readonly bool IsCreated => _indexToArray != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemoryBlock(Allocator allocator, int arraySize, int capacity = 8)
        {
            if (arraySize <= 0)
                throw new Exception($"MemoryBlock :: Array size ({arraySize}) must be higher than 0!");

            if (allocator == Allocator.Invalid || allocator == Allocator.None)
                throw new Exception($"MemoryBlock :: Allocator ({(int)allocator}) is not valid!");

            _indexToArray = null;
            _indexToUsed = null;

            _count = 0;
            _capacity = 0;
            _lastReleasedIndex = 0;

            _arraySize = arraySize;
            _allocator = allocator;

            _lock = CesSpinLock.Create();

            SetCapacity(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            for (int i = 0; i < _capacity; i++)
            {
                UnsafeUtility.Free(_indexToArray[i], _allocator);
            }

            CesMemoryUtility.FreeAndNullify(ref _indexToArray, _allocator);
            CesMemoryUtility.FreeAndNullify(ref _indexToUsed, _allocator);
        }

        public MemoryBlockSpan<T> Get<T>() where T : unmanaged
        {
            _lock.Lock();

            int freeIndex = GetFreeIndex();

            _indexToUsed[freeIndex] = true;
            _count++;

            _lock.Unlock();

            return new MemoryBlockSpan<T>(_indexToArray[freeIndex], CapacityOfSpan<T>(), freeIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemoryBlockSpan<T> GetWithoutLock<T>() where T : unmanaged
        {
            if (_lock.IsLocked)
                throw new Exception("MemoryBlock :: GetWithoutLock :: Lock is locked!");

            int freeIndex = GetFreeIndex();

            _indexToUsed[freeIndex] = true;
            _count++;

            return new MemoryBlockSpan<T>(_indexToArray[freeIndex], CapacityOfSpan<T>(), freeIndex);
        }

        public void Release<T>(ref MemoryBlockSpan<T> span) where T : unmanaged
        {
#if CES_COLLECTIONS_CHECK
            if (!span.IsCreated)
                throw new Exception("$MemoryBlock :: Span is not created!");

            if (CesCollectionsUtility.ExceedsCapacity(span.SpanIndex, _capacity))
                throw new Exception($"MemoryBlock :: Release :: Span index ({span.SpanIndex}) is out of range ({_capacity})!");
#endif

            _lock.Lock();

#if CES_COLLECTIONS_CHECK
            if (!_indexToUsed[span.SpanIndex])
                throw new Exception($"MemoryBlock :: Release :: Span index {span.SpanIndex} is not used!");
#endif

            _indexToUsed[span.SpanIndex] = false;
            _count--;

            _lock.Unlock();

            span = MemoryBlockSpan<T>.Null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseWithoutLock<T>(ref MemoryBlockSpan<T> span) where T : unmanaged
        {
#if CES_COLLECTIONS_CHECK
            if (_lock.IsLocked)
                throw new Exception("MemoryBlock :: Lock is locked!");

            if (!span.IsCreated)
                throw new Exception("$MemoryBlock :: Span is not created!");

            if (CesCollectionsUtility.ExceedsCapacity(span.SpanIndex, _capacity))
                throw new Exception($"MemoryBlock :: Release :: Span index ({span.SpanIndex}) is out of range ({_capacity})!");

            if (!_indexToUsed[span.SpanIndex])
                throw new Exception($"MemoryBlock :: Release :: Span index {span.SpanIndex} is not used!");
#endif

            _indexToUsed[span.SpanIndex] = false;
            _count--;

            span = MemoryBlockSpan<T>.Null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CapacityOfSpan<T>() where T : unmanaged
        {
            return _arraySize / UnsafeUtility.SizeOf<T>();
        }

        void SetCapacity(int capacity)
        {
#if CES_COLLECTIONS_CHECK
            if (capacity <= _capacity)
                throw new Exception($"MemoryBlock :: SetCapacity :: Passed capacity ({capacity}) must be greater than existing capacity ({_capacity})!");

            if (_count != _capacity)
                throw new Exception($"MemoryBlock :: SetCapacity :: Count ({_count}) must be 0 when setting capacity!");
#endif

            var indexToArray = CesMemoryUtility.AllocatePtrs<byte>(capacity, _allocator);
            var indexToUsed = CesMemoryUtility.Allocate<bool>(capacity, _allocator);

            if (_capacity > 0)
            {
                CesMemoryUtility.CopyAndFree(capacity, (IntPtr*)indexToArray, (IntPtr*)_indexToArray, _allocator);
                CesMemoryUtility.CopyAndFree(capacity, indexToUsed, _indexToUsed, _allocator);
            }

            for (int i = _capacity; i < capacity; i++)
            {
                indexToArray[i] = CesMemoryUtility.Allocate<byte>(_arraySize, _allocator);
                indexToUsed[i] = false;
            }

            _indexToArray = indexToArray;
            _indexToUsed = indexToUsed;

            _capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetFreeIndex()
        {
            if (_count == _capacity)
            {
                SetCapacity(_capacity * 2);
                return _count;
            }

            for (int i = 0; i < _capacity; i++)
            {
                int index = (_lastReleasedIndex + i) % _capacity;

                if (!_indexToUsed[index])
                    return index;
            }

            throw new Exception($"MemoryBlock :: GetFreeIndex :: No free index found, this should never happen.. ({_count}, {_capacity})");
        }
    }
}