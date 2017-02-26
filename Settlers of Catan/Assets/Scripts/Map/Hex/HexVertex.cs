using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexVertex : MonoBehaviour
{
    public CornerUnitType[] States;
    private CornerUnit _type;
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
    public Vector3 Position;
	public int Index;
	//keeps track of neighboring vertices
    public List<HexVertex> Neighbors;

	private void Awake()
	{
		Neighbors = new List<HexVertex>();
		Index = 0;
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        MyEdges = new List<HexEdge>();
        Type = CornerUnit.Disabled;
    }
}
