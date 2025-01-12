using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Mathematics;
using Ces.Collections;

public unsafe struct FieldsMap : IDisposable
{
    [NativeDisableUnsafePtrRestriction]
    public uint* Fields;

    public readonly int2 TextureSize;
    readonly Allocator _allocator;

    public readonly bool IsCreated => Fields != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FieldsMap(Allocator allocator, int2 textureSize)
    {
        if (allocator == Allocator.None)
        {
            Fields = null;
            TextureSize = 0;
            _allocator = allocator;
            return;
        }

        if (math.any(textureSize <= int2.zero))
            throw new Exception($"RawArray :: Texture dimensions ({textureSize}) must be higher than 0!");

        Fields = CesMemoryUtility.Allocate<uint>(textureSize.x * textureSize.y, allocator);
        TextureSize = textureSize;
        _allocator = allocator;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FieldsMap(Allocator allocator, int2 textureSize, uint* fields)
    {
        if (allocator == Allocator.None)
        {
            Fields = null;
            TextureSize = 0;
            _allocator = allocator;
            return;
        }

        if (math.any(textureSize <= int2.zero))
            throw new Exception($"RawArray :: Texture dimensions ({textureSize}) must be higher than 0!");

        Fields = fields;
        TextureSize = textureSize;
        _allocator = allocator;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (!IsCreated && _allocator != Allocator.None)
            throw new Exception("RawArray :: Dispose :: Is not created!");

        CesMemoryUtility.FreeAndNullify(ref Fields, _allocator);
    }

    public readonly uint this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
#if CES_COLLECTIONS_CHECK
            if (CesCollectionsUtility.IsOutOfRange(index, TextureSize.x * TextureSize.y))
                throw new Exception("RawArray :: this[] :: Index out of range!");
#endif

            return Fields[index];
        }
    }

    public readonly uint this[uint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this[(int)index];
    }

    public readonly uint this[int2 pixelCoord]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this[TexUtilities.PixelCoordToFlat(pixelCoord, TextureSize.x)];
    }
}