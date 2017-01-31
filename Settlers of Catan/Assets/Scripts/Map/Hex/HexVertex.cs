using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexVertex : MonoBehaviour {

	public bool occupied;

	Vector3 position;

	//add reference to vertex unit here

	//this sphere is purely for "glowing" purposes
	GameObject sphere;

	void Update(){
		if(occupied) { // make sure to add cheeck that reference to vertex unit is not null
			//determine level of settlement and display element accordingly
		}
	}


	public HexVertex(float x, float y, float z){
		//create the vector position
		position = new Vector3(x,y,z);

		occupied = false;

		//create the sphere and assign to the position of the vertex
		sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
		sphere.transform.position = temp;
		sphere.GetComponent<MeshRenderer>().enabled = false;
	}

	public void displayAvailabilty(){
		if(!occupied){
			sphere.GetComponent<MeshRenderer>().enabled = true;

			//assign shader here
		}
	}

	public void placeSettlement(){
		occupied = true;

		//change player reference here
	}

}
