using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVertex {

	public bool occupied;

	public Vector3 position;

	//keeps track of neighboring vertices
	public List<HexVertex> neighbors = new List<HexVertex>();

	//all the Hexes that this vertex is associated with
	public HexCell[] hexAssociations;

	//add reference to vertex unit here (i.e settlement)

	//perhaps add reference to edge

	//this sphere is purely for "glowing" purposes
	public GameObject sphere;

	//void Update(){
	//	if(occupied) { // make sure to add check to make sure that the reference to vertex unit is not null
	//		//determine level of settlement and display element accordingly
	//	}
	//}

	//constructor for taking in x,y,z positions and creates vector3 
	public HexVertex(float x, float y, float z){
		//create the vector position
		position = new Vector3(x,y,z);

		occupied = false;

		//create a sphere and assign it to the position of the vertex

	}

	//constructor that takes in pre-existing vector3
	public HexVertex(Vector3 position)
	{
		//create the vector position
		this.position = position;

		occupied = false;

		//create the sphere and assign to the position of the vertex
		sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = position;
		sphere.GetComponent<MeshRenderer>().enabled = true; //change back to false
	}

	//makes sphere "glow" if vertex is available for settlement placement
	public void displayAvailabilty(){
		sphere.GetComponent<MeshRenderer>().enabled |= !occupied;
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
		


}
