using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HexGrid : MonoBehaviour {

    // the array of hexcells that the grid stores
    public HexCell[] cells;
    public int width = 8;
    public int height = 7;

    int[] tokens;

	//this will keep track of all unique vertex positions to later create the HexVertices
	public static HashSet<Vector3> positions = new HashSet<Vector3>();

	//this will keep track of all unique hexvertex positions that will be used to create the network
	public static List<HexVertex> vertexPositions = new List<HexVertex>();

    // the prefab to make the grid use as cells
    public HexCell cellPrefab;
    // making our grid know about the label prefab
    public Text cellLabelPrefab;
    // making our grid know about the canvas too
    Canvas gridCanvas;
    Canvas UI;
    bool roadBuild = false;
    public Button buildButton;
    public HexEdge edgePrefab;
    public HexEdge possibleEdgePrefab;
    Dropdown edgeDirectionDD;


	//vertex prefab
	public HexVertex settlementPrefab;

    

    public HexCell[] getCells()
    {
        return this.cells;
    }

    void Awake()
    {
        // there's only one canvas as a child to the gameObject this script is attached to
        // hence we don't need to search for the name
        gridCanvas = GetComponentInChildren<Canvas>();
        UI = GameObject.Find("User Interface").GetComponent<Canvas>();
        makeTokens();
        cells = new HexCell[height * width];
        edgeDirectionDD = UI.GetComponentInChildren<Dropdown>();
        edgeDirectionDD.gameObject.SetActive(false);
        // i is the index of the cell in the HexCell array.
        // i goes from 0 to (height*width);
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
        Triangulate(cells);
    }

    void Update()
    {
        // Mouse Button 0 = Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();

        }

    }

    public void toggleBuild()
    {
        if (roadBuild == false)
        {
            roadBuild = true;
            edgeDirectionDD.gameObject.SetActive(true);
            buildButton.GetComponentInChildren<Text>().text = "End Build Road";
            ShowPossibleEdges();
        } else
        {
            roadBuild = false;
            edgeDirectionDD.gameObject.SetActive(false);
            buildButton.GetComponentInChildren<Text>().text = "Build Road";
            HidePossibleEdges();
        }
    }

    
    // Very generic mouse input handle method.
    // Check this out for more info
    // https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            // EchoNeighbors(hit.point);
            if (roadBuild)
            {
                PlaceEdge(hit.point);
            }
        }
    }

    // Calls onto the token generator.
    void makeTokens()
    {
        tokens = TokenGenerator.generate(44);
    }

    public void assignTokens()
    {
        int tokenIndex = 0;
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                cells[i].cellNumber = tokens[tokenIndex];
                cells[i].label.text = cells[i].cellNumber.ToString();
                cells[i].gameObject.name = "Hex " + cells[i].cellNumber.ToString();
                tokenIndex++;
            }
        }
    }

    void ShowPossibleEdges()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                cells[i].transform.Find("Empty Edges").gameObject.SetActive(true);
            }
        }
    }

    public void HidePossibleEdges()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                cells[i].transform.Find("Empty Edges").gameObject.SetActive(false);
            }
        }
    }


	// creates HexVertex layer that will reside within the board
	//This is called once the board has been trimmed
	public void createHexVertices()
	{
		
		HashSet<Vector3>.Enumerator iterator = positions.GetEnumerator();

		List<Vector3> tempList = new List<Vector3>();

		//get rid of vertices only associated with null hexes
		do
		{
			// grab current vector3
			Vector3 temp = iterator.Current;

			// go through non-null cell and determine if the vector3 exists as a position
			foreach (HexCell i in cells)
			{

				if (i != null)
				{
					if (i.globalVertices.Contains(temp))
					{
						tempList.Add(temp);
						break;
					}
				}
			}
		} while ((iterator.MoveNext() != false));

		//convert vertex set to hexvertex set
		foreach (Vector3 i in tempList)
		{
			Debug.Log("vertex: " + i);
			HexVertex v = Instantiate(settlementPrefab, i, Quaternion.identity);
			vertexPositions.Add(v);

			//here you want to add the appropriate HexVertex references within the cell and add as child
			foreach (HexCell cell in cells)
			{

				if (cell != null)
				{
					if (cell.globalVertices.Contains(i))
					{
						cell.hexVertices.Add(v);
						v.transform.SetParent(cell.transform.Find("Active Vertices").transform);

					}
				}
			}
		}



		//assign neighbors (leave for nima)????


		
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
        //HexVertex position;
        float xCoord = (x + (z * 0.5f) - z / 2) * (HexMetrics.innerRadius * 2f);
        float yCoord = 0f;
        float zCoord = z * (HexMetrics.outerRadius * 1.5f);

		Vector3 position = new Vector3(xCoord, yCoord, zCoord);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);

        cell.transform.SetParent(transform, false);
        cell.transform.position = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

		//Create new HexVertex
		//HexVertex v = Instantiate(settlementPrefab, position, Quaternion.identity);

		//v.isCenter = true;
		//cell.centerVertex = v;

        // HEX NEIGHBOR SET

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
        Text label = cell.label = Instantiate<Text>(cellLabelPrefab);
        // making sure the label falls under the canvas, as its child
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
    }

    void PlaceEdge(Vector3 position)
    {
        // Converting hit point to cell reference
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        // Linking with UI Dropdown menu values
        
        if (cell.GetEdge(edgeDirectionDD.value) == null) // Null for prevent dups
        {
            HexEdge edge = Instantiate<HexEdge>(edgePrefab);
            cell.SetEdge(edgeDirectionDD.value, edge);
            Destroy(cell.possibleEdges[edgeDirectionDD.value].gameObject);
            edge.transform.SetParent(cell.transform, false);
            // Getting the middle point of the two vertices for that edge, placing there.
            edge.transform.localPosition = (HexMetrics.corners[edgeDirectionDD.value] + HexMetrics.corners[(edgeDirectionDD.value + 1) % 6]) / 2;
            edge.transform.localRotation = EdgeMetrics.rotations[edgeDirectionDD.value];
            if (cell.GetNeighbor(edgeDirectionDD.value) != null) // Set neighbors only if another cell exists
            {
                edge.name = "edge between " + cell.cellNumber + " & " + cell.GetNeighbor(edgeDirectionDD.value).cellNumber;
                edge.firstCell = cell;
                edge.secondCell = cell.GetNeighbor(edgeDirectionDD.value);
            }
            else
            {
                edge.name = "edge between " + cell.cellNumber + " & -- ";
            }

        }
    }

    public void createPossibleEdges()
    {
        for (int x = 0; x < cells.Length; x++)
        {
            if (cells[x] != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (cells[x].GetEdge(i) == null)
                    {
                        if (cells[x].possibleEdges[i] == null)
                        {
                            HexEdge edge = Instantiate<HexEdge>(possibleEdgePrefab);
                            cells[x].possibleEdges[i] = edge;
                            edge.transform.SetParent(cells[x].transform.Find("Empty Edges").transform);
                            edge.transform.localPosition = (HexMetrics.corners[i] + HexMetrics.corners[(i + 1) % 6]) / 2;
                            edge.transform.localRotation = EdgeMetrics.rotations[i];
                            if (i < 3)
                            {
                                if (cells[x].GetNeighbor(i) != null)
                                {
                                    cells[x].GetNeighbor(i).possibleEdges[i + 3] = edge;
                                }
                            }
                            else
                            {
                                if (cells[x].GetNeighbor(i) != null)
                                {
                                    cells[x].GetNeighbor(i).possibleEdges[i - 3] = edge;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Straightforward method that is meant for debugging purposes to be able
    // to echo out the neighbors of each cell and make sure that it actually sees its neighbors!
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

    // Method for triangulating all cells within the "cells" array of a grid.
    void Triangulate(HexCell[] hexCells)
    {
        for (int i = 0; i < hexCells.Length; i++)
        {
            if (hexCells[i] != null)
            {
                hexCells[i].Triangulate();
            }
        }
    }

}
