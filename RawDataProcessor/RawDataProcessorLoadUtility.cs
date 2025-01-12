using Ces.Collections;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

public static unsafe class RawDataProcessorLoadUtility
{
    const Allocator ALLOCATOR = Allocator.Persistent;

    public static RawArray<RawArray<uint>> LoadAreas(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        return BinaryReadUtility.ReadRawArrayOfRawArrays<uint>(fileStream, allocator);
    }

    public static FieldsMap LoadFieldsMap(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var binaryReader = new BinaryReader(fileStream);

        int width = binaryReader.ReadInt32();
        int height = binaryReader.ReadInt32();

        var fields = BinaryReadUtility.ReadArraySimple<uint>(fileStream, width * height, allocator);

        return new FieldsMap(new int2(width, height), fields, allocator);
    }

    public readonly struct FieldsMap
    {
        public readonly int2 TextureSize;
        public readonly uint* Fields;
        public readonly Allocator Allocator;

        public FieldsMap(int2 textureSize, uint* fields, Allocator allocator)
        {
            TextureSize = textureSize;
            Fields = fields;
            Allocator = allocator;
        }

        public void Dispose()
        {
            UnsafeUtility.Free(Fields, Allocator);
        }
    }

    public static RawArray<uint> LoadBordersDistances(string path, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<uint>(path, allocator);
    }

    public static RawArray<byte> LoadBordersMap(string path, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<byte>(path, allocator);
    }

    public static Fields LoadFields(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var binaryReader = new BinaryReader(fileStream);

        int length = binaryReader.ReadInt32();

        var centerGeoCoords = BinaryReadUtility.ReadArraySimple<double2>(fileStream, length, allocator);
        var isLand = BinaryReadUtility.ReadArraySimple<bool>(fileStream, length, allocator);
        var colors = BinaryReadUtility.ReadArraySimple<int>(fileStream, length, allocator);

        return new Fields(length, centerGeoCoords, isLand, colors, allocator);
    }

    public unsafe readonly struct Fields
    {
        public readonly int Length;
        public readonly double2* CenterGeoCoords;
        public readonly bool* IsLand;
        public readonly int* Colors;
        public readonly Allocator Allocator;

        public Fields(int length, double2* ceterGeoCoords, bool* isLand, int* color, Allocator allocator)
        {
            Length = length;
            CenterGeoCoords = ceterGeoCoords;
            IsLand = isLand;
            Colors = color;
            Allocator = allocator;
        }

        public void Dispose()
        {
            UnsafeUtility.Free(IsLand, Allocator);
            UnsafeUtility.Free(Colors, Allocator);
        }
    }

    public static Borders LoadBorders(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        int length = fileStream.ReadValue<int>();
        var fields = BinaryReadUtility.ReadArraySimple<uint2>(fileStream, length, allocator);

        var borderCoords = CesMemoryUtility.Allocate<RawArray<RawArray<int2>>>(length, allocator);

        for (int i = 0; i < length; i++)
        {
            borderCoords[i] = BinaryReadUtility.ReadRawArrayOfRawArrays<int2>(fileStream, allocator);
        }

        return new Borders(length, fields, borderCoords, allocator);
    }

    public readonly struct Borders
    {
        public readonly int Length;

        [NativeDisableUnsafePtrRestriction]
        public readonly uint2* Fields;

        [NativeDisableUnsafePtrRestriction]
        public readonly RawArray<RawArray<int2>>* BorderCoords;

        readonly Allocator _allocator;

        public Borders(int length, uint2* fields, RawArray<RawArray<int2>>* borderCoords, Allocator allocator)
        {
            Length = length;
            Fields = fields;
            BorderCoords = borderCoords;
            _allocator = allocator;
        }

        public void Dispose()
        {
            UnsafeUtility.Free(Fields, _allocator);

            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < BorderCoords[i].Length; j++)
                {
                    BorderCoords[i][j].Dispose();
                }

                BorderCoords[i].Dispose();
            }

            UnsafeUtility.Free(BorderCoords, _allocator);
        }
    }

    public static NodesFinals LoadNodes(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        int length = fileStream.ReadValue<int>();

        var geoCoord = BinaryReadUtility.ReadArraySimple<double2>(fileStream, length, allocator);
        var owner = BinaryReadUtility.ReadArraySimple<NodeOwner>(fileStream, length, allocator);
        var edgesIndexes = CesMemoryUtility.Allocate<RawArray<uint>>(length, allocator);

        for (int i = 0; i < length; i++)
        {
            edgesIndexes[i] = BinaryReadUtility.ReadRawArray<uint>(fileStream, allocator);
        }

        return new NodesFinals(length, geoCoord, owner, edgesIndexes, allocator);
    }

    public readonly struct NodesFinals : IDisposable
    {
        public readonly int Length;
        public readonly double2* GeoCoord;
        public readonly NodeOwner* Owner;
        public readonly RawArray<uint>* EdgesIndexes;

        readonly Allocator _allocator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NodesFinals(int length, double2* geoCoord, NodeOwner* owner, RawArray<uint>* edgesIndexes, Allocator allocator)
        {
            Length = length;
            GeoCoord = geoCoord;
            Owner = owner;
            EdgesIndexes = edgesIndexes;
            _allocator = allocator;
        }

        public void Dispose()
        {
            for (int i = 0; i < Length; i++)
            {
                EdgesIndexes[i].Dispose();
            }

            UnsafeUtility.Free(GeoCoord, _allocator);
            UnsafeUtility.Free(Owner, _allocator);
            UnsafeUtility.Free(EdgesIndexes, _allocator);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct NodeOwner
    {
        public readonly NodeOwnerType Type;
        public readonly uint Index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NodeOwner(NodeOwnerType type, uint index)
        {
            Type = type;
            Index = index;
        }
    }

    public enum NodeOwnerType : uint
    {
        Field = 0,
        River = 10
    }

    public static EdgesFinals LoadEdges(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        int length = fileStream.ReadValue<int>();

        var nodeIndexes = BinaryReadUtility.ReadArraySimple<uint2>(fileStream, length, allocator);
        var distanceGround = BinaryReadUtility.ReadArraySimple<double>(fileStream, length, allocator);
        var distanceAir = BinaryReadUtility.ReadArraySimple<double>(fileStream, length, allocator);
        var crossedRiverNodeIndex = BinaryReadUtility.ReadArraySimple<int>(fileStream, length, allocator);

        return new EdgesFinals(length, nodeIndexes, distanceGround, distanceAir, crossedRiverNodeIndex, allocator);
    }

    public readonly struct EdgesFinals : IDisposable
    {
        public readonly int Length;

        public readonly uint2* NodesIndexes;

        public readonly double* DistanceGround;
        public readonly double* DistanceAir;

        public readonly int* CrossedRiverPointIndex;

        readonly Allocator _allocator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EdgesFinals(int length, uint2* nodesIndexes, double* distanceGround, double* distanceAir, int* crossedRiverNodeIndex, Allocator allocator)
        {
            Length = length;
            NodesIndexes = nodesIndexes;
            DistanceGround = distanceGround;
            DistanceAir = distanceAir;
            CrossedRiverPointIndex = crossedRiverNodeIndex;
            _allocator = allocator;
        }

        public void Dispose()
        {
            UnsafeUtility.Free(NodesIndexes, _allocator);
            UnsafeUtility.Free(DistanceGround, _allocator);
            UnsafeUtility.Free(DistanceAir, _allocator);
            UnsafeUtility.Free(CrossedRiverPointIndex, _allocator);
        }
    }

    public static RawArray<uint> LoadFieldsNodesIndexes(string path, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<uint>(path, allocator);
    }

    public static RawArray<River> LoadRivers(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        int length = fileStream.ReadValue<int>();
        var rivers = new RawArray<River>(allocator, length);

        for (int i = 0; i < length; i++)
        {
            rivers[i] = new River
            {
                RiverPointsIndexes = BinaryReadUtility.ReadRawArray<uint>(in fileStream, allocator),
                PixelCoords = BinaryReadUtility.ReadRawArray<int2>(in fileStream, allocator),
            };
        }

        return rivers;
    }

    public struct River : IDisposable
    {
        public RawArray<uint> RiverPointsIndexes;
        public RawArray<int2> PixelCoords;

        public void Dispose()
        {
            RiverPointsIndexes.Dispose();
            PixelCoords.Dispose();
        }
    }

    public static RawArray<RiverPoint> LoadRiverPoints(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        int length = fileStream.ReadValue<int>();

        var riverPoints = new RawArray<RiverPoint>(allocator, length);

        for (int i = 0; i < riverPoints.Length; i++)
        {
            riverPoints[i] = new RiverPoint
            {
                RiverIndex = fileStream.ReadValue<uint>(),
                NodeIndex = fileStream.ReadValue<uint>(),
                StartsFromFieldIndex = fileStream.ReadValue<int>(),
                StartsFrom = BinaryReadUtility.ReadRawArray<uint>(fileStream, allocator),
                EndsInto = BinaryReadUtility.ReadRawArray<uint>(fileStream, allocator),
                NeighborFieldsIndexes = BinaryReadUtility.ReadRawArray<uint>(fileStream, allocator),
            };
        }

        return riverPoints;
    }

    public struct RiverPoint : IDisposable
    {
        public uint RiverIndex;
        public uint NodeIndex;

        public int StartsFromFieldIndex;

        public RawArray<uint> StartsFrom;
        public RawArray<uint> EndsInto;

        public RawArray<uint> NeighborFieldsIndexes;

        public void Dispose()
        {
            StartsFrom.Dispose();
            EndsInto.Dispose();
            NeighborFieldsIndexes.Dispose();
        }
    }

    public static RawArray<RawArray<uint>> LoadRiverPointsCatchments(string path, Allocator allocator)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        return BinaryReadUtility.ReadRawArrayOfRawArrays<uint>(fileStream, allocator);
    }

    public static RawArray<float> LoadFieldsElevations(string savePathFieldsLatitudes, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<float>(savePathFieldsLatitudes, allocator);
    }

    public static RawArray<RawArray<uint>> LoadEntities(string savePathEntities, Allocator allocator)
    {
        using var fileStream = new FileStream(savePathEntities, FileMode.Open, FileAccess.Read);

        return BinaryReadUtility.ReadRawArrayOfRawArrays<uint>(fileStream, allocator);
    }

    public static RawArray<float> LoadFieldsPops(string savePathFieldsPops, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<float>(savePathFieldsPops, allocator);
    }

    public static RawArray<LandForm> LoadFieldsLandForms(string savePathFieldsLandForms, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<LandForm>(savePathFieldsLandForms, allocator);
    }

    public enum LandForm : byte
    {
        None = 0,
        BedrockMountainSteepRough = 1,
        BedrockMountainSteepSmooth = 2,
        BedrockMountainModerateRough = 3,
        BedrockMountainModerateSmooth = 4,
        HillsRough = 5,
        HillsSmooth = 6,
        UpperLargeSlope = 7,
        MiddleLargeSlope = 8,
        DissectedTerraceModeratePlateau = 9,
        TerraceEdgeOrValleyBottomPlain = 10,
        TerraceOrSmoothPlateau = 11,
        AlluvialFanOrPediment = 12,
        UpstreamAlluvialPlain = 13,
        AlluvialOrCoastalPlain = 14,
        DeltaOrMarsh = 15
    }

    public static RawArray<FieldWeathers> LoadFieldsWeathers(string path, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<FieldWeathers>(path, allocator);
    }

    public struct FieldWeathers
    {
        public float Weather0;
        public float Weather1;
        public float Weather2;
        public float Weather3;
        public float Weather4;
        public float Weather5;
        public float Weather6;
        public float Weather7;
        public float Weather8;
        public float Weather9;
        public float Weather10;
        public float Weather11;
    }

    public static RawArray<SoilType> LoadSoilTypes(string path, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<SoilType>(path, allocator);
    }

    public enum SoilType : byte
    {
        Acrisols = 0,
        Albeluvisols = 1,
        Alisols = 2,
        Andosols = 3,
        Arenosols = 4,
        Calcisols = 5,
        Cambisols = 6,
        Chernozems = 7,
        Cryosols = 8,
        Durisols = 9,
        Ferralsols = 10,
        Fluvisols = 11,
        Gleysols = 12,
        Gypsisols = 13,
        Histosols = 14,
        Kastanozems = 15,
        Leptosols = 16,
        Lixisols = 17,
        Luvisols = 18,
        Nitisols = 19,
        Phaeozems = 20,
        Planosols = 21,
        Plinthosols = 22,
        Podzols = 23,
        Regosols = 24,
        Solonchaks = 25,
        Solonetz = 26,
        Stagnosols = 27,
        Umbrisols = 28,
        Vertisols = 29,

        None = 255,
    }

    public static RawArray<float> LoadFieldsSurfaces(string path, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<float>(path, allocator);
    }

    public static RawArray<FieldLandCoverParams> LoadFieldsLandCoverParams(string path, Allocator allocator)
    {
        return BinaryReadUtility.ReadRawArray<FieldLandCoverParams>(path, allocator);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FieldLandCoverParams
    {
        public float Wetness;
        public float Vegetation;
        public float Cultivation;
        public float Glaciation;
        public float Desertification;
        public float Buildings;
    }
}
