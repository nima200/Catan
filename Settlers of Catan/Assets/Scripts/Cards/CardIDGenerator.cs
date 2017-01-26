using UnityEngine;
using System.Collections.Generic;

public class CardIDGenerator : MonoBehaviour {

    // This list will contain a list of all card ID's.
    public static List<string> ids = new List<string>();

    // Recursive ID Generation method for cards. Pretty straightforward.
    // Creates a random 5 digit number and tries to add it to the list of ids, above.
    // If it doesn't exist, it adds it, if it does exist, it just recursively calls 
    // itself to generate another random number.
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
