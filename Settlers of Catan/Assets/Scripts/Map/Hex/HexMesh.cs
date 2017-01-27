using UnityEngine;
using System.Collections.Generic;


public class HexMesh : MonoBehaviour {

    //Mesh hexMesh;
    //List<Vector3> vertices;
    //List<int> triangles;
    //List<Color> colors;
    //MeshCollider meshCollider;

    void Awake()
    {
        //GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        //meshCollider = gameObject.AddComponent<MeshCollider>();
        //hexMesh.name = "Hex Mesh";
        gameObject.name = "Hex Mesh";
        //vertices = new List<Vector3>();
        //colors = new List<Color>();
        //triangles = new List<int>();
    }


    
    public void Triangulate(HexCell[] cells)
    {
        // first clearing any old data that might have been in the mesh data structure
        //hexMesh.Clear();
        //vertices.Clear();
        //triangles.Clear();
        //colors.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            // triangulating each cell
            cells[i].Triangulate();
        }
        //hexMesh.vertices = vertices.ToArray();
        //hexMesh.colors = colors.ToArray();
        //hexMesh.triangles = triangles.ToArray();
        //hexMesh.RecalculateNormals();
        //meshCollider.sharedMesh = hexMesh;
    }

    //void Triangulate(HexCell cell)
    //{
    //    // the center vertex, i.e. the first vertex, of all triangles will be the center of the cell.
    //    Vector3 center = cell.transform.localPosition;
    //    for (int i = 0; i < 6; i++)
    //    {
    //        AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i+1]);
    //        AddTriangleColor(cell.color);
            
    //    }
    //}

    // the actual triangle generating method that takes in 3 vectors and makes a triangle out of them, 
    // assigning the relevant vertices and triangle indices.
    //void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    //{
    //    int vertexIndex = vertices.Count;
    //    vertices.Add(v1);
    //    vertices.Add(v2);
    //    vertices.Add(v3);
    //    triangles.Add(vertexIndex);
    //    triangles.Add(vertexIndex + 1);
    //    triangles.Add(vertexIndex + 2);
    //}

    //void AddTriangleColor(Color color)
    //{
    //    colors.Add(color);
    //    colors.Add(color);
    //    colors.Add(color);
    //}
}
