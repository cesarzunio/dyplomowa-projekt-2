using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Burst.CompilerServices;
using System.Runtime.CompilerServices;

public static unsafe class TexUtilities
{
    public static int2 ClampPixelCoord(int2 pixelCoord, int2 textureSize)
    {
        if (Hint.Unlikely(pixelCoord.y < 0 || pixelCoord.y > textureSize.y - 1))
        {
            pixelCoord.y = math.clamp(pixelCoord.y, 0, textureSize.y - 1);
            pixelCoord.x += textureSize.x / 2;
        }

        pixelCoord.x = (pixelCoord.x + textureSize.x) % textureSize.x;

        return pixelCoord;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PixelCoordToFlat(int pixelCoordX, int pixelCoordY, int textureSizeX) => pixelCoordX + (pixelCoordY * textureSizeX);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PixelCoordToFlat(int2 pixelCoord, int textureSizeX) => PixelCoordToFlat(pixelCoord.x, pixelCoord.y, textureSizeX);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int2 FlatToPixelCoordInt2(int flat, int textureSizeX) => new(flat % textureSizeX, flat / textureSizeX);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FlatToPixelCoordInts(int flat, int textureSizeX, out int pixelCoordX, out int pixelCoordY)
    {
        pixelCoordX = flat % textureSizeX;
        pixelCoordY = flat / textureSizeX;
    }

    /// <summary>
    /// Up Down, Left Right
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetNeighbors4(int2 pixelCoord, int2 textureSize, int2* neighborsArray)
    {
        neighborsArray[0] = ClampPixelCoord(pixelCoord + new int2(0, 1), textureSize);
        neighborsArray[1] = ClampPixelCoord(pixelCoord + new int2(0, -1), textureSize);
        neighborsArray[2] = ClampPixelCoord(pixelCoord + new int2(-1, 0), textureSize);
        neighborsArray[3] = ClampPixelCoord(pixelCoord + new int2(1, 0), textureSize);
    }

    /// <summary>
    /// Left to Right, Down to Up
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetNeighbors8(int2 pixelCoord, int2 textureSize, int2* neighborsArray)
    {
        neighborsArray[0] = ClampPixelCoord(pixelCoord + new int2(-1, -1), textureSize);
        neighborsArray[1] = ClampPixelCoord(pixelCoord + new int2(0, -1), textureSize);
        neighborsArray[2] = ClampPixelCoord(pixelCoord + new int2(1, -1), textureSize);

        neighborsArray[3] = ClampPixelCoord(pixelCoord + new int2(-1, 0), textureSize);
        neighborsArray[4] = ClampPixelCoord(pixelCoord + new int2(1, 0), textureSize);

        neighborsArray[5] = ClampPixelCoord(pixelCoord + new int2(-1, 1), textureSize);
        neighborsArray[6] = ClampPixelCoord(pixelCoord + new int2(0, 1), textureSize);
        neighborsArray[7] = ClampPixelCoord(pixelCoord + new int2(1, 1), textureSize);
    }
}
