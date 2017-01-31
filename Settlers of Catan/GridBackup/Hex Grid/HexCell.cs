using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour {
    Mesh cellMesh;
    List<Vector3> vertices;
    List<int> triangle;
    public HexCoordinates coordinates;
    public Renderer rend;
    

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = cellMesh = new Mesh();
        cellMesh.name = "Cell Mesh";
        vertices = new List<Vector3>();
        triangle = new List<int>();
    }
    void Start()
    {
        gameObject.AddComponent<MeshCollider>();
        GetComponent<MeshCollider>().sharedMesh = cellMesh;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            TouchCell(hit.point);
        }
    }

    void TouchCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        Debug.Log(coordinates.ToString());
        rend = gameObject.GetComponent<Renderer>();
        rend.material.color = Color.red;
    }
    public void Triangulate()
    {
        cellMesh.Clear();
        vertices.Clear();
        triangle.Clear();
        Vector3 center = gameObject.transform.parent.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i+1]);
            
            
            cellMesh.vertices = vertices.ToArray();
            cellMesh.triangles = triangle.ToArray();
        }
        cellMesh.RecalculateNormals();
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangle.Add(vertexIndex);
        triangle.Add(vertexIndex + 1);
        triangle.Add(vertexIndex + 2);
    }
    // UNDER CONSTRUCTION
    //void AddLight()
    //{
    //    for (int i = 0; i < HexMetrics.corners.Length; i++) {
    //        GameObject cornerIndicator = new GameObject("CornerIndicator Light");
    //        Light cornerLight = cornerIndicator.AddComponent<Light>();
    //        cornerLight.color = Color.red;
    //        cornerLight.range = 5;
    //        cornerLight.intensity = 8;
    //        cornerIndicator.transform.position = HexMetrics.corners[i];
    //        cornerIndicator.transform.Translate(0, 0.5f, 0, Space.Self);
    //    }
        
    //}
}
