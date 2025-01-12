using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ces.Collections;

public struct Sim
{
    public FieldsMap FieldsMap;

    //public SimProcessorsData ProcessorsData;

    public DatabaseStatic<FieldConst, FieldConstColumns> FieldsConst;
    public DatabaseStatic<Field, FieldColumns> Fields;

    public DatabaseStatic<AreaConst, AreaConstColumns> AreasConst;

    public DatabaseStatic<RiverConst, RiverConstColumns> RiversConst;

    public DatabaseStatic<RiverPointConst, RiverPointConstColumns> RiverPointsConst;
    public DatabaseStatic<RiverPoint, RiverPointColumns> RiverPoints;

    public DatabaseStatic<NodeConst, NodeConstColumns> NodesConst;
    public DatabaseStatic<Node, NodeColumns> Nodes;

    public DatabaseStatic<EdgeConst, EdgeColumns> EdgesConst;

    //public Database<Entity, EntityColumns> Entities;
    public DatabaseLookup<Entity, EntityColumns, EntityRelations> Entities;

    public Database<Pop, PopColumns> Pops;

    public DatabaseMul<GroundUnit, GroundUnitColumns> GroundUnits;
}
