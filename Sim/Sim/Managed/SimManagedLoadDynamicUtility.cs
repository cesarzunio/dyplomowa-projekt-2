using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static unsafe class SimManagedLoadDynamicUtility
{
    public static SimManaged LoadSim(string path)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var sim = new SimManaged();

        LoadEntities(in sim, in fileStream);

        return sim;
    }

    static void LoadEntities(in SimManaged sim, in FileStream fileStream)
    {
        int length = fileStream.ReadValue<int>();

        sim.Entities = BinaryReadUtility.ReadArrayManagedOfSerializables(in fileStream, length, &EntityManaged.Deserialize);
    }
}
