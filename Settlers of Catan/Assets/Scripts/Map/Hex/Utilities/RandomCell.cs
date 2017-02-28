using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomCell
{
    public static BoardManager grid = GameObject.Find("Hex Grid").GetComponent<BoardManager>();

    public static int giveCell()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, grid.Cells.Length - 1);
            if (grid.Cells[randomIndex] != null)
            {
                return randomIndex;
            }
        }
    }

    public static int giveDir(HexCell cell)
    {
        while (true)
        {
            int randomDirection = Random.Range(0, 6);
            if (cell.GetEdge(randomDirection) == null)
            {
                return randomDirection;
            }
        }
    }

}
