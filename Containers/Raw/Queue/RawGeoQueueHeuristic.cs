using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Ces.Collections
{
    public readonly unsafe struct RawGeoQueueHeuristic
    {
        readonly RawGeoQueueHeuristicType _type;
        readonly uint _nodeIndexTarget;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        RawGeoQueueHeuristic(RawGeoQueueHeuristicType type, uint nodeIndexTarget)
        {
            _nodeIndexTarget = nodeIndexTarget;
            _type = type;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawGeoQueueHeuristic Null()
        {
            return new RawGeoQueueHeuristic(RawGeoQueueHeuristicType.Invalid, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawGeoQueueHeuristic None()
        {
            return new RawGeoQueueHeuristic(RawGeoQueueHeuristicType.None, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawGeoQueueHeuristic EuclideanDistance(uint nodeIndexTarget)
        {
            return new RawGeoQueueHeuristic(RawGeoQueueHeuristicType.EuclideanDistance, nodeIndexTarget);
        }

        public readonly double CalculateHeuristic(uint nodeIndex, in RawSpan<double2> nodesColumnsGeoCoordRows) => _type switch
        {
            RawGeoQueueHeuristicType.None => 0,
            RawGeoQueueHeuristicType.EuclideanDistance => GeoUtilitiesDouble.Distance(nodesColumnsGeoCoordRows[nodeIndex], nodesColumnsGeoCoordRows[_nodeIndexTarget]),

            _ => throw new Exception($"RawGeoQueueHeuristic :: CalculateHeuristic :: Cannot match RawGeoQueueHeuristicType ({(uint)_type})!")
        };
    }

    public enum RawGeoQueueHeuristicType : uint
    {
        Invalid = 0,

        None = 10,
        EuclideanDistance = 11,
    }
}