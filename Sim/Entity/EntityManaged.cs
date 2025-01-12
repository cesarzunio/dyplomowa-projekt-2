using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

public struct EntityManaged
{
    public bool Enabled;

    public string NameFull;
    public string NameShort;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in EntityManaged entity)
    {
        fileStream.WriteValue(entity.Enabled);

        if (!entity.Enabled)
            return;

        fileStream.WriteString(entity.NameFull);
        fileStream.WriteString(entity.NameShort);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityManaged Deserialize(in FileStream fileStream)
    {
        bool enabled = fileStream.ReadValue<bool>();

        if (!enabled)
        {
            return new EntityManaged
            {
                Enabled = false,
                NameFull = "NULL_NAME",
                NameShort = "NULL",
            };
        }

        return new EntityManaged
        {
            Enabled = true,
            NameFull = fileStream.ReadString(),
            NameShort = fileStream.ReadString(),
        };
    }
}
