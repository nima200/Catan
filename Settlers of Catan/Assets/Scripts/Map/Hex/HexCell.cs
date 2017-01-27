using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum HexType { Wood, Ore, Brick, Sheep, Sea, Desert };
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour {

    Mesh cellMesh;
    List<Vector3> vertices;
    List<int> triangles;
    // making each cell aware of its coordinates
    public HexType myHexType;
    public HexCoordinates coordinates;
    List<Color> colors;
    public Color color;
    public Renderer rend;
    public int cellNumber;
    public Text label;
    [SerializeField]
    HexCell[] neighbors;

    void Awake()
    {
        
        GetComponent<MeshFilter>().mesh = cellMesh = new Mesh();
        cellMesh.name = "Cell Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        gameObject.AddComponent<MeshCollider>();
        GetComponent<MeshCollider>().sharedMesh = cellMesh;
        
    }

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
        this.neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void Triangulate()
    {
        cellMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        Vector3 center = gameObject.transform.parent.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i + 1]);
            

            cellMesh.vertices = vertices.ToArray();
            cellMesh.triangles = triangles.ToArray();
        }
        cellMesh.RecalculateNormals();

    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}
