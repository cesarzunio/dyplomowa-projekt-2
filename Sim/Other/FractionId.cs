using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ces.Collections;

[StructLayout(LayoutKind.Sequential)]
public struct FractionId
{
    public float Fraction;
    public DatabaseId Id;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public FractionId(float fraction, DatabaseId id)
    {
        Fraction = fraction;
        Id = id;
    }

    public static FractionId Null => new()
    {
        Fraction = 0f,
        Id = DatabaseId.Invalid
    };
}
