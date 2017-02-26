﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTrimmer : MonoBehaviour {

    public HexGrid grid;
    // Manually trimming off the sides of a rectangular grid.
    // Very hacky and hard coded, lol.

    // Also, we delete all the extras, and then we get to start tokenizing the cells
    // Since at first we have width*height many cells but we only want 44 anyways.
    void Start()
    {
        HexCell[] cells = grid.GetCells();

		// desired hexes to be trimmed
		int[] numbers = { 0, 1, 7, 8, 15, 16, 32, 40, 47, 48, 49, 55};

		foreach (int i in numbers)
		{

			//Debug.Log(cells[i]);
			for (int j = 0; j < 6; j++)
			{
				if (cells[i].GetNeighbor(j) != null)
				{
					
					cells[i].Neighbors[j].Neighbors[(int)((HexDirection)j).Opposite()] = null;
				}
			}
			Destroy(cells[i].gameObject);
			Destroy(cells[i].Label.gameObject);
			cells[i] = null;
		}

        grid.AssignTokens();
        grid.HidePossibleEdges();
        //        grid.PlaceEdge_Sandbox(EdgeUnitType.Road);
        //        grid.PlaceEdge_Sandbox(EdgeUnitType.Road);
        grid.CreateEdges();

        grid.CreateVertices();
    }

}
