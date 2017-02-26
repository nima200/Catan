using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexVertex : MonoBehaviour
{
    public CornerUnitType[] States;

    public CornerUnit _type;

    public CornerUnit Type
    {
        get { return _type; }
        set
        {
            _type = value;
            foreach (var state in States)
            {
                if (state.Type != Type) continue;
                var meshes = state.GetComponentsInChildren<MeshFilter>();
                if (Type != CornerUnit.Disabled)
                {
                    var combine = new CombineInstance[meshes.Length];
                    int i = 0;
                    while (i < meshes.Length)
                    {
                        combine[i].mesh = meshes[i].sharedMesh;
                        combine[i].transform = meshes[i].transform.localToWorldMatrix;
                        i++;
                    }
                    gameObject.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
                    gameObject.GetComponent<Renderer>().material = state.GetComponentsInChildren<Renderer>()[0].sharedMaterial;
                }
                else
                {
                    gameObject.GetComponent<MeshFilter>().mesh = state.GetComponent<MeshFilter>().sharedMesh;
                }
                break;
            }
        }
    }

    public List<HexEdge> MyEdges;

    public Vector3 position;

	public int index;

	//keeps track of neighboring vertices
    public List<HexVertex> Neighbors;

	//all the Hexes that this vertex is associated with
	public HexCell[] hexAssociations;

	//add reference to vertex unit here (i.e settlement)

	//perhaps add reference to edge


	void Awake()
	{
		Neighbors = new List<HexVertex>();
		hexAssociations = new HexCell[3];
		index = 0;
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        MyEdges = new List<HexEdge>();
        Type = CornerUnit.Disabled;
    }

	void Start()
	{

		//change to false
		//gameObject.GetComponentsInChildren<MeshRenderer>().enabled = true;

//		foreach (MeshRenderer mrenderer in gameObject.GetComponentsInChildren<MeshRenderer>())
//		{
//			mrenderer.enabled = false;
//		}
        
	}

	void Update(){

    }


	//makes sphere "glow" if vertex is available for settlement placement
	public void displayAvailabilty(){
	}

	public void placeSettlement(){

		//change player reference here
	}

	//simple method to extract all Vector3's from an array of HexVertex
	public static Vector3[] convert(HexVertex[] vertices){
		Vector3[] array = new Vector3[vertices.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = vertices[i].position;
		}

		return array;
	}
}
