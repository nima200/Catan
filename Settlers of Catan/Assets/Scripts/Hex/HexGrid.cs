using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HexGrid : MonoBehaviour {

    // the array of hexcells that the grid stores
    HexCell[] cells;
    public int width = 6;
    public int height = 6;

    int cellIndex = 0;

    // the prefab to make the grid use as cells
    public HexCell cellPrefab;
    
    // making our grid know about the label prefab
    public Text cellLabelPrefab;
    // making our grid know about the canvas too
    Canvas gridCanvas;
    // making our grid know about the hex mesh
    HexMesh hexMesh;

    public Color defaultColor = Color.white;
    public Color neighborColor = Color.magenta;

    void Awake()
    {
        // there's only one canvas as a child to the gameObject this script is attached to
        // hence we don't need to search for the name
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();


        cells = new HexCell[height * width];
        // i is the index of the cell in the HexCell array.
        // i goes from 0 to height*width;
        // i is incremented every time we create a new cell
        // we use x and z for the location of the cell because we build on the XZ plane.
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    // After the grid is awake, we can now triangulate the cells of the mesh.
    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            TouchCell(hit.point);
            EchoNeighbors(hit.point);
        }
    }

    void TouchCell(Vector3 position)
    {
        Debug.ClearDeveloperConsole();
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        for (int i = 0; i < 6; i++)
        {
            if (cell.GetNeighbor(i) != null)
            {
                cell.GetNeighbor(i).color = Color.red;

            }
        }
        hexMesh.Triangulate(cells);
        
    }

    // distance between adjacent hexagon cells in the x direction is equal to twice the inner radius of the hex
    // distance between adjacent hexagon cells in the z direction (distance between two rows) is equal to 1.5 times the outer radius

    // also, hex rows are not directly on top of each other. Each row is offset along the X axis by the inner radius.
    // however we need to bring them back cause if for every row we only add half of z, then it just turns into a rhombus in the overall shape
    // to get the brick stack effect,  we need to bring back the x offset of every "other" row.

    // subtracting by the INT division makes this awesome cause we can essentially subtract every time but the effect is only applied
    // once every other row.

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + (z * 0.5f) - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);

        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        cell.cellNumber = cellIndex;
        cellIndex++;
        if (x > 0)
        {
            // adding the west neighbor of all cells
            // the first cell of each row doesn't have a west neighbor, but all the other cells in the row do.
            // that's why we only create these neighbors for those with x > 0 and not for those with x = 0
            // note that setting a neighbor connection also sets the reverse cause we have implemented this in 
            // the setneighbor method of hexcell.
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }

        if (z > 0)
        {
            // the rows are bricked. So we have to treat the neighbors of the rows differently.
            // all cells in even rows have an SE neighbor.

            // we used bitwise AND to find out if the row is even.
            // even numbers always have a binary 0 LSD.
            // so we ignore everything besides the first bit, if that first bit and 1 have bitwise AND 0,
            // then it's even.
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                // we can also set the SW neighbors. Except for the first cell in each (even) row obviously.
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            } else
            {
                // now we deal with odd rows
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        // assigning the coordinates of the cell to the label prefab
        Text label = Instantiate<Text>(cellLabelPrefab);
        // making sure the label falls under the canvas, as its child
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    void EchoNeighbors(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        Debug.Log("My cell number is: " + cells[index].cellNumber);
        if (cells[index].GetNeighbor(HexDirection.NE) != null)
        {
            Debug.Log("My NE neighbor is: " + cells[index].GetNeighbor(HexDirection.NE).cellNumber);
        }
        if (cells[index].GetNeighbor(HexDirection.E) != null)
        {
            Debug.Log("My E neighbor is: " + cells[index].GetNeighbor(HexDirection.E).cellNumber);
        }
        if (cells[index].GetNeighbor(HexDirection.SE) != null)
        {
            Debug.Log("My SE neighbor is: " + cells[index].GetNeighbor(HexDirection.SE).cellNumber);
        }
        if (cells[index].GetNeighbor(HexDirection.SW) != null)
        {
            Debug.Log("My SW neighbor is: " + cells[index].GetNeighbor(HexDirection.SW).cellNumber);
        }
        if (cells[index].GetNeighbor(HexDirection.W) != null)
        {
            Debug.Log("My W neighbor is: " + cells[index].GetNeighbor(HexDirection.W).cellNumber);
        }
        if (cells[index].GetNeighbor(HexDirection.NW) != null)
        {
            Debug.Log("My NW neighbor is: " + cells[index].GetNeighbor(HexDirection.NW).cellNumber);
        }
    }
}
