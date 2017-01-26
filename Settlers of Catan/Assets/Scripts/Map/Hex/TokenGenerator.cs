using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TokenGenerator : MonoBehaviour {

    static List<int> allNums;
    public static int length;
    public static int[] tokens;

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
