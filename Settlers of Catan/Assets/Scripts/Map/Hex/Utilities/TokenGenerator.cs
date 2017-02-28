using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TokenGenerator : MonoBehaviour {

    static List<int> allNums;
    public static int length;
    public static int[] tokens;

    // Very generic method that simply gives out an array of ints from
    // 1 to the number passed to it as length.

    // The way it works is that we place all numbers in a row in a list,
    // and then fill out an array by picking random indices into the list,
    // and once we add the number at the random index to the array, we delete
    // it off the list of all numbers to not get duplicates.
    public static int[] generate(int length)
    {
        allNums = new List<int>();
        for (int i = 1; i <= length; i++)
        {
            allNums.Add(i);
        }
        int[] tokens = new int[length];
        
        for (int i = 0; i < length; i++)
        {
            int index = Random.Range(0, allNums.Count - 1);
            tokens[i] = allNums[index];
            allNums.RemoveAt(index);
        }
        return tokens;
    }
}
