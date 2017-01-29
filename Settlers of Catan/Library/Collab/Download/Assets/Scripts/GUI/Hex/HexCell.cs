﻿using UnityEngine;

public class HexCell : MonoBehaviour {

    // making each cell aware of its coordinates

    public HexCoordinates coordinates;

    public Color color;

    public int cellNumber;
    [SerializeField]
    HexCell[] neighbors;

    public HexCell GetNeighbor (HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public HexCell GetNeighbor(int index)
    {
        return neighbors[index];
    }

    public void SetNeighbor (HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
}