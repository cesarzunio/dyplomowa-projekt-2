using Ces.Collections;

public struct Pop
{
    public uint FieldIndex;
    public float Amount;
    public PopClass Class;

    public PopDemographics Demographics;
    public PopEducations Educations;
    public RawSet<DatabaseId> Nationalities;
    public RawSet<DatabaseId> Religions;

    public RawSet<DatabaseId> WorkplaceEmploymentsIds;
}