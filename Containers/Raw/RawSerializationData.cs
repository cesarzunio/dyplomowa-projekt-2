using System.Runtime.CompilerServices;
using Unity.Collections;

namespace Ces.Collections
{
    public readonly unsafe struct RawSerializationData<T> where T : unmanaged
    {
        public readonly T* Array;
        public readonly int Length;
        public readonly Allocator Allocator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSerializationData(T* array, int length, Allocator allocator)
        {
            Array = array;
            Length = length;
            Allocator = allocator;
        }

        public readonly bool IsInvalid => Array == null || Length <= 0;
    }

    public readonly unsafe struct RawSerializationData<T0, T1>
        where T0 : unmanaged
        where T1 : unmanaged
    {
        public readonly T0* Array0;
        public readonly T1* Array1;
        public readonly int Length;
        public readonly Allocator Allocator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSerializationData(T0* array0, T1* array1, int length, Allocator allocator)
        {
            Array0 = array0;
            Array1 = array1;
            Length = length;
            Allocator = allocator;
        }

        public readonly bool IsInvalid => Array0 == null || Array1 == null || Length <= 0;
    }
}