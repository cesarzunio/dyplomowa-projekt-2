using Ces.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public struct EntityFamilyIds
{
    public DatabaseId ParentId;
    public RawSet<DatabaseId> ChildrenIds;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Serialize(in FileStream fileStream, in EntityFamilyIds familyIds)
    {
        fileStream.WriteValue(familyIds.ParentId);
        fileStream.WriteValue(familyIds.ChildrenIds.Count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityFamilyIds Deserialize(in FileStream fileStream, Allocator allocator, int capacityIfEmpty)
    {
        var parentId = fileStream.ReadValue<DatabaseId>();
        int childrenCapacity = math.max(fileStream.ReadValue<int>(), capacityIfEmpty);

        return new EntityFamilyIds
        {
            ParentId = parentId,
            ChildrenIds = new RawSet<DatabaseId>(allocator, childrenCapacity),
        };
    }
}