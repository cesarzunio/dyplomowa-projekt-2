using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static unsafe class SimManagedSaveDynamicUtility
{
    public static void SaveSim(in SimManaged sim, string path)
    {
        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

        SaveEntities(in sim, fileStream);
    }

    static void SaveEntities(in SimManaged sim, in FileStream fileStream)
    {
        fileStream.WriteValue(sim.Entities.Length);

        BinarySaveUtility.WriteArrayManagedOfSerializables(in fileStream, sim.Entities, &EntityManaged.Serialize);
    }
}
