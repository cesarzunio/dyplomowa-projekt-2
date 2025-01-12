using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct EntityFamilyDataEconomy
{
    public const ulong FLAG_NONE = 0ul;

    public const ulong FLAG_CAN_SET_TAX_PERSONAL = 1ul << 0;
    public const ulong FLAG_CAN_SET_TAX_CORPORATE = 1ul << 1;
    public const ulong FLAG_CAN_SET_TAX_VALUE = 1ul << 2;
    public const ulong FLAG_CAN_SET_TAX_PROPERTY = 1ul << 3;
    public const ulong FLAG_CAN_SET_TAX_EXCISE = 1ul << 4;
    public const ulong FLAG_CAN_SET_TAX_DUTY = 1ul << 5;
    public const ulong FLAG_CAN_SET_TAX_OTHER = 1ul << 6;

    public const ulong FLAG_OWN_CURRENCY = 0ul << 10;
    public const ulong FLAG_CAN_OWN_ASSETS = 0ul << 11;
    public const ulong FLAG_CAN_CREATE_ECONOMIC_ZONES = 0ul << 12;

    public ulong Flags;

    public float TaxSharePersonal;
    public float TaxShareCorporate;
    public float TaxShareValue;
    public float TaxShareProperty;
    public float TaxShareExcise;
    public float TaxShareDuty;
    public float TaxShareOther;
}

[StructLayout(LayoutKind.Sequential)]
public struct EntityFamilyDataAdministration
{
    public const ulong FLAG_NONE = 0ul;

    

    public ulong Flags;
}

[StructLayout(LayoutKind.Sequential)]
public struct EntityFamilyDataSecurity
{
    public const ulong FLAG_NONE = 0ul;

    public const ulong FLAG_OWN_POLICE = 1ul << 0;
    public const ulong FLAG_OWN_INTELLIGENCE = 1ul << 1;

    public ulong Flags;

    public EntityMilitaryArmamentLevelCap MilitaryArmamentLevelCap;
}

public enum EntityMilitaryArmamentLevelCap : uint
{

}

[StructLayout(LayoutKind.Sequential)]
public struct EntityFamilyDataPolitics
{
    public const ulong FLAG_NONE = 0ul;

    public const ulong FLAG_HAS_SECESSION_RIGHT = 1ul << 0;

    public ulong Flags;

    public EntityPoliticalSystemChangeAbility PoliticalSystemChangeAbility;
}

public enum EntityPoliticalSystemChangeAbility : uint
{
    All = 0,
    Minor = 10,
    None = 20,
}