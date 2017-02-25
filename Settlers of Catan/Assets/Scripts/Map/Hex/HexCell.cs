using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public enum HexType { Wood, Ore, Brick, Sheep, Sea, Desert };
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : MonoBehaviour {

    // The actual mesh of the cell. Followed by the list of vertices and triangle indices 
    // for each cell. These are populated in the AddTriangle() method.
    Mesh _cellMesh;
	public List<Vector3> Vertices;
    List<int> _triangles;
    // Each cell needs to know its enum hex type -> the resources that the hex generates.
    public HexType MyHexType;
    // Each cell needs to acquire a point in the axial coordiate system.
    // http://catlikecoding.com/unity/tutorials/hex-map-1/hexagonal-coordinates/cube-coordinates.png
    public HexCoordinates Coordinates;
    // Need a renderer component to be able to access the material of the gameobject this
    // script is attached to. For coloring the hexes based on their resource type (HexType).
    public Renderer Rend;
    // The value attached to each hex, as well as the Text element that displays this value. 
    // Randomly generated. 
    public int CellNumber;
    public Text Label;
    // It was adviced by the tutorial I followed to serialize the neighbor connections of cells
    // so that they would survive recompiles. However I'm not entirely sure if we need this. Doesn't harm anyways.
    [SerializeField]
    HexCell[] _neighbors;
    [SerializeField]
    public HexEdge[] MyEdges;
    public HexEdge[] PossibleEdges;

	public HexVertex CenterVertex;
	public HexVertex[] HexVertices;
	public HashSet<Vector3> GlobalVertices;


    private void Awake()
    {
        // Once the script wakes up we essentially want to intialize all those undeclared
        // attributes we created above.

        // Linking the mesh of the MeshFilter component to be our cellMesh attribute and then initializing it.
        GetComponent<MeshFilter>().mesh = _cellMesh = new Mesh();
        _cellMesh.name = "Cell Mesh";
        _neighbors = new HexCell[6];
        Vertices = new List<Vector3>();
        _triangles = new List<int>();
        Rend = GetComponent<Renderer>();
        MyEdges = new HexEdge[6];
        PossibleEdges = new HexEdge[6];
		HexVertices = new HexVertex[6];
		GlobalVertices = new HashSet<Vector3>();
    }

    private void Start()
    {
        // Adding a collider to the game object that holds this script.
        // This is needed for mouse interaction with the hexes.
        gameObject.AddComponent<MeshCollider>();
        // Mesh collider needs a mesh to feed into it so that it can adapt its shape/size/location/etc.
        GetComponent<MeshCollider>().sharedMesh = _cellMesh;
		        
    }

    // Getter Method for neighbors, replies back based on the direction_int given.
    public HexCell GetNeighbor (HexDirection direction)
    {
        return _neighbors[(int)direction];
    }

    public HexCell GetNeighbor_Opposite(HexDirection direction)
    {
        return ((int) direction < 3) ? _neighbors[(int) direction + 3] : _neighbors[(int) direction - 3];
    }

    public HexCell GetNeighbor_Opposite(int direction)
    {
        return (direction < 3) ? _neighbors[direction + 3] : _neighbors[direction - 3];
    }

    // Overloading the getter method to be able to also access the neighbors with
    // an int direction_int.
    public HexCell GetNeighbor(int index)
    {
        return _neighbors[index];
    }

    // Setter for neighbors. It does the job for opposing directions together.
    // i.e. if I am at your east, you are at my west. It sets the link bothways!
    public void SetNeighbor (HexDirection direction, HexCell cell)
    {
        _neighbors[(int)direction] = cell;
        cell._neighbors[(int)direction.Opposite()] = this;
    }

    public HexEdge GetEdge(HexDirection direction)
    {
        return MyEdges[(int)direction];
    }

    public HexEdge GetEdge(int index)
    {
        return MyEdges[index];
    }

    public void SetEdge(int directionInt, HexEdge edge)
    {
        var direction = (HexDirection) directionInt;
        if (MyEdges[directionInt] == null)
        {
            MyEdges[directionInt] = edge;
            if (_neighbors[directionInt] != null && _neighbors[directionInt].GetEdge(direction.Opposite()) == null)
            {
                _neighbors[directionInt].SetEdge(direction.Opposite(), edge);
            }
        }
        else
        {
            Debug.Log("Cell already has an edge placed there!");
        }

    }

    public void SetEdge(HexDirection direction, HexEdge edge)
    {
        if (MyEdges[(int) direction] == null)
        {
            MyEdges[(int) direction] = edge;
            if (_neighbors[(int) direction] != null && _neighbors[(int) direction].GetEdge(direction.Opposite()) == null)
            {
                _neighbors[(int) direction].SetEdge(direction.Opposite(), edge);
            }
        }
        else
        {
            Debug.Log("Cell already has an edge placed there!");
        }

    }

    // Method used from outside this class. It essentially initializes/creates the mesh
    // for the hex. 
    public void Triangulate()
    {
        // Start by clearing any old info that are stored in these arrays.
        // Adds functionality for retriangulating the cells in case needed.
        _cellMesh.Clear();
        Vertices.Clear();
        _triangles.Clear();

        // The center vertex of each cell aligned to the center of the game object in the scene.
        Vector3 center = gameObject.transform.parent.localPosition;

		Vector3 globalCenter = gameObject.transform.position;

        // Please don't spam my console log Jimmy.
		// Debug.Log("center: " + center + " globalcenter: " + globalCenter);
        
		// 6 for 6 corners
        for (int i = 0; i < 6; i++)
        {
            // Using modulus for the i+1 direction_int to prevent outofbounds so that it jumps back to the first point.
			AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[(i + 1) % 6]);


			// denote the unique positions of the cell's vertices
			HexGrid.Positions.Add(globalCenter + HexMetrics.corners[i]);
			HexGrid.Positions.Add(globalCenter + HexMetrics.corners[(i + 1) % 6]);

			GlobalVertices.Add(globalCenter + HexMetrics.corners[i]);
			GlobalVertices.Add(globalCenter + HexMetrics.corners[(i + 1) % 6]);

            // Converting our vertices and triangles lists that we have populated so far into arrays to assign
            // to the mesh.

            // https://docs.unity3d.com/ScriptReference/Mesh-vertices.html
            // https://docs.unity3d.com/ScriptReference/Mesh-triangles.html

			_cellMesh.vertices = Vertices.ToArray();
            _cellMesh.triangles = _triangles.ToArray();
        }
        // Need to recalculate surface normals so that the colors appear correct when rendered.
        _cellMesh.RecalculateNormals();

    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // We need an direction_int that fetches where we left off from the previous time this method was called.
        // Since you don't want to overlap the vertices previously made.
        int vertexIndex = Vertices.Count;	
		Vertices.Add(v1);
		Vertices.Add(v2);
		Vertices.Add(v3); 


        // A mesh's "triangles" array is just an direction_int list. It holds the indices of the
        // three vertices that it should point to for the rendering engine to create a triangle out of
        // The indices automatically points to the mesh's "triangles" array.
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }

    public bool HasEdgeAtPosition(Vector3 position)
    {
        return MyEdges.Where(edge => edge != null).Any(edge => edge.GetPosition_FC() == position);
    }

    public bool HasPossibleEdgeAtPosition(Vector3 position)
    {
        return PossibleEdges.Where(edge => edge != null).Any(edge => edge.GetPosition_FC() == position);
    }

    public bool HasEdgeAtDirection(HexDirection direction)
    {
        return (MyEdges[(int) direction] != null);
    }

    public bool HasEdgeAtDirection(int direction)
    {
        return (MyEdges[direction] != null);
    }

    public bool HasPossibleEdgeAtDirection(HexDirection direction)
    {
        return (PossibleEdges[(int) direction] != null);
    }

    public bool HasPossibleEdgeAtDirection(int direction)
    {
        return (PossibleEdges[direction] != null);
    }
}
