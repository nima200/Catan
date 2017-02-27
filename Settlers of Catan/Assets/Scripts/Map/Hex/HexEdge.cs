﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexEdge : MonoBehaviour
{ 
    public EdgeUnitType[] States = new EdgeUnitType[4];
    private EdgeUnit _type = EdgeUnit.Disabled;
    public HexCell FirstCell, SecondCell;
    public HexEdge[] NeighborEdges;
    public List<HexEdge> Neighbors = new List<HexEdge>();
    public List<HexEdge> PossibleNeighbors = new List<HexEdge>();
    private Vector3 _positionFc;
    private Vector3 _positionSc;
    private bool _isActual = true;
    public bool IsActual
    {
        get
        {
            return _isActual;
        }
        set
        {
            _isActual = value;
        }
    }
    public EdgeUnit Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            foreach (var state in States)
            {
                if (state.Unit != value) continue;
                gameObject.GetComponent<MeshFilter>().mesh = state.GetComponent<MeshFilter>().sharedMesh;
                gameObject.GetComponent<Renderer>().material = state.GetComponent<Renderer>().sharedMaterial;
                gameObject.transform.localScale = state.transform.localScale;
                break;
            }
        }
    }

    public HexVertex[] MyVertices = new HexVertex[2]; 

    public void SetNeighbor(EdgeDirection direction, HexEdge edge)
    {
        NeighborEdges[(int) direction] = edge;
        edge.NeighborEdges[(int) direction.Opposite()] = this;
    }

    public void SetNeighbor(int direction, HexEdge edge)
    {
        NeighborEdges[direction] = edge;
        edge.NeighborEdges[(direction < 2) ? direction + 2 : direction - 2] = this;
    }

    public Vector3 GetPosition_FC()
    {
        return _positionFc;
    }

    public void SetPosition_FC(Vector3 position)
    {
        _positionFc = position;
    }
    public Vector3 GetPosition_SC()
    {
        return _positionSc;
    }

    public void SetPosition_SC(Vector3 position)
    {
        _positionSc = position;
    }

    
}
