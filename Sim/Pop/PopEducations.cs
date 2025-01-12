using Ces.Collections;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct PopEducations
{
    public const int CAPACITY = 10;
    public fixed float EducationToLevel[CAPACITY];
}

public unsafe static class PopEducationsUtility
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref float EducationToLevel(this ref PopEducations popEducations, uint educationIndex)
    {
#if CES_COLLECTIONS_CHECK
        if (CesCollectionsUtility.IsOutOfRange(educationIndex, PopEducations.CAPACITY))
            throw new Exception($"PopEducationsUtility :: EducationToLevel :: Index ({educationIndex}) out of range ({PopEducations.CAPACITY})!");
#endif

        return ref popEducations.EducationToLevel[educationIndex];
    }
}

[Serializable]
public enum PopEducationType : uint
{
    Service = 0,
    Industry = 1,
    Agriculture = 2,
    Education = 3,
    Medicine = 4,
    Engineering = 5,
    Administration = 6,
    Finance = 7,
    Security = 8,
    Culture = 9,
}
