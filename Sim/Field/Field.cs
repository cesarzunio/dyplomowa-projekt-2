using Ces.Collections;
using System.Runtime.InteropServices;

public struct Field
{
    public DatabaseId EntityId;

    public FieldLandCover LandCover;
    public FieldLandCoverParams LandCoverParams;
    public float WaterLevel;

    public RawSet<DatabaseId> PopsIds;
}

[StructLayout(LayoutKind.Sequential, Size = 32)]
public struct FieldLandCoverParams
{
    public float Wetness;
    public float Temperature;
    public float Vegetation;
    public float Cultivation;
    public float Glaciation;
    public float Desertification;
    public float Buildings;
}