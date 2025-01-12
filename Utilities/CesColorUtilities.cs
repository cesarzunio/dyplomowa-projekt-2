using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public static class CesColorUtilities
{
    const int TWO_8 = 1 << 8;
    const int TWO_16 = 1 << 16;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Color32ToIndex(Color32 color)
    {
        int r = color.r;
        int g = color.g * TWO_8;
        int b = color.b * TWO_16;

        return r + g + b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color32 IndexToColor32(int index)
    {
        int r = index % TWO_8;
        int g = (index / TWO_8) % TWO_8;
        int b = (index / TWO_16) % TWO_8;

        return new Color32((byte)r, (byte)g, (byte)b, 255);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToIndex(this Color32 color) => Color32ToIndex(color);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color32 ToColor32(this int index) => IndexToColor32(index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ColorEquals(Color32 lhs, Color32 rhs) => lhs.r == rhs.r && lhs.g == rhs.g && lhs.b == rhs.b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ColorNotEquals(Color32 lhs, Color32 rhs) => !ColorEquals(lhs, rhs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte FloatToByte(float f) => (byte)math.round(math.saturate(f) * 255);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ByteToFloat(byte b) => b / 255f;

    const int RANDOM_COLOR_TRIES_MAX = 1024;

    public static int GetRandomColor(HashSet<int> usedColors, ref Unity.Mathematics.Random random)
    {
        int tries = 0;

        while (true)
        {
            byte r = (byte)random.NextInt(0, 255);
            byte g = (byte)random.NextInt(0, 255);
            byte b = (byte)random.NextInt(0, 255);

            int color = new Color32(r, g, b, 255).ToIndex();

            if (usedColors.Add(color))
                return color;

            if (tries++ > RANDOM_COLOR_TRIES_MAX)
                throw new System.Exception("CesColorUtilities :: GetRandomColor :: Exceeded max number of tries!");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color32 Lerp(Color32 a, Color32 b, float t)
    {
        t = Mathf.Clamp01(t);

        return new Color32
        {
            r = (byte)Mathf.Lerp(a.r, b.r, t),
            g = (byte)Mathf.Lerp(a.g, b.g, t),
            b = (byte)Mathf.Lerp(a.b, b.b, t),
            a = (byte)Mathf.Lerp(a.a, b.a, t),
        };
    }
}
