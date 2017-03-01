using UnityEngine;
using System.Collections;

public class HexColorManager : MonoBehaviour {

    public BoardManager grid;

    void Start()
    {
        HexCell[] cells = grid.Cells;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                HexType cellType = TypeGenerator();
                cells[i].MyHexType = cellType;
                ColorHex(cells[i]);
            }
            
        }
    }
    // TODO: Figure out how to change the distribution of the hex types
    // so that they actually correspond to what the game has.
    // i.e. there aren't as many desert hexes as there are wood.
    HexType TypeGenerator()
    {
        int type = Random.Range(0, 6);
        if (type == 0)
        {
            return HexType.Wood;
        } else if (type == 1)
        {
            return HexType.Ore;
        } else if (type == 2)
        {
            return HexType.Brick;
        } else if (type == 3)
        {
            return HexType.Sheep;
        } else if (type == 4)
        {
            return HexType.Sea;
        } else
        {
            return HexType.Desert;
        }
            
    }

    void ColorHex(HexCell cell)
    {
        if (cell.MyHexType == HexType.Brick)
        {
            cell.Rend.material.color = new Color(0.61f, 0.09f, 0.04f);
        } else if (cell.MyHexType == HexType.Desert)
        {
            cell.Rend.material.color = new Color(0.76f, 0.70f, 0.50f);
        } else if (cell.MyHexType == HexType.Wood)
        {
            cell.Rend.material.color = Color.green;
        } else if (cell.MyHexType == HexType.Sheep)
        {
            cell.Rend.material.color = Color.white;
        } else if (cell.MyHexType == HexType.Sea)
        {
            cell.Rend.material.color = Color.blue;
        } else
        {
            cell.Rend.material.color = Color.grey;
        }
    }
}
