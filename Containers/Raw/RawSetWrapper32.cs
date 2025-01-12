using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Ces.Collections
{
    [StructLayout(LayoutKind.Sequential, Size = 32)]
    public struct RawSetWrapper32<T> : IDisposable where T : unmanaged
    {
        public RawSet<T> Set;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSetWrapper32(Allocator allocator, int capacity = 4)
        {
            Set = new RawSet<T>(allocator, capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawSetWrapper32(RawSerializationData<T> serializationData)
        {
            Set = new RawSet<T>(serializationData);
        }

        public static RawSetWrapper32<T> Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new()
            {
                Set = RawSet<T>.Null,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Set.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Serialize(in FileStream fileStream, in RawSetWrapper32<T> wrapper)
        {
            BinarySaveUtility.WriteRawSet(in fileStream, wrapper.Set);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawSetWrapper32<T> Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
        {
            Set = BinaryReadUtility.ReadRawSet<T>(in fileStream, allocator, capacityIfEmpty),
        };
    }
}