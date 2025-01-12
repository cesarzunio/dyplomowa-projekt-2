using Ces.Collections;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

namespace Ces.Collections
{
    public static unsafe class DatabaseSerializeUtility
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteDatabaseMulTablesCounts<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable* databaseTables, int tablesAmount)
            where TDatabaseTable : unmanaged, IDatabaseTable
        {
            var savesTable = stackalloc int[tablesAmount];

            for (int i = 0; i < tablesAmount; i++)
            {
                savesTable[i] = databaseTables[i].GetCount();
            }

            BinarySaveUtility.WriteArraySimple(in fileStream, savesTable, tablesAmount);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteDatabaseMulTables<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable* databaseTables, int tablesAmount)
            where TDatabaseTable : unmanaged, IDatabaseTable
        {
            for (int i = 0; i < tablesAmount; i++)
            {
                BinarySaveUtility.WriteArraySimple(in fileStream, databaseTables[i].GetIndexToId(), databaseTables[i].GetCount());
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ReadDatabaseMulTables<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable* databaseTables, int tablesAmount)
            where TDatabaseTable : unmanaged, IDatabaseTable
        {
            for (int i = 0; i < tablesAmount; i++)
            {
                BinaryReadUtility.ReadArraySimple(in fileStream, databaseTables[i].GetCount(), databaseTables[i].GetIndexToId());
            }
        }


















        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseIdsSize<TDatabaseIdData>(in FileStream fileStream, in TDatabaseIdData databaseIdData)
        //    where TDatabaseIdData : unmanaged, IDatabaseIdData
        //{
        //    fileStream.WriteValue(databaseIdData.GetCapacity());
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseMulIdsSize<TDatabaseMulIdData>(in FileStream fileStream, in TDatabaseMulIdData databaseMulIdData)
        //    where TDatabaseMulIdData : unmanaged, IDatabaseMulIdData
        //{
        //    fileStream.WriteValue(databaseMulIdData.GetCapacity());
        //}

        //// --------------------

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseIds<TDatabaseIdData>(in FileStream fileStream, in TDatabaseIdData databaseIdData)
        //    where TDatabaseIdData : unmanaged, IDatabaseIdData
        //{
        //    int capacity = databaseIdData.GetCapacity();

        //    BinarySaveUtility.WriteArraySimple(in fileStream, databaseIdData.GetIdToIndex(), capacity);
        //    BinarySaveUtility.WriteArraySimple(in fileStream, databaseIdData.GetIdToUseCount(), capacity);
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseMulIds<TDatabaseMulIdData>(in FileStream fileStream, in TDatabaseMulIdData databaseMulIdData)
        //    where TDatabaseMulIdData : unmanaged, IDatabaseMulIdData
        //{
        //    int capacity = databaseMulIdData.GetCapacity();

        //    BinarySaveUtility.WriteArraySimple(in fileStream, databaseMulIdData.GetIdToIndex(), capacity);
        //    BinarySaveUtility.WriteArraySimple(in fileStream, databaseMulIdData.GetIdToUseCount(), capacity);
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void DeserializeDatabaseIds<TDatabaseIdData>(in FileStream fileStream, in TDatabaseIdData databaseMulIdData, int capacityId)
        //    where TDatabaseIdData : unmanaged, IDatabaseIdData
        //{
        //    BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, databaseMulIdData.GetIdToIndex());
        //    BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, databaseMulIdData.GetIdToUseCount());
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void DeserializeDatabaseMulIds<TDatabaseMulIdData>(in FileStream fileStream, in TDatabaseMulIdData databaseMulIdData, int capacityId)
        //    where TDatabaseMulIdData : unmanaged, IDatabaseMulIdData
        //{
        //    BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, databaseMulIdData.GetIdToIndex());
        //    BinaryReadUtility.ReadArraySimple(in fileStream, capacityId, databaseMulIdData.GetIdToUseCount());
        //}

        //// --------------------

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseTableSize<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable databaseTable)
        //    where TDatabaseTable : unmanaged, IDatabaseTable
        //{
        //    fileStream.WriteValue(databaseTable.GetCount());
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseMulTablesSizes<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable* databaseTables, int tablesCount)
        //    where TDatabaseTable : unmanaged, IDatabaseTable
        //{
        //    var savesTable = stackalloc int[tablesCount];

        //    for (int i = 0; i < tablesCount; i++)
        //    {
        //        savesTable[i] = databaseTables[i].GetCount();
        //    }

        //    BinarySaveUtility.WriteArraySimple(in fileStream, savesTable, tablesCount);
        //}

        //// --------------------

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseTable<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable databaseTable)
        //    where TDatabaseTable : unmanaged, IDatabaseTable
        //{
        //    BinarySaveUtility.WriteArraySimple(in fileStream, databaseTable.GetIndexToId(), databaseTable.GetCount());
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void SerializeDatabaseMulTables<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable* databaseTables, int tablesCount)
        //    where TDatabaseTable : unmanaged, IDatabaseTable
        //{
        //    for (int i = 0; i < tablesCount; i++)
        //    {
        //        SerializeDatabaseTable(in fileStream, in databaseTables[i]);
        //    }
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void DeserializeDatabaseTable<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable databaseTable, int capacityTable)
        //    where TDatabaseTable : unmanaged, IDatabaseTable
        //{
        //    BinaryReadUtility.ReadArraySimple(in fileStream, capacityTable, databaseTable.GetIndexToId());
        //}

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public static void DeserializeDatabaseMulTables<TDatabaseTable>(in FileStream fileStream, in TDatabaseTable* databaseTables, int* capacitiesOfTables, int tablesCount)
        //    where TDatabaseTable : unmanaged, IDatabaseTable
        //{
        //    for (int i = 0; i < tablesCount; i++)
        //    {
        //        DeserializeDatabaseTable(in fileStream, in databaseTables[i], capacitiesOfTables[i]);
        //    }
        //}
    }

    public unsafe struct DatabaseIdDataSave
    {
        public int Capacity;
        public Allocator Allocator;
        public DatabaseIndex* IdToIndex;
        public uint* IdToUseCount;
    }

    public unsafe struct DatabaseMulIdDataSave
    {
        public int Capacity;
        public Allocator Allocator;
        public DatabaseMulIndex* IdToIndex;
        public uint* IdToUseCount;
    }

    public unsafe struct DatabaseTableSave
    {
        public int Count;
        public Allocator Allocator;
        public DatabaseId* IndexToId;
    }
}