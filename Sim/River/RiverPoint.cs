using Ces.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

public struct RiverPoint
{
    public RiverPointData Data;
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct RiverPointData
{
    public float WaterAmount;
    public float WaterAmountSource;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in RiverPointData data)
    {
        BinarySaveUtility.WriteValue(fileStream, data.WaterAmount);
        BinarySaveUtility.WriteValue(fileStream, data.WaterAmountSource);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RiverPointData Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty) => new()
    {
        WaterAmount = fileStream.ReadValue<float>(),
        WaterAmountSource = fileStream.ReadValue<float>(),
    };
}
