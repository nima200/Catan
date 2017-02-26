using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVertex : MonoBehaviour{

	public bool occupied;

	public Vector3 position;

	public int index;

	//keeps track of neighboring vertices
	public HashSet<HexVertex> neighbors; 

	//all the Hexes that this vertex is associated with
	public HexCell[] hexAssociations;

	//add reference to vertex unit here (i.e settlement)

	//perhaps add reference to edge


	void Awake()
	{
		neighbors = new HashSet<HexVertex>();
		hexAssociations = new HexCell[3];
		index = 0;
	}

	void Start()
	{
		occupied = false;

		//change to false
		//gameObject.GetComponentsInChildren<MeshRenderer>().enabled = true;

		//foreach (MeshRenderer mrenderer in gameObject.GetComponentsInChildren<MeshRenderer>())
		//{
		//	mrenderer.enabled = false;
		//}
	}

	void Update(){
		if(occupied) { // make sure to add check to make sure that the reference to vertex unit is not null
			//determine level of settlement and display element accordingly
		}
	}


	//makes sphere "glow" if vertex is available for settlement placement
	public void displayAvailabilty(){
		gameObject.GetComponent<MeshRenderer>().enabled |= !occupied;
	}

	public void placeSettlement(){
		occupied = true;

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

	public static void assignNeighbors()
	{	
		//look at all the unique HexVertex
		foreach (HexVertex vertex in HexGrid.vertexPositions)
		{
			//grab the current vertex
			HexVertex current = vertex;



		}
	}
		


}
