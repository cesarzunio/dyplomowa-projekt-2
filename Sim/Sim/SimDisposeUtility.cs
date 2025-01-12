using Ces.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class SimDisposeUtility
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void DisposeAll(ref Sim sim)
    {
        SimDisposeDynamicUtility.DisposeSim(ref sim);
        SimDisposeConstUtility.DisposeSim(ref sim);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void DisposeAll(ref RawPtr<Sim> sim)
    {
        DisposeAll(ref sim.Ref);
        sim.Dispose();
    }
}
