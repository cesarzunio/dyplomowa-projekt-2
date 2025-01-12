using Ces.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Ces.Collections
{
    public static unsafe class RawArrayUtility
    {
        public static RawArray<T> CreateRawArrayFromArray<T>(in T[] array, Allocator allocator) where T : unmanaged
        {
            if (array == null)
                throw new Exception("RawArrayUtility :: CreateRawArrayFromArray :: Array is null!");

            if (array.Length <= 0)
                throw new Exception("RawArrayUtility :: CreateRawArrayFromArray :: Array length is 0!");

            var rawArray = new RawArray<T>(allocator, array.Length);

            fixed (T* ptr = array)
            {
                CesMemoryUtility.Copy(array.LongLength, rawArray.Data, ptr);
            }

            return rawArray;
        }

        public static T[] CreateArrayFromRawArray<T>(in RawArray<T> rawArray) where T : unmanaged
        {
            if (!rawArray.IsCreated)
                throw new Exception("RawArrayUtility :: CreateArrayFromRawArray :: RawArray is not created!");

            if (rawArray.Length <= 0)
                throw new Exception("RawArrayUtility :: CreateArrayFromRawArray :: RawArray length ({rawArray.Length}) is invalid!");

            var array = new T[rawArray.Length];

            fixed (T* ptr = array)
            {
                CesMemoryUtility.Copy(rawArray.Length, ptr, rawArray.Data);
            }

            return array;
        }
    }
}