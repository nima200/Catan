using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEdge : MonoBehaviour
{

    public EdgeUnitType MyEdgeUnitType;
    public HexCell FirstCell, SecondCell;
    public HexEdge[] NeighborEdges;

    public void SetNeighbor(EdgeDirection direction, HexEdge edge)
    {
        this.NeighborEdges[(int) direction] = edge;
        edge.NeighborEdges[(int) direction.Opposite()] = this;
    }

    public void SetNeighbor(int direction, HexEdge edge)
    {
        this.NeighborEdges[direction] = edge;
        edge.NeighborEdges[(direction < 2) ? direction + 2 : direction - 2] = this;
    }
}
