using UnityEngine;
using System.Collections.Generic;

public class CardIDGenerator : MonoBehaviour {

    public static List<string> ids = new List<string>();

    public static void generate()
    {
        int id = 0;
        string stringID;
        for (int i = 0; i < 5; i++)
        {
            int randomNumber = Random.Range(1, 10);
            id = id * 10 + randomNumber;
            
        }
        stringID = id.ToString();
        if (!ids.Contains(stringID))
        {
            ids.Add(stringID);
        } else
        {
            generate();
        }
        
    }
}
