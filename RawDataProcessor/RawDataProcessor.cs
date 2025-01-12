using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using Ces.Collections;

public sealed unsafe class RawDataProcessor : MonoBehaviour
{
    const Allocator ALLOCATOR = Allocator.Persistent;

    [Header("RawData Input")]
    [SerializeField] string _savePathAreas;
    [SerializeField] string _savePathFields;
    [SerializeField] string _savePathFieldsMap;
    [SerializeField] string _savePathBorders;
    [SerializeField] string _savePathBordersDistances;
    [SerializeField] string _savePathRiverPoints;
    [SerializeField] string _savePathRiverPointsCatchments;
    [SerializeField] string _savePathNodes;
    [SerializeField] string _savePathEdges;
    [SerializeField] string _savePathFieldsNodesIndexes;
    [SerializeField] string _savePathRivers;
    [SerializeField] string _savePathRiversNodes;
    [SerializeField] string _savePathFieldsLandCoverParams;
    [SerializeField] string _savePathFieldsElevations;
    [SerializeField] string _savePathEntities;
    [SerializeField] string _savePathFieldsPops;
    [SerializeField] string _savePathFieldsLandForms;
    [SerializeField] string _savePathFieldsSoils;
    [SerializeField] string _savePathFieldsSurfaces;
    [SerializeField] string _savePathFieldsTemperatures;
    [SerializeField] string _savePathFieldsRainfalls;

    [Header("Output")]
    [SerializeField] string _savePathSimPersistent;
    [SerializeField] string _savePathSimDynamic;
    [SerializeField] string _savePathSimManagedDynamic;

    public void CreateSimSavesFromRawData()
    {
        var areas = RawDataProcessorLoadUtility.LoadAreas(_savePathAreas, ALLOCATOR);
        var fields = RawDataProcessorLoadUtility.LoadFields(_savePathFields, ALLOCATOR);
        var fieldsMap = RawDataProcessorLoadUtility.LoadFieldsMap(_savePathFieldsMap, ALLOCATOR);
        var nodes = RawDataProcessorLoadUtility.LoadNodes(_savePathNodes, ALLOCATOR);
        var nodeEdges = RawDataProcessorLoadUtility.LoadEdges(_savePathEdges, ALLOCATOR);
        var riverPoints = RawDataProcessorLoadUtility.LoadRiverPoints(_savePathRiverPoints, ALLOCATOR);
        var riverPointsCatchments = RawDataProcessorLoadUtility.LoadRiverPointsCatchments(_savePathRiverPointsCatchments, ALLOCATOR);
        var fieldsNodesIndexes = RawDataProcessorLoadUtility.LoadFieldsNodesIndexes(_savePathFieldsNodesIndexes, ALLOCATOR);
        var rivers = RawDataProcessorLoadUtility.LoadRivers(_savePathRivers, ALLOCATOR);
        var fieldsLandCovers = RawDataProcessorLoadUtility.LoadFieldsLandCoverParams(_savePathFieldsLandCoverParams, ALLOCATOR);
        var fieldsElevations = RawDataProcessorLoadUtility.LoadFieldsElevations(_savePathFieldsElevations, ALLOCATOR);
        var entities = RawDataProcessorLoadUtility.LoadEntities(_savePathEntities, ALLOCATOR);
        var fieldsPops = RawDataProcessorLoadUtility.LoadFieldsPops(_savePathFieldsPops, ALLOCATOR);
        var fieldsLandForms = RawDataProcessorLoadUtility.LoadFieldsLandForms(_savePathFieldsLandForms, ALLOCATOR);
        var fieldsSoils = RawDataProcessorLoadUtility.LoadSoilTypes(_savePathFieldsSoils, ALLOCATOR);
        var fieldsSurfaces = RawDataProcessorLoadUtility.LoadFieldsSurfaces(_savePathFieldsSurfaces, ALLOCATOR);
        var fieldsTemperatures = RawDataProcessorLoadUtility.LoadFieldsWeathers(_savePathFieldsTemperatures, ALLOCATOR);
        var fieldsRainfalls = RawDataProcessorLoadUtility.LoadFieldsWeathers(_savePathFieldsRainfalls, ALLOCATOR);

        var sim = new Sim();
        var simManaged = new SimManaged();

        RawDataProcessorCreateUtility.FillFieldsMap(ref sim, fieldsMap, ALLOCATOR);
        RawDataProcessorCreateUtility.FillFields(
            ref sim, fields, fieldsNodesIndexes, fieldsElevations, fieldsLandForms, fieldsSoils,
            fieldsSurfaces, fieldsLandCovers, fieldsTemperatures, fieldsRainfalls, ALLOCATOR);
        RawDataProcessorCreateUtility.FillAreas(ref sim, areas, ALLOCATOR);
        RawDataProcessorCreateUtility.FillNodes(ref sim, nodes, ALLOCATOR);
        RawDataProcessorCreateUtility.FillEdges(ref sim, nodeEdges, ALLOCATOR);
        RawDataProcessorCreateUtility.FillRivers(ref sim, rivers, riverPoints, riverPointsCatchments, ALLOCATOR);
        RawDataProcessorCreateUtility.FillEntities(ref sim, entities, ALLOCATOR);
        RawDataProcessorCreateUtility.FillPops(ref sim, fieldsPops, ALLOCATOR);
        RawDataProcessorCreateUtility.InitializeOthers(ref sim, ALLOCATOR);

        RawDataProcessorCreateUtility.FillEntitiesManageds(in sim, in simManaged);

        areas.DisposeDeep();
        fields.Dispose();
        fieldsMap.Dispose();
        riverPoints.DisposeDeep();
        riverPointsCatchments.DisposeDeep();
        nodes.Dispose();
        nodeEdges.Dispose();
        fieldsNodesIndexes.Dispose();
        rivers.DisposeDeep();
        fieldsLandCovers.Dispose();
        fieldsElevations.Dispose();
        entities.DisposeDeep();
        fieldsPops.Dispose();
        fieldsLandForms.Dispose();
        fieldsSoils.Dispose();
        fieldsSurfaces.Dispose();
        fieldsTemperatures.Dispose();
        fieldsRainfalls.Dispose();

        SimSavePersistentUtility.SaveSim(in sim, _savePathSimPersistent);
        SimSaveDynamicUtility.SaveSim(in sim, _savePathSimDynamic);

        SimManagedSaveDynamicUtility.SaveSim(in simManaged, _savePathSimManagedDynamic);

        SimDisposeConstUtility.DisposeSim(ref sim);
    }
}

[CustomEditor(typeof(RawDataProcessor))]
public sealed class RawDataProcessorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var rdp = (RawDataProcessor)target;

        if (GUILayout.Button("Create sim saves from raw data"))
        {
            rdp.CreateSimSavesFromRawData();
        }
    }
}