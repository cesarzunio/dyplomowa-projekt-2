using Ces.Collections;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;

public struct Node
{
    public bool Enabled;
    public RawSetWrapper32<DatabaseId> GroundUnitsIds;
}