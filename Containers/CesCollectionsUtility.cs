using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using Ces.Collections;

namespace Ces.Collections
{
    public static unsafe class CesCollectionsUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutOfRange(int index, int capacity)
        {
            return index < 0 || index >= capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutOfRange(uint index, int capacity)
        {
            return (int)index >= capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExceedsCapacity(int length, int capacity)
        {
            return length < 0 || length > capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CapacityUp(int capacityCurrent)
        {
            return (int)(capacityCurrent * 1.5f);
        }

        public static int CapacityInitialAligned(int capacityMin, int capacityRequested)
        {
            int capacityCurrent = capacityMin;

            while (capacityCurrent < capacityRequested)
            {
                capacityCurrent = CapacityUp(capacityCurrent);
            }

            return capacityCurrent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawSet<T> ShallowCopyToRawBag<T>(this ref RawArray<T> original, Allocator allocator) where T : unmanaged
        {
            var copy = new RawSet<T>(allocator, original.Length);

            for (int i = 0; i < original.Length; i++)
            {
                copy.Add(original[i]);
            }

            return copy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeDeep<T>(this ref RawArray<T> rawArray) where T : unmanaged, IDisposable
        {
            for (int i = 0; i < rawArray.Length; i++)
            {
                rawArray[i].Dispose();
            }

            rawArray.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeArraySimpleOfDisposables<T>(T* array, int length) where T : unmanaged, IDisposable
        {
            for (int i = 0; i < length; i++)
            {
                array[i].Dispose();
            }
        }
    }
}