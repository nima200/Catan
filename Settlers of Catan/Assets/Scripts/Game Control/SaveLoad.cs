using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class SaveLoad : MonoBehaviour {

	private static SaveLoad saveload;

	//Will need to create Game Manager class
	private static List<GameManager> savedGames = new List<GameManager>();

	//will be persisting data in a gameobject
	//singleton design pattern to make sure only one instance of object can exist
	void Awake () {
		if(saveload == null){
			DontDestroyOnLoad(gameObject);
			saveload = this;
		}
		else if(saveload != this){
			Destroy(gameObject);
		}
	}
	
	public static void Save() {
	    savedGames.Add(GameManager.getCurrentGame());
	    BinaryFormatter bf = new BinaryFormatter();
	    FileStream file = File.Create (Application.persistentDataPath + "/savedGames.catan");
	    bf.Serialize(file, SaveLoad.savedGames);
	    file.Close();
	}

	public static void Load() {
	    if(File.Exists(Application.persistentDataPath + "/savedGames.catan")) {
	        BinaryFormatter bf = new BinaryFormatter();
	        FileStream file = File.Open(Application.persistentDataPath + "/savedGames.catan", FileMode.Open);
	        SaveLoad.savedGames = (List<GameManager>)bf.Deserialize(file);
	        file.Close();
	    }
	}
}
