using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public struct Trader
{
    public float Price0;
    public float Price1;
    public float Price2;

    public ResourceAmount Archetype;
    public float AmountStored;

    public TraderCurrent Current;

    public float PriceCap;
}

[StructLayout(LayoutKind.Sequential)]
public struct TraderCurrent
{
    public float AmountTrading;
    public float Cost;
}
