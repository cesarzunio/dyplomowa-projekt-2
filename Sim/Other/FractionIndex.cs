using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct FractionIndex
{
    public float Fraction;
    public uint Index;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FractionIndex(float fraction, uint index)
    {
        Fraction = fraction;
        Index = index;
    }
}
