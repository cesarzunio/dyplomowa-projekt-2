using Ces.Collections;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

public struct Entity
{
    public Color32 MapColor;
    public EntityDisplayData DisplayData;

    public DatabaseId Id;

    public EntityFamilyIds FamilyIds;
    public EntityFamilyDataEconomy FamilyDataEconomy;
    public EntityFamilyDataAdministration FamilyDataAdministration;
    public EntityFamilyDataSecurity FamilyDataSecurity;
    public EntityFamilyDataPolitics FamilyDataPolitics;
}

[StructLayout(LayoutKind.Sequential)]
public struct EntityDisplayData
{
    public FixedString64Bytes NameFull;
    public FixedString32Bytes NameShort;

    public Color32 MapColor;
}