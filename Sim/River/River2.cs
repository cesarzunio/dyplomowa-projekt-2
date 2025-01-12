using Ces.Collections;

public struct River2
{
    public bool Enabled;
    public RawArray<uint> PointNodeIndexes;
    public RawArray<RiverPointData> PointDatas;
    public RawArray<RawSet<DatabaseId>> GroundUnitsIds;
    public RiverConnections StartsFrom;
    public RiverConnections EndsInto;
}