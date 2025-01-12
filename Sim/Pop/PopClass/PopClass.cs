using System;

[Serializable]
public enum PopClass : uint
{
    // no-skilled
    UnskilledLaborers = 0,

    Peasants = 10,
    Slaves = 11,

    // low-skilled
    IndustrialLaborers = 20,
    AgriculturalLaborers = 21,
    OfficeLaborers = 22,
    ServiceLaborers = 23,

    // medium-skilled
    MedicalWorkers = 40,
    EngineeringWorkers = 41,
    AdministrationWorkers = 42,
    ScienceWorkers = 43,

    // high-skilled
    MedicalSpecialists = 60,
    EngineeringSpecialists = 61,
    AdministrationSpecialists = 62,
    ScienceSpecialists = 63,

    // owners
    Capitalists = 81,
    Aristocrats = 82,

    // security
    LawEnforcement = 100,
    ArmedForces = 101,

    // other
    Clergy = 121,
    CultureWorkers = 122,

    // none
    NONE = PopClassDataSerialized.POP_CLASS_CAPACITY,
}
