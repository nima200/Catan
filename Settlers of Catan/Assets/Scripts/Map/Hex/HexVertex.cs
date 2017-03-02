using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexVertex : MonoBehaviour
{
    public CornerUnitType[] States;
    [SerializeField]
    private CornerUnit _type = CornerUnit.Hidden;

    //makes sure that the right mesh is rendered based on HexVertex state
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
                if (Type != CornerUnit.Disabled && Type != CornerUnit.Hidden)
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
    public List<HexEdge> NeighborEdges = new List<HexEdge>();
    public Vector3 Position;
    public int Index = 0;

    //keeps track of neighboring vertices
    public List<HexVertex> Neighbors = new List<HexVertex>();
    public Player Owner { get; set; }

    private void Awake()
    {
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
    }
}
