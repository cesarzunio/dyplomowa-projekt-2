using System.Runtime.CompilerServices;
using Unity.Mathematics;

static class EdgeUtility
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetNodeIndexOther(uint2 nodesIndexes, uint nodeIndex)
    {
        return nodesIndexes.x ^ nodesIndexes.y ^ nodeIndex;
    }
}