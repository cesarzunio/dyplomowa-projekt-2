using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PopDemographics
{
    /// <summary>
    /// x - Children <br></br>
    /// y - Young adult <br></br>
    /// z - Old adult <br></br>
    /// w - Retired <br></br>
    /// </summary>
    public float4 AgePercents;
}
