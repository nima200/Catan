using UnityEngine;
using System.Collections.Generic;
using System;

public class HexMesh : MonoBehaviour {

    void Awake()
    {
        gameObject.name = "Hex Mesh";
    }

    public void Triangulate(HexCell[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Triangulate();
        }
    }
}
