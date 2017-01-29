using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum HexType { Wood, Ore, Brick, Sheep, Sea, Desert };
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour {

    // The actual mesh of the cell. Followed by the list of vertices and triangle indices 
    // for each cell. These are populated in the AddTriangle() method.
    Mesh cellMesh;
    List<Vector3> vertices;
    List<int> triangles;
    // Each cell needs to know its enum hex type -> the resources that the hex generates.
    public HexType myHexType;
    // Each cell needs to acquire a point in the axial coordiate system.
    // http://catlikecoding.com/unity/tutorials/hex-map-1/hexagonal-coordinates/cube-coordinates.png
    public HexCoordinates coordinates;
    // Need a renderer component to be able to access the material of the gameobject this
    // script is attached to. For coloring the hexes based on their resource type (HexType).
    public Renderer rend;
    // The value attached to each hex, as well as the Text element that displays this value. 
    // Randomly generated. 
    public int cellNumber;
    public Text label;
    // It was adviced by the tutorial I followed to serialize the neighbor connections of cells
    // so that they would survive recompiles. However I'm not entirely sure if we need this. Doesn't harm anyways.
    [SerializeField]
    HexCell[] neighbors;

    void TestFunctionForMerging()
    {
        // ha
        // ha
        // ha
        // LOL
    }

    void Awake()
    {
        // Once the script wakes up we essentially want to intialize all those undeclared
        // attributes we created above.

        // Linking the mesh of the MeshFilter component to be our cellMesh attribute and then initializing it.
        GetComponent<MeshFilter>().mesh = cellMesh = new Mesh();
        cellMesh.name = "Cell Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        // Adding a collider to the game object that holds this script.
        // This is needed for mouse interaction with the hexes.
        gameObject.AddComponent<MeshCollider>();
        // Mesh collider needs a mesh to feed into it so that it can adapt its shape/size/location/etc.
        GetComponent<MeshCollider>().sharedMesh = cellMesh;
        
    }

    // Getter Method for neighbors, replies back based on the direction given.
    public HexCell GetNeighbor (HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    // Overloading the getter method to be able to also access the neighbors with
    // an int index.
    public HexCell GetNeighbor(int index)
    {
        return neighbors[index];
    }

    // Setter for neighbors. It does the job for opposing directions together.
    // i.e. if I am at your east, you are at my west. It sets the link bothways!
    public void SetNeighbor (HexDirection direction, HexCell cell)
    {
        this.neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    // Method used from outside this class. It essentially initializes/creates the mesh
    // for the hex. 
    public void Triangulate()
    {
        // Start by clearing any old info that are stored in these arrays.
        // Adds functionality for retriangulating the cells in case needed.
        cellMesh.Clear();
        vertices.Clear();
        triangles.Clear();

        // The center vertex of each cell aligned to the center of the game object in the scene.
        Vector3 center = gameObject.transform.parent.localPosition;
        // 6 for 6 corners
        for (int i = 0; i < 6; i++)
        {
            // Using modulus for the i+1 index to prevent outofbounds so that it jumps back to the first point.
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[(i + 1) % 6]);

            // Converting our vertices and triangles lists that we have populated so far into arrays to assign
            // to the mesh.

            // https://docs.unity3d.com/ScriptReference/Mesh-vertices.html
            // https://docs.unity3d.com/ScriptReference/Mesh-triangles.html

            cellMesh.vertices = vertices.ToArray();
            cellMesh.triangles = triangles.ToArray();
        }
        // Need to recalculate surface normals so that the colors appear correct when rendered.
        cellMesh.RecalculateNormals();

    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // We need an index that fetches where we left off from the previous time this method was called.
        // Since you don't want to overlap the vertices previously made.
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        // A mesh's "triangles" array is just an index list. It holds the indices of the
        // three vertices that it should point to for the rendering engine to create a triangle out of
        // The indices automatically points to the mesh's "triangles" array.
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}
