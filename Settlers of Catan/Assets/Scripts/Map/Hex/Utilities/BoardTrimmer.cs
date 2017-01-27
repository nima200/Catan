using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTrimmer : MonoBehaviour {

    public HexGrid grid;

    void Start()
    {
        HexCell[] cells = grid.getCells();
        
        Destroy(cells[0].gameObject);
        Destroy(cells[0].label.gameObject);
        cells[0] = null;
        Destroy(cells[1].gameObject);
        Destroy(cells[1].label.gameObject);
        cells[1] = null;

        Destroy(cells[7].gameObject);
        Destroy(cells[7].label.gameObject);
        cells[7] = null;
        Destroy(cells[8].gameObject);
        Destroy(cells[8].label.gameObject);
        cells[8] = null;

        Destroy(cells[15].gameObject);
        Destroy(cells[15].label.gameObject);
        cells[15] = null;
        Destroy(cells[16].gameObject);
        Destroy(cells[16].label.gameObject);
        cells[16] = null;

        Destroy(cells[32].gameObject);
        Destroy(cells[32].label.gameObject);
        cells[32] = null;

        Destroy(cells[40].gameObject);
        Destroy(cells[40].label.gameObject);
        cells[40] = null;

        Destroy(cells[47].gameObject);
        Destroy(cells[47].label.gameObject);
        cells[47] = null;
        Destroy(cells[48].gameObject);
        Destroy(cells[48].label.gameObject);
        cells[48] = null;
        Destroy(cells[49].gameObject);
        Destroy(cells[49].label.gameObject);
        cells[49] = null;

        Destroy(cells[55].gameObject);
        Destroy(cells[55].label.gameObject);
        cells[55] = null;

        grid.assignTokens();
    }

}
