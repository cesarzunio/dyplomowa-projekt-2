using System;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct ResourceAmount
{
    public uint Index;
    public float Amount;
}
