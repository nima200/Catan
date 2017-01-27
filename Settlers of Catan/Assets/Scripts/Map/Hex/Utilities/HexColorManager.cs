using UnityEngine;
using System.Collections;

public class HexColorManager : MonoBehaviour {

    public HexGrid grid;

    void Start()
    {
        HexCell[] cells = grid.getCells();
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                HexType cellType = TypeGenerator();
                cells[i].myHexType = cellType;
                ColorHex(cells[i]);
            }
            
        }
    }

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
        if (cell.myHexType == HexType.Brick)
        {
            cell.rend.material.color = new Color(0.61f, 0.09f, 0.04f);
        } else if (cell.myHexType == HexType.Desert)
        {
            cell.rend.material.color = new Color(0.76f, 0.70f, 0.50f);
        } else if (cell.myHexType == HexType.Wood)
        {
            cell.rend.material.color = Color.green;
        } else if (cell.myHexType == HexType.Sheep)
        {
            cell.rend.material.color = Color.white;
        } else if (cell.myHexType == HexType.Sea)
        {
            cell.rend.material.color = Color.blue;
        } else
        {
            cell.rend.material.color = Color.grey;
        }
    }
}
