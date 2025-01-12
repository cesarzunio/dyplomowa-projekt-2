using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Ces.Collections
{
    [NoAlias]
    public unsafe struct RawPtr<T> where T : unmanaged
    {
        [NativeDisableUnsafePtrRestriction, NoAlias]
        T* _ptr;

        readonly Allocator _allocator;

        public readonly bool IsCreated => _ptr != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawPtr(Allocator allocator)
        {
            _ptr = CesMemoryUtility.Allocate<T>(1, allocator);
            _allocator = allocator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawPtr(Allocator allocator, T value) : this(allocator)
        {
            *_ptr = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (!IsCreated)
                throw new Exception("RawPtr :: Dispose :: Is already disposed!");

            UnsafeUtility.Free(_ptr, _allocator);
            _ptr = null;
        }

        public readonly ref T Ref
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (!IsCreated)
                    throw new Exception("RawPtr :: Ref :: Ptr is null!");
#endif

                return ref (*_ptr);
            }
        }

        public readonly T Val
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (!IsCreated)
                    throw new Exception("RawPtr :: Val :: Ptr is null!");
#endif

                return *_ptr;
            }
        }

        public readonly T* Ptr
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if CES_COLLECTIONS_CHECK
                if (!IsCreated)
                    throw new Exception("RawPtr :: Ptr :: Ptr is null!");
#endif

                return _ptr;
            }
        }
    }
}