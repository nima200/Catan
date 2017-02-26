using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEdge : MonoBehaviour
{

    public EdgeUnitType MyEdgeUnitType;
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
