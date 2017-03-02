using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexEdge : MonoBehaviour
{ 
    public EdgeUnitType[] States = new EdgeUnitType[5];

    //every edge needs to be hidden when created
    private EdgeUnit _type = EdgeUnit.Hidden;

    //the two hexcells that the edge divides
    public HexCell FirstCell, SecondCell;
    public HexEdge[] NeighborEdges;
    public List<HexEdge> Neighbors = new List<HexEdge>();
    //head positions
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

    //this is in charge of making sure the right mesh is rendered based on edge state
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

    //the hexvertexes that lie on each end of the edge
    public List<HexVertex> Heads = new List<HexVertex>();
    public Player Owner { get; set; }


    //Helper functions for setting neighbors
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

    //********************************************//
    //           Getters and Setters              //
    //********************************************//

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
