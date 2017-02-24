using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
//	public static List<HexVertex> vertexPositions = new List<HexVertex>();
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

    public HexCell[] getCells()
    {
        return this.cells;
    }

    void Awake()
    {
        // there's only one canvas as a child to the gameObject this script is attached to
        // hence we don't need to search for the name
        gridCanvas = GameObject.Find("Hex Grid Canvas").GetComponent<Canvas>();
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
            buildButton.GetComponentInChildren<Text>().text = "End Build";
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
            if (roadBuild)
            {
                PlaceEdge(hit.point, EdgeUnitType.Road);
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
		} while (iterator.MoveNext() != false);

		//convert vertex set to hexvertex set
		foreach (Vector3 i in tempList)
		{
//			vertexPositions.Add(new HexVertex(i));
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
        float xCoord = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        float yCoord = 0f;
        float zCoord = z * (HexMetrics.outerRadius * 1.5f);

		Vector3 position = new Vector3(xCoord, yCoord, zCoord);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);

        cell.transform.SetParent(transform, false);
        cell.transform.position = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
//		cell.centerVertex = new HexVertex(position);

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

    void PlaceEdge(Vector3 position, EdgeUnitType edgeUnitType)
    {
        // Converting hit point to cell reference
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        // Linking with UI Dropdown menu values

        
        if (cell.GetEdge(edgeDirectionDD.value) == null) // Null for prevent dups
        {
            // TODO: Place unit only if followed by another unit.
            int direction = edgeDirectionDD.value;
            if (cell.possibleEdges[direction] != null)
            {
                // Instantiate prefab and set unit type (Road/Ship)
                HexEdge edge = Instantiate<HexEdge>(edgePrefab);
                edge.MyEdgeUnitType = edgeUnitType;

                Debug.Log(cell.possibleEdges[direction].ToString());
                Destroy(cell.possibleEdges[direction].gameObject);
                Destroy(cell.GetNeighbor(direction).possibleEdges[direction < 3 ? direction + 3 : direction - 3]);
                cell.possibleEdges[direction] = null;
                cell.GetNeighbor(direction).possibleEdges[direction < 3 ? direction + 3 : direction - 3] = null;

                cell.SetEdge(direction, edge);
                //Destroy(cell.possibleEdges[edgeDirectionDD.value].gameObject); // Remove that edge location from possible locations
                edge.transform.SetParent(cell.transform, false);

                // Getting the middle point of the two vertices for that edge, placing there.
                edge.transform.localPosition = (HexMetrics.corners[direction] + HexMetrics.corners[(direction + 1) % 6]) / 2;
                edge.transform.localRotation = EdgeMetrics.rotations[direction];
                edge.FirstCell = cell;
                if (cell.GetNeighbor(direction) != null) // Set neighbors only if another cell exists
                {
                    edge.name = "edge between " + cell.cellNumber + " & " + cell.GetNeighbor(direction).cellNumber;

                    edge.SecondCell = cell.GetNeighbor(direction);
                }
                else
                {
                    edge.name = "edge between " + cell.cellNumber + " & -- ";
                }

                // AND BEHOLD, THE EDGE UNIT NEIGHBOR STRUCTURE SETUP
                // Don't even try understanding this.
                if (edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1) != null)
                {
                    edge.NeighborEdges[0] =
                        edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1);

                    if (edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1).FirstCell ==
                        edge.FirstCell)
                    {
                        edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1)
                            .NeighborEdges[1] = edge;
                    }
                    else
                    {
                        if (edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1)
                                .NeighborEdges[2] == null)
                        {
                            edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1)
                            .NeighborEdges[2] = edge;
                        }

                    }
                }


                if (edge.FirstCell.GetEdge((direction + 1) % 6) != null)
                {
                    edge.NeighborEdges[1] = edge.FirstCell.GetEdge((direction + 1) % 6);

                    if (edge.FirstCell.GetEdge((direction + 1) % 6).FirstCell == edge.FirstCell)
                    {
                        edge.FirstCell.GetEdge((direction + 1) % 6).NeighborEdges[0] = edge;
                    }
                    else
                    {
                        if (edge.FirstCell.GetEdge((direction + 1) % 6).NeighborEdges[3] == null)
                        {
                            edge.FirstCell.GetEdge((direction + 1) % 6).NeighborEdges[3] = edge;
                        }
                    }
                }

                if (edge.SecondCell != null)
                {
                    if (edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1) != null)
                    {
                        edge.NeighborEdges[2] =
                            edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1);
                        if (edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1).SecondCell ==
                            edge.SecondCell)
                        {
                            edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1)
                                .NeighborEdges[3] = edge;
                        }
                        else
                        {
                            if (
                                edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1)
                                    .NeighborEdges[0] == null)
                            {
                                edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1)
                                    .NeighborEdges[0] = edge;
                            }
                        }

                    }
                    if (
                        edge.SecondCell.GetEdge(direction < 3
                            ? direction + 3 - 1
                            : (direction - 3 - 1 < 0 ? direction - 3 - 1 + 6
                                                       : direction - 3 - 1)) != null)
                    {
                        edge.NeighborEdges[3] = edge.SecondCell.GetEdge(direction < 3
                            ? direction + 3 - 1
                            : (direction - 3 - 1 < 0
                                ? direction - 3 - 1 + 6
                                : direction - 3 - 1));
                        if (edge.SecondCell.GetEdge(direction < 3
                                ? direction + 3 - 1
                                : (direction - 3 - 1 < 0
                                    ? direction - 3 - 1 + 6
                                    : direction - 3 - 1)).SecondCell == edge.SecondCell)
                        {
                            edge.SecondCell.GetEdge(direction < 3
                                ? direction + 3 - 1
                                : (direction - 3 - 1 < 0
                                    ? direction - 3 - 1 + 6
                                    : direction - 3 - 1)).NeighborEdges[2] = edge;
                        }
                        else
                        {
                            if (edge.SecondCell.GetEdge(direction < 3
                                    ? direction + 3 - 1
                                    : (direction - 3 - 1 < 0
                                        ? direction - 3 - 1 + 6
                                        : direction - 3 - 1)).NeighborEdges[1] == null)
                            {
                                edge.SecondCell.GetEdge(direction < 3
                                    ? direction + 3 - 1
                                    : (direction - 3 - 1 < 0
                                        ? direction - 3 - 1 + 6
                                        : direction - 3 - 1)).NeighborEdges[1] = edge;
                            }
                        }
                    }
                }
                // After placing an edge unit on a location, update the set of possible edge units that can be placed.
                createPossibleEdges(cell);
            }
            // Place unit on the edge user asked for, through dropdown menu.
            
            
            
        }
    }

    public void PlaceEdge_Sandbox(EdgeUnitType edgeUnitType)
    {
        // Converting hit point to cell reference
        //position = transform.InverseTransformPoint(position);
        //HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        //int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        int index = RandomCell.giveCell();
        HexCell cell = cells[index];
        int randomEdgeDirection = RandomCell.giveDir(cells[index]);
        // Linking with UI Dropdown menu values


        if (cell.GetEdge(randomEdgeDirection) == null) // Null for prevent dups
        {
            // TODO: Place unit only if followed by another unit.
            // Instantiate prefab and set unit type (Road/Ship)
            HexEdge edge = Instantiate<HexEdge>(edgePrefab);
            edge.MyEdgeUnitType = edgeUnitType;
            int direction = randomEdgeDirection;
            if (cell.possibleEdges[direction] != null)
            {
                Destroy(cell.possibleEdges[direction].gameObject);
                cell.possibleEdges[direction] = null;
            }
            // Place unit on the edge user asked for, through dropdown menu.
            cell.SetEdge(direction, edge);
            //Destroy(cell.possibleEdges[edgeDirectionDD.value].gameObject); // Remove that edge location from possible locations
            edge.transform.SetParent(cell.transform, false);

            // Getting the middle point of the two vertices for that edge, placing there.
            edge.transform.localPosition = (HexMetrics.corners[direction] + HexMetrics.corners[(direction + 1) % 6]) / 2;
            edge.transform.localRotation = EdgeMetrics.rotations[direction];
            edge.FirstCell = cell;
            if (cell.GetNeighbor(direction) != null) // Set neighbors only if another cell exists
            {
                edge.name = "edge between " + cell.cellNumber + " & " + cell.GetNeighbor(direction).cellNumber;

                edge.SecondCell = cell.GetNeighbor(direction);
            }
            else
            {
                edge.name = "edge between " + cell.cellNumber + " & -- ";
            }

            // AND BEHOLD, THE EDGE UNIT NEIGHBOR STRUCTURE SETUP
            // Don't even try understanding this.
            if (edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1) != null)
            {
                edge.NeighborEdges[0] =
                    edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1);

                if (edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1).FirstCell ==
                    edge.FirstCell)
                {
                    edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1)
                        .NeighborEdges[1] = edge;
                }
                else
                {
                    if (edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1)
                            .NeighborEdges[2] == null)
                    {
                        edge.FirstCell.GetEdge(direction - 1 < 0 ? (direction - 1 + 6) % 6 : direction - 1)
                        .NeighborEdges[2] = edge;
                    }

                }
            }


            if (edge.FirstCell.GetEdge((direction + 1) % 6) != null)
            {
                edge.NeighborEdges[1] = edge.FirstCell.GetEdge((direction + 1) % 6);

                if (edge.FirstCell.GetEdge((direction + 1) % 6).FirstCell == edge.FirstCell)
                {
                    edge.FirstCell.GetEdge((direction + 1) % 6).NeighborEdges[0] = edge;
                }
                else
                {
                    if (edge.FirstCell.GetEdge((direction + 1) % 6).NeighborEdges[3] == null)
                    {
                        edge.FirstCell.GetEdge((direction + 1) % 6).NeighborEdges[3] = edge;
                    }
                }
            }

            if (edge.SecondCell != null)
            {
                if (edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1) != null)
                {
                    edge.NeighborEdges[2] =
                        edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1);
                    if (edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1).SecondCell ==
                        edge.SecondCell)
                    {
                        edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1)
                            .NeighborEdges[3] = edge;
                    }
                    else
                    {
                        if (
                            edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1)
                                .NeighborEdges[0] == null)
                        {
                            edge.SecondCell.GetEdge(direction < 3 ? (direction + 3 + 1) % 6 : direction - 3 + 1)
                                .NeighborEdges[0] = edge;
                        }
                    }

                }
                if (
                    edge.SecondCell.GetEdge(direction < 3
                        ? direction + 3 - 1
                        : (direction - 3 - 1 < 0 ? direction - 3 - 1 + 6
                                                   : direction - 3 - 1)) != null)
                {
                    edge.NeighborEdges[3] = edge.SecondCell.GetEdge(direction < 3
                        ? direction + 3 - 1
                        : (direction - 3 - 1 < 0
                            ? direction - 3 - 1 + 6
                            : direction - 3 - 1));
                    if (edge.SecondCell.GetEdge(direction < 3
                            ? direction + 3 - 1
                            : (direction - 3 - 1 < 0
                                ? direction - 3 - 1 + 6
                                : direction - 3 - 1)).SecondCell == edge.SecondCell)
                    {
                        edge.SecondCell.GetEdge(direction < 3
                            ? direction + 3 - 1
                            : (direction - 3 - 1 < 0
                                ? direction - 3 - 1 + 6
                                : direction - 3 - 1)).NeighborEdges[2] = edge;
                    }
                    else
                    {
                        if (edge.SecondCell.GetEdge(direction < 3
                                ? direction + 3 - 1
                                : (direction - 3 - 1 < 0
                                    ? direction - 3 - 1 + 6
                                    : direction - 3 - 1)).NeighborEdges[1] == null)
                        {
                            edge.SecondCell.GetEdge(direction < 3
                                ? direction + 3 - 1
                                : (direction - 3 - 1 < 0
                                    ? direction - 3 - 1 + 6
                                    : direction - 3 - 1)).NeighborEdges[1] = edge;
                        }
                    }

                }
            }
            // After placing an edge unit on a location, update the set of possible edge units that can be placed.
            createPossibleEdges(cell);
        }
    }

    private void createPossibleEdges(HexCell cell)
    {
        // For all edges around the cell
        for (int i = 0; i < 6; i++)
        {
            HexEdge currentEdge = cell.GetEdge(i);

            // If there is an edge placed on location i
            if (currentEdge != null)
            {
                int firstCell_previousIndex = i - 1 < 0 ? (i - 1 + 6) % 6 : (i - 1) % 6;
                int firstCell_nextIndex = (i + 1) % 6;
                // If the NW edge location of the placed edge unit is not already a possible location to place an edge unit
                if (currentEdge.FirstCell.possibleEdges[firstCell_previousIndex] == null // If there's no 
                    && currentEdge.FirstCell.GetEdge(firstCell_previousIndex) == null)
                {
                    HexEdge possibleEdge_NW = Instantiate<HexEdge>(possibleEdgePrefab);
                    currentEdge.FirstCell.possibleEdges[firstCell_previousIndex] =
                        possibleEdge_NW;

                    if (currentEdge.FirstCell.GetNeighbor(firstCell_previousIndex) != null &&
                        currentEdge.FirstCell.GetNeighbor(firstCell_previousIndex).possibleEdges[
                            (firstCell_previousIndex < 3) ? firstCell_previousIndex + 3 : firstCell_previousIndex - 3] == null)
                    {
                        currentEdge.FirstCell.GetNeighbor(firstCell_previousIndex).possibleEdges[
                            (firstCell_previousIndex < 3) ? firstCell_previousIndex + 3 : firstCell_previousIndex - 3] = possibleEdge_NW;
                    }
                    possibleEdge_NW.transform.SetParent(cell.transform.Find("Empty Edges").transform);
                    possibleEdge_NW.transform.localPosition =
                    (HexMetrics.corners[i - 1 < 0 ? (i - 1 + 6) % 6 : (i - 1) % 6] +
                     HexMetrics.corners[i]) / 2;
                    possibleEdge_NW.transform.localRotation = EdgeMetrics.rotations[i - 1 < 0 ? (i - 1 + 6) % 6 : (i - 1) % 6];
                }
                // If the SW edge location of the placed edge unit is not already a possible location to place an edge unit
                if (currentEdge.FirstCell.possibleEdges[(i + 1) % 6] == null &&
                    currentEdge.FirstCell.GetEdge((i + 1) % 6) == null) 
                {
                    HexEdge possibleEdge_SW = Instantiate<HexEdge>(possibleEdgePrefab);
                    currentEdge.FirstCell.possibleEdges[firstCell_nextIndex] = possibleEdge_SW;
                    if (currentEdge.FirstCell.GetNeighbor(firstCell_nextIndex) != null &&
                        currentEdge.FirstCell.GetNeighbor(firstCell_nextIndex).possibleEdges[
                            (firstCell_nextIndex < 3) ? firstCell_nextIndex + 3 : firstCell_nextIndex - 3] == null)
                    {
                        currentEdge.FirstCell.GetNeighbor(firstCell_nextIndex).possibleEdges[
                            (firstCell_nextIndex < 3) ? firstCell_nextIndex + 3 : firstCell_nextIndex - 3] = possibleEdge_SW;
                    }
                    possibleEdge_SW.transform.SetParent(cell.transform.Find("Empty Edges").transform);
                    possibleEdge_SW.transform.localPosition =
                        (HexMetrics.corners[(i + 1) % 6] + HexMetrics.corners[(i + 2) % 6]) / 2;
                    possibleEdge_SW.transform.localRotation = EdgeMetrics.rotations[(i + 1) % 6];
                }
                if (currentEdge.SecondCell != null)
                {
                    if (i < 3) // then opposite edge dir = i + 3
                    {
                        // If the NE edge location of the placed edge unit is not already a possible location to place an edge unit
                        if (currentEdge.SecondCell.possibleEdges[(i + 3 + 1) % 6] == null &&
                            currentEdge.SecondCell.GetEdge((i + 3 + 1) % 6) == null)
                        {
                            HexEdge possibleEdge_NE = Instantiate<HexEdge>(possibleEdgePrefab);
                            int secondCell_nextIndex = (i + 3 + 1) % 6;
                            currentEdge.SecondCell.possibleEdges[secondCell_nextIndex] = possibleEdge_NE;
                            if (currentEdge.SecondCell.GetNeighbor(secondCell_nextIndex) != null &&
                                currentEdge.SecondCell.GetNeighbor(secondCell_nextIndex).possibleEdges[
                                    (secondCell_nextIndex < 3) ? secondCell_nextIndex + 3 : secondCell_nextIndex - 3] ==
                                null)
                            {
                                currentEdge.SecondCell.GetNeighbor(secondCell_nextIndex).possibleEdges[
                                        (secondCell_nextIndex < 3) ? secondCell_nextIndex + 3 : secondCell_nextIndex - 3] =
                                    possibleEdge_NE;
                            }
                            possibleEdge_NE.transform.SetParent(
                                currentEdge.SecondCell.transform.Find("Empty Edges").transform);
                            possibleEdge_NE.transform.localPosition = (HexMetrics.corners[(i + 3 + 1) % 6] +
                                                                       HexMetrics.corners[(i + 3 + 2) % 6]) / 2;
                            possibleEdge_NE.transform.localRotation = EdgeMetrics.rotations[(i + 3 + 1) % 6];
                        }
                        // If the SE edge location of the placed edge unit is not already a possible location to place an edge unit
                        if (
                            currentEdge.SecondCell.possibleEdges[
                                i + 3 - 1 < 0 ? (i + 3 - 1 + 6) % 6 : (i + 3 - 1) % 6] == null &&
                            currentEdge.SecondCell.GetEdge(
                                i + 3 - 1 < 0 ? (i + 3 - 1 + 6) % 6 : (i + 3 - 1) % 6) == null) 
                        {
                            HexEdge possibleEdge_SE = Instantiate<HexEdge>(possibleEdgePrefab);
                            int secondCell_previousIndex = i + 3 - 1 < 0 ? (i + 3 - 1 + 6) % 6 : (i + 3 - 1) % 6;
                            currentEdge.SecondCell.possibleEdges[secondCell_previousIndex] = possibleEdge_SE;
                            if (currentEdge.SecondCell.GetNeighbor(secondCell_previousIndex) != null &&
                                currentEdge.SecondCell.GetNeighbor(secondCell_previousIndex).possibleEdges[
                                    (secondCell_previousIndex < 3)
                                        ? secondCell_previousIndex + 3
                                        : secondCell_previousIndex - 3] == null)
                            {
                                currentEdge.SecondCell.GetNeighbor(secondCell_previousIndex).possibleEdges[
                                    (secondCell_previousIndex < 3)
                                        ? secondCell_previousIndex + 3
                                        : secondCell_previousIndex - 3] = possibleEdge_SE;
                            }
                            possibleEdge_SE.transform.SetParent(
                                currentEdge.SecondCell.transform.Find("Empty Edges").transform);
                            possibleEdge_SE.transform.localPosition =
                            (HexMetrics.corners[i + 3 - 1 < 0 ? (i + 3 - 1 + 6) % 6 : (i + 3 - 1) % 6] +
                             HexMetrics.corners[(i + 3) % 6]) / 2;
                            possibleEdge_SE.transform.localRotation =
                                EdgeMetrics.rotations[i + 3 - 1 < 0 ? (i + 3 - 1 + 6) % 6 : (i + 3 - 1) % 6];
                        }
                    }
                    else // then the opposite edge dir = i - 3
                    {
                        
                        // If the NE edge location of the placed edge unit is not already a possible location to place an edge unit
                        if (currentEdge.SecondCell.possibleEdges[(i - 3 + 1) % 6] == null &&
                            currentEdge.SecondCell.GetEdge((i - 3 + 1) % 6) == null) 
                        {
                            HexEdge possibleEdge_NE = Instantiate<HexEdge>(possibleEdgePrefab);
                            int secondCell_nextIndex = (i - 3 + 1) % 6;
                            currentEdge.SecondCell.possibleEdges[secondCell_nextIndex] = possibleEdge_NE;
                            if (currentEdge.SecondCell.GetNeighbor(secondCell_nextIndex) != null &&
                                currentEdge.SecondCell.GetNeighbor(secondCell_nextIndex).possibleEdges[
                                    (secondCell_nextIndex < 3) ? secondCell_nextIndex + 3 : secondCell_nextIndex - 3] ==
                                null)
                            {
                                currentEdge.SecondCell.GetNeighbor(secondCell_nextIndex).possibleEdges[
                                        (secondCell_nextIndex < 3) ? secondCell_nextIndex + 3 : secondCell_nextIndex - 3] =
                                    possibleEdge_NE;
                            }
                            possibleEdge_NE.transform.SetParent(
                                currentEdge.SecondCell.transform.Find("Empty Edges").transform);
                            possibleEdge_NE.transform.localPosition = (HexMetrics.corners[(i - 3 + 1) % 6] +
                                                                       HexMetrics.corners[(i - 3 + 2) % 6]) / 2;
                            possibleEdge_NE.transform.localRotation = EdgeMetrics.rotations[(i - 3 + 1) % 6];
                        }
                        // If the SE edge location of the placed edge unit is not already a possible location to place an edge unit
                        if (
                            currentEdge.SecondCell.possibleEdges[
                                i - 3 - 1 < 0 ? (i - 3 - 1 + 6) % 6 : (i - 3 - 1) % 6] == null &&
                            currentEdge.SecondCell.GetEdge(
                                i - 3 - 1 < 0 ? (i - 3 - 1 + 6) % 6 : (i - 3 - 1) % 6) == null)
                        {
                            int secondCell_previousIndex = i - 3 - 1 < 0 ? (i - 3 - 1 + 6) % 6 : (i - 3 - 1) % 6;
                            HexEdge possibleEdge_SE = Instantiate<HexEdge>(possibleEdgePrefab);
                            currentEdge.SecondCell.possibleEdges[secondCell_previousIndex] = possibleEdge_SE;
                            if (currentEdge.SecondCell.GetNeighbor(secondCell_previousIndex) != null &&
                                currentEdge.SecondCell.GetNeighbor(secondCell_previousIndex).possibleEdges[
                                    (secondCell_previousIndex < 3)
                                        ? secondCell_previousIndex + 3
                                        : secondCell_previousIndex - 3] == null)
                            {
                                currentEdge.SecondCell.GetNeighbor(secondCell_previousIndex).possibleEdges[
                                    (secondCell_previousIndex < 3)
                                        ? secondCell_previousIndex + 3
                                        : secondCell_previousIndex - 3] = possibleEdge_SE;
                            }
                            possibleEdge_SE.transform.SetParent(
                                currentEdge.SecondCell.transform.Find("Empty Edges").transform);
                            possibleEdge_SE.transform.localPosition =
                            (HexMetrics.corners[i - 3 - 1 < 0 ? (i - 3 - 1 + 6) % 6 : (i - 3 - 1) % 6] +
                             HexMetrics.corners[(i - 3) % 6]) / 2;
                            possibleEdge_SE.transform.localRotation =
                                EdgeMetrics.rotations[i - 3 - 1 < 0 ? (i - 3 - 1 + 6) % 6 : (i - 3 - 1) % 6];
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
