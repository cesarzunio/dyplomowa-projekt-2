using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class FieldUtility
{
    const float TEMPERATURE_MAX = 40f;
    const float TEMPERATURE_MIN = -20f;

    const float HIGH = 0.75f;
    const float MEDIUM = 0.5f;
    const float LOW = 0.25f;

    public static FieldLandCover CalculateFieldLandCover(in FieldLandCoverParams landCoverParams)
    {
        if (landCoverParams.Buildings > HIGH)
            return FieldLandCover.UrbanDense;

        if (landCoverParams.Buildings > MEDIUM)
            return FieldLandCover.UrbanSparse;

        // -----

        if (landCoverParams.Desertification > MEDIUM)
            return FieldLandCover.Sand;

        if (landCoverParams.Glaciation > MEDIUM)
            return FieldLandCover.Ice;

        // -----

        if (landCoverParams.Cultivation > MEDIUM)
            return FieldLandCover.Cropland;

        // -----

        float temperatureRatio = math.unlerp(TEMPERATURE_MIN, TEMPERATURE_MAX, landCoverParams.Temperature);

        if (landCoverParams.Wetness > MEDIUM)
            return temperatureRatio > HIGH && landCoverParams.Vegetation > MEDIUM ? FieldLandCover.Mangrove : FieldLandCover.Wetland;

        if (landCoverParams.Vegetation > HIGH)
            return temperatureRatio > HIGH ? FieldLandCover.Jungle : FieldLandCover.Forest;

        if (landCoverParams.Vegetation > MEDIUM)
            return temperatureRatio > HIGH ? FieldLandCover.Herbaceous : FieldLandCover.Shrub;

        if (landCoverParams.Vegetation > LOW)
            return FieldLandCover.SparseVegetation;

        // -----

        return FieldLandCover.Rock;
    }
}
