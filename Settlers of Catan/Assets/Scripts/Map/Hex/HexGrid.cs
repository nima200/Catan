using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

    // the array of hexcells that the grid stores
    public HexCell[] Cells;
    public int Width = 8;
    public int Height = 7;

    private int[] _tokens;

	//this will keep track of all unique vertex positions to later create the HexVertices
	public static HashSet<Vector3> Positions = new HashSet<Vector3>();

	//this will keep track of all unique hexvertex positions that will be used to create the network
    //public static List<HexVertex> vertexPositions = new List<HexVertex>();
    // the prefab to make the grid use as cells
    public HexCell CellPrefab;
    // making our grid know about the label prefab
    public Text CellLabelPrefab;
    // making our grid know about the canvas too
    private Canvas _gridCanvas;
    private Canvas _ui;
    private bool _roadBuild = false;
    public Button BuildButton;
    public HexEdge EdgePrefab;
    public HexEdge PossibleEdgePrefab;
    private Dropdown _edgeDirectionDD;
	public HexVertex settlementPrefab;

    public HexCell[] GetCells()
    {
        return this.Cells;
    }

    private void Awake()
    {
        // there's only one canvas as a child to the gameObject this script is attached to
        // hence we don't need to search for the name
        _gridCanvas = GameObject.Find("Hex Grid Canvas").GetComponent<Canvas>();
        _ui = GameObject.Find("User Interface").GetComponent<Canvas>();
        MakeTokens();
        Cells = new HexCell[Height * Width];
        _edgeDirectionDD = _ui.GetComponentInChildren<Dropdown>();
        _edgeDirectionDD.gameObject.SetActive(false);
        // i is the index of the cell in the HexCell array.
        // i goes from 0 to (height*width);
        // i is incremented every time we create a new cell
        // we use x and z for the location of the cell because we build on the XZ plane.
        for (int z = 0, i = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    // After the grid is awake, we can now triangulate the cells of the mesh.
    private void Start()
    {
        Triangulate(Cells);
    }

    private void Update()
    {
        // Mouse Button 0 = Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }

    }

    public void ToggleBuild()
    {
        if (_roadBuild == false)
        {
            _roadBuild = true;
            _edgeDirectionDD.gameObject.SetActive(true);
            BuildButton.GetComponentInChildren<Text>().text = "End Build";
            ShowPossibleEdges();
        } else
        {
            _roadBuild = false;
            _edgeDirectionDD.gameObject.SetActive(false);
            BuildButton.GetComponentInChildren<Text>().text = "Build Road";
            HidePossibleEdges();
        }
    }

    
    // Very generic mouse input handle method.
    // Check this out for more info
    // https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            if (_roadBuild)
            {
                PlaceEdge(hit.point, EdgeUnitType.Road);
            }
        }
    }

    // Calls onto the token generator.
    private void MakeTokens()
    {
        _tokens = TokenGenerator.generate(44);
    }

    public void AssignTokens()
    {
        int tokenIndex = 0;
        for (int i = 0; i < Cells.Length; i++)
        {
            if (Cells[i] != null)
            {
                Cells[i].CellNumber = _tokens[tokenIndex];
                Cells[i].Label.text = Cells[i].CellNumber.ToString();
                Cells[i].gameObject.name = "Hex " + Cells[i].CellNumber.ToString();
                tokenIndex++;
            }
        }
    }

    private void ShowPossibleEdges()
    {
        foreach (var t in Cells)
        {
            if (t != null)
            {
                t.transform.Find("Empty Edges").gameObject.SetActive(true);
            }
        }
    }

    public void HidePossibleEdges()
    {
        foreach (var t in Cells)
        {
            if (t != null)
            {
                t.transform.Find("Empty Edges").gameObject.SetActive(false);
            }
        }
    }


	// creates HexVertex layer that will reside within the board
	//This is called once the board has been trimmed
	public void CreateHexVertices()
	{
		
		HashSet<Vector3>.Enumerator iterator = Positions.GetEnumerator();

		List<Vector3> tempList = new List<Vector3>();

		//get rid of vertices only associated with null hexes
		do
		{
			// grab current vector3
			Vector3 temp = iterator.Current;

			// go through non-null cell and determine if the vector3 exists as a position
			foreach (HexCell i in Cells)
			{

				if (i != null)
				{
					if (i.GlobalVertices.Contains(temp))
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

		//allow each vertex to be aware of surrounding Hex position



		//assign neighbors



		
	}

    // distance between adjacent hexagon cells in the x direction is equal to twice the inner radius of the hex
    // distance between adjacent hexagon cells in the z direction (distance between two rows) is equal to 1.5 times the outer radius

    // also, hex rows are not directly on top of each other. Each row is offset along the X axis by the inner radius.
    // however we need to bring them back cause if for every row we only add half of z, then it just turns into a rhombus in the overall shape
    // to get the brick stack effect,  we need to bring back the x offset of every "other" row.

    // subtracting by the INT division makes this awesome cause we can essentially subtract every time but the effect is only applied
    // once every other row.

    private void CreateCell(int x, int z, int i)
    {
        //HexVertex position;
        float xCoord = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        float yCoord = 0f;
        float zCoord = z * (HexMetrics.outerRadius * 1.5f);

		Vector3 position = new Vector3(xCoord, yCoord, zCoord);

        HexCell cell = Cells[i] = Instantiate<HexCell>(CellPrefab);

        cell.transform.SetParent(transform, false);
        cell.transform.position = position;
        cell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

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
            cell.SetNeighbor(HexDirection.W, Cells[i - 1]);
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
                cell.SetNeighbor(HexDirection.SE, Cells[i - Width]);
                // we can also set the SW neighbors. Except for the first cell in each (even) row obviously.
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, Cells[i - Width - 1]);
                }
            } else
            {
                // now we deal with odd rows
                cell.SetNeighbor(HexDirection.SW, Cells[i - Width]);
                if (x < Width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, Cells[i - Width + 1]);
                }
            }
        }

        // assigning the coordinates of the cell to the label prefab
        Text label = cell.Label = Instantiate<Text>(CellLabelPrefab);
        // making sure the label falls under the canvas, as its child
        label.rectTransform.SetParent(_gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
    }

    public void CreateVertices()
	{
		foreach (var cell in cells)
		{
			if (cell != null)
			{
				for (int i = 0; i < 6; i++)
				{
					if (cell.MyVertices[i] == null)
					{
						Debug.Log("creating vertex at location" + i + " of cell " + cell.cellNumber);
						var vertex = Instantiate(settlementPrefab);
						cell.MyVertices[i] = vertex;
						vertex.transform.SetParent(cell.transform);
						vertex.transform.localPosition = HexMetrics.corners[i];
						if (cell.GetNeighbor(i) != null)
						{
							Debug.Log("vertex between " + cell.cellNumber + " and cell " + cell.GetNeighbor(i).cellNumber);
							cell.GetNeighbor(i).MyVertices[(int)(((HexDirection)i).Opposite() + 1) % 6] = vertex;
						}
						if (cell.GetNeighbor((i + 5) % 6) != null)
						{
							Debug.Log("vertex between " + cell.cellNumber + " and cell " + cell.GetNeighbor((i + 5)%6).cellNumber);
							cell.GetNeighbor((i + 5) % 6).MyVertices[(int)(((HexDirection)i).Opposite() + 5) % 6] = vertex;
						}
					}
				}
			}
		}
	}
    private void PlaceEdge(Vector3 position, EdgeUnitType edgeUnitType)
    {
        // Converting hit point to cell reference
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * Width + coordinates.Z / 2;
        Debug.Log("index: " + index + " length: " + Cells.Length);
        if (index > Cells.Length - 1) return;
        
        var cell = Cells[index];

        // Prevent duplicates
        if (cell.GetEdge(_edgeDirectionDD.value) != null) return;

        // Just some casting that will help reduce typing
        int directionInt = _edgeDirectionDD.value;
        var direction = (HexDirection) directionInt;
        var directionOpposite = direction.Opposite();
        int directionOppositeInt = (int)directionOpposite;

        // If there isn't a possible edge at the location we want to place our edge, prevent placing an edge
        if (cell.PossibleEdges[(int)direction] == null ||
            (cell.GetNeighbor(direction) != null &&
             cell.GetNeighbor(direction).PossibleEdges[(int)direction.Opposite()] == null)) return;

        // Create the edge
        var edge = Instantiate(EdgePrefab);
        edge.MyEdgeUnitType = edgeUnitType;

        // Take a reference to the possible edge so that we can destroy it
        var possibleEdge = cell.PossibleEdges[(int)direction];

        // Remove the references from the cell to the possible edge location
        cell.PossibleEdges[(int)direction] = null;
        if (cell.GetNeighbor(direction) != null)
        {
            cell.GetNeighbor(direction).PossibleEdges[(int)direction.Opposite()] = null;
        }
        // Destroy the possible edge
        
        Destroy(possibleEdge.gameObject);
        // From this point onward this function and placeedge_sandbox share the rest
        // So best to have them both call a third helper function to basically not
        // have to edit everything twice.
        PlaceEdge_Continue(cell, directionInt, direction, directionOppositeInt, edge);
    }

    private void PlaceEdge_Continue(HexCell cell, int directionInt, HexDirection direction, int directionOppositeInt, HexEdge edge)
    {
        // Create edge-cell references
        cell.SetEdge(direction, edge);
        edge.transform.SetParent(cell.transform);

        // Find the mid-point between the two corner vertices that the edge will go to
        var midpoint_FC = (HexMetrics.corners[directionInt] + HexMetrics.corners[(directionInt + 1) % 6]) / 2;
        var midpoint_SC = (HexMetrics.corners[directionOppositeInt] + HexMetrics.corners[(directionOppositeInt + 1) % 6]) /
                          2;

        // Position and rotation respective to where it needs to go
        edge.transform.localPosition = midpoint_FC;
        edge.transform.localRotation = EdgeMetrics.rotations[directionInt];

        // Make the edge know about it's position. 
        // Just an attribute
        edge.SetPosition_FC(midpoint_FC);
        edge.FirstCell = cell;

        // Set the neighbor cell to know about this edge as well, if there is a cell there.
        if (cell.GetNeighbor(direction) != null)
        {
            edge.name = "Edge between " + cell.CellNumber + " & " + cell.GetNeighbor(direction).CellNumber;
            edge.SecondCell = cell.GetNeighbor(direction);
            edge.SetPosition_SC(midpoint_SC);
        }
        else
        {
            edge.name = "Edge between " + cell.CellNumber + " & --";
            edge.SecondCell = null;
        }

        // ==================================================================
        // +                                                                +
        // +                  Edge neighboring structure                    +
        // +                                                                +
        // ==================================================================

        // Try to find neighboring edges and add them to edge's neighbor list.

        // Previous edge location on the cell
        // (direction + 5) % 6 is essentially direction - 1 (prevents 0 - 1 = -1 index)
        int nwEdgeIndex = ((int)direction + 5) % 6;
        var nwEdge = cell.MyEdges[nwEdgeIndex];

        if (nwEdge != null)
        {
            edge.Neighbors.Add(nwEdge);
            nwEdge.Neighbors.Add(edge);
        }

        // Next edge location on the cell
        int swEdgeIndex = ((int)direction + 1) % 6;
        var swEdge = cell.MyEdges[swEdgeIndex];

        if (swEdge != null)
        {
            edge.Neighbors.Add(swEdge);
            swEdge.Neighbors.Add(edge);
        }

        // The other two edge neighbors will only exist if there is a neighboring cell for that edge
        if (edge.SecondCell != null)
        {
            // Consider this edge, but in the eyes of the neighboring cell.
            // That cell will consider it at an opposite direction.
            var oppositeEdgeDirection = direction.Opposite();

            // Opposite cell, previous edge location
            int seEdgeIndex = ((int)oppositeEdgeDirection + 5) % 6;
            var seEdge = cell.MyEdges[seEdgeIndex];

            if (seEdge != null)
            {
                edge.Neighbors.Add(seEdge);
                seEdge.Neighbors.Add(edge);
            }

            // Opposite cell, next edge location
            int neEdgeIndex = ((int)oppositeEdgeDirection + 1) % 6;
            var neEdge = cell.MyEdges[neEdgeIndex];

            if (neEdge != null)
            {
                edge.Neighbors.Add(neEdge);
                neEdge.Neighbors.Add(edge);
            }

        }
        CreatePossibleEdges(cell);
    }

    public void PlaceEdge_Sandbox(EdgeUnitType edgeUnitType)
    {
        int index = RandomCell.giveCell();
        int randomEdgeDirection = RandomCell.giveDir(Cells[index]);
        var cell = Cells[index];

        // Only place an edge if it's known to be empty
        if (cell.GetEdge(randomEdgeDirection) != null) return;

        // Some casting that will come in handy
        int directionInt = _edgeDirectionDD.value;
        var direction = (HexDirection)directionInt;
        var directionOpposite = direction.Opposite();
        int directionOppositeInt = (int) directionOpposite;
            
        // Instantiate prefab and set unit type (Road/Ship)
        var edge = Instantiate(EdgePrefab);
        edge.MyEdgeUnitType = edgeUnitType;

        if (cell.PossibleEdges[(int) direction] != null)
        {
            var possibleEdge = cell.PossibleEdges[(int) direction];
            cell.PossibleEdges[(int) direction] = null;
            if (cell.GetNeighbor(direction) != null)
            {
                cell.GetNeighbor(direction).PossibleEdges[(int) direction.Opposite()] = null;
            }
            Destroy(possibleEdge.gameObject);
        }
        // From this point onward this function and placeedge_sandbox share the rest
        // So best to have them both call a third helper function to basically not
        // have to edit everything twice.
        PlaceEdge_Continue(cell, directionInt, direction, directionOppositeInt, edge);
    }

    private void CreatePossibleEdges(HexCell cell)
    {
        // For all edges around the cell
        for (int i = 0; i < 6; i++)
        {
            // Extract the edge on location i
            var currentEdge = cell.GetEdge(i);

            // If there is no edge, skip.
            if (currentEdge == null || currentEdge.IsActual == false) continue;

            // Just some casting that is useful.
            var direction = (HexDirection) i;

            // Edge directions for the 4 neighboring edges that an edge can have
            int fcPreviousDirection = ((int) direction + 5) % 6; /* <-- (direction + 5) % 6 = direction - 1 */
            int fcNextDirection = ((int) direction + 1) % 6; 
            int scPreviousDirection = ((int) direction.Opposite() + 5) % 6; /* <-- (direction + 5) % 6 = direction - 1 */
            int scNextDirection = ((int) direction.Opposite() + 1) % 6;

            // Midpoints between the two vertices where the possible edges would fall.
            var fcPreviousPosition = (HexMetrics.corners[fcPreviousDirection] +
                                      HexMetrics.corners[(fcPreviousDirection
                                                          + 1) % 6]) / 2;
            var fcNextPosition = (HexMetrics.corners[fcNextDirection] +
                                  HexMetrics.corners[(fcNextDirection + 1) % 6]) / 2;
            var scPreviousPosition = (HexMetrics.corners[scPreviousDirection] +
                                      HexMetrics.corners[(scPreviousDirection + 1) % 6]) / 2;
            var scNextPosition = (HexMetrics.corners[scNextDirection] +
                                  HexMetrics.corners[(scNextDirection + 1) % 6]) / 2;

            // fcPD --> \ / 
            //  fc       |
            // fcND --> / \ 


            // Checking to see if it's possible for us to extend our possible edges
            // This check includes making sure that we only create a possible edge at a location that is followed
            // by a fully placed edge, and not followed by another possible edge.

            // This check is a fix to issue SETTLERS-10 and is applied to all 4 possible edge placements, below.
            // https://knightsofharambe.myjetbrains.com/youtrack/issue/SETTLERS-10

            if (CanPlacePossibleEdge(currentEdge.FirstCell, fcPreviousDirection) &&
                currentEdge.FirstCell.GetEdge((fcPreviousDirection + 1) % 6) != null &&
                currentEdge.FirstCell.GetEdge((fcPreviousDirection + 1) % 6).IsActual) 
            {
                // First Cell Previous Possible Edge
                var fcPPE = Instantiate(PossibleEdgePrefab);
                fcPPE.IsActual = false;
                fcPPE.name = currentEdge.SecondCell != null
                    ? "fcP Possible Edge between " + currentEdge.FirstCell.CellNumber + " & " +
                      currentEdge.SecondCell.CellNumber
                    : "fcP Possible Edge between " + currentEdge.FirstCell.CellNumber + " & --";
                cell.PossibleEdges[fcPreviousDirection] = fcPPE;
                // If there's a cell in that direction and if that cell already doesn't have
                // a possible edge placed on this location
                if (currentEdge.FirstCell.GetNeighbor(fcPreviousDirection) != null &&
                    currentEdge.FirstCell.GetNeighbor(fcPreviousDirection).PossibleEdges[
                        (int) ((HexDirection) fcPreviousDirection).Opposite()] == null)
                {
                    // Then let that cell know about this possible edge
                    currentEdge.FirstCell.GetNeighbor(fcPreviousDirection).PossibleEdges[
                        (int) ((HexDirection) fcPreviousDirection).Opposite()] = fcPPE;
                }
                // Place it in correct spot in hierarchy
                fcPPE.transform.SetParent(currentEdge.FirstCell.transform.Find("Empty Edges").transform);
                // Position and rotation accordingly
                fcPPE.transform.localPosition = fcPreviousPosition;
                fcPPE.transform.localRotation = EdgeMetrics.rotations[fcPreviousDirection];
                fcPPE.SetPosition_FC(fcPreviousPosition);
                currentEdge.PossibleNeighbors.Add(fcPPE);

                // FIX [SETTLERS-11] Applied to both fcP Possible edge creation and fcN Possible Edge creation
                // https://knightsofharambe.myjetbrains.com/youtrack/issue/SETTLERS-11
                if (currentEdge.SecondCell == null)
                {
                    var neighbor = currentEdge.FirstCell.GetNeighbor(fcPreviousDirection);
                    int scNPEIndex = ((int)((HexDirection)fcPreviousDirection).Opposite() + 5) % 6;
                    if (neighbor != null && CanPlacePossibleEdge(neighbor, scNPEIndex))
                    {
                        var scNPE = Instantiate(PossibleEdgePrefab);
                        scNPE.IsActual = false;
                        scNPE.name = "scP Possible Edge between "; // TODO: FIX NAMING
                        neighbor.PossibleEdges[scNPEIndex] = scNPE;
                        scNPE.transform.SetParent(neighbor.transform.Find("Empty Edges").transform);
                        scNPE.transform.localPosition = (HexMetrics.corners[scNPEIndex] + HexMetrics.corners[(scNPEIndex + 1) % 6]) / 2;
                        scNPE.transform.localRotation = EdgeMetrics.rotations[scNPEIndex];
                        scNPE.SetPosition_FC(scNextPosition);
                        currentEdge.PossibleNeighbors.Add(scNPE);
                    }
                }
            }
            // Same idea as above, different location
            if (CanPlacePossibleEdge(currentEdge.FirstCell, fcNextDirection) &&
                currentEdge.FirstCell.GetEdge((fcNextDirection + 5) % 6) != null &&
                currentEdge.FirstCell.GetEdge((fcNextDirection + 5) % 6).IsActual) 
            {
                // First Cell Next Possible Edge
                var fcNPE = Instantiate(PossibleEdgePrefab);
                currentEdge.FirstCell.PossibleEdges[fcNextDirection] = fcNPE;
                fcNPE.IsActual = false;
                fcNPE.name = currentEdge.SecondCell != null
                    ? "fcN Possible Edge between " + currentEdge.FirstCell.CellNumber + " & " +
                      currentEdge.SecondCell.CellNumber
                    : "fcN Possible Edge between " + currentEdge.FirstCell.CellNumber + " & --";
                if (currentEdge.FirstCell.GetNeighbor(fcNextDirection) != null &&
                    currentEdge.FirstCell.GetNeighbor(fcNextDirection).PossibleEdges[
                        (int) ((HexDirection)fcNextDirection).Opposite()] == null)
                {
                    currentEdge.FirstCell.GetNeighbor(fcNextDirection).PossibleEdges[
                        (int) ((HexDirection) fcNextDirection).Opposite()] = fcNPE;
                }
                fcNPE.transform.SetParent(currentEdge.FirstCell.transform.Find("Empty Edges").transform);
                fcNPE.transform.localPosition = fcNextPosition;
                fcNPE.transform.localRotation = EdgeMetrics.rotations[fcNextDirection];
                fcNPE.SetPosition_FC(fcNextPosition);
                currentEdge.PossibleNeighbors.Add(fcNPE);

                // TODO: Comment code

                // FIX [SETTLERS-11] Applied to both fcP Possible edge creation and fcN Possible Edge creation
                // https://knightsofharambe.myjetbrains.com/youtrack/issue/SETTLERS-11
                if (currentEdge.SecondCell == null)
                {
                    var neighbor = currentEdge.FirstCell.GetNeighbor(fcNextDirection);
                    int scPPEIndex = ((int)((HexDirection)fcNextDirection).Opposite() + 1) % 6;
                    if (neighbor != null && CanPlacePossibleEdge(neighbor, scPPEIndex))
                    {
                        var scPPE = Instantiate(PossibleEdgePrefab);
                        scPPE.IsActual = false;
                        scPPE.name = "scP Possible Edge between "; // TODO: FIX NAMING
                        neighbor.PossibleEdges[scPPEIndex] = scPPE;
                        scPPE.transform.SetParent(neighbor.transform.Find("Empty Edges").transform);
                        scPPE.transform.localPosition = (HexMetrics.corners[scPPEIndex] + HexMetrics.corners[(scPPEIndex + 1) % 6]) / 2;
                        scPPE.transform.localRotation = EdgeMetrics.rotations[scPPEIndex];
                        scPPE.SetPosition_FC(scPreviousPosition);
                        currentEdge.PossibleNeighbors.Add(scPPE);
                    }
                }
            }

            // We can't continue if there's no second cell, because the next two possible edges
            // Would only exist if there's a second cell to place them on

            //     \ / <-- scND
            //      |   sc
            //     / \ <-- scPD

            if (currentEdge.SecondCell == null) continue;

            // Same concept as those for the first cell
            if (CanPlacePossibleEdge(currentEdge.SecondCell, scPreviousDirection) &&
                currentEdge.SecondCell.GetEdge((scPreviousDirection + 1) % 6) != null &&
                currentEdge.SecondCell.GetEdge((scPreviousDirection + 1) % 6).IsActual) 
            {
                // Second Cell Previous Possible Edge
                var scPPE = Instantiate(PossibleEdgePrefab);
                currentEdge.SecondCell.PossibleEdges[scPreviousDirection] = scPPE;
                scPPE.IsActual = false;
                scPPE.name = currentEdge.SecondCell != null
                    ? "scP Possible Edge between " + currentEdge.FirstCell.CellNumber + " & " +
                      currentEdge.SecondCell.CellNumber
                    : "scP Possible Edge between " + currentEdge.FirstCell.CellNumber + " & --";

                if (currentEdge.SecondCell.GetNeighbor(scPreviousDirection) != null &&
                    currentEdge.SecondCell.GetNeighbor(scPreviousDirection).PossibleEdges[
                        (int) ((HexDirection) scPreviousDirection).Opposite()] == null)
                {
                    currentEdge.SecondCell.GetNeighbor(scPreviousDirection).PossibleEdges[
                        (int) ((HexDirection) scPreviousDirection).Opposite()] = scPPE;
                }
                scPPE.transform.SetParent(currentEdge.SecondCell.transform.Find("Empty Edges").transform);
                scPPE.transform.localPosition = scPreviousPosition;
                scPPE.transform.localRotation = EdgeMetrics.rotations[scPreviousDirection];
                scPPE.SetPosition_FC(scPreviousPosition);
                currentEdge.PossibleNeighbors.Add(scPPE);
            }

            // And again
            if (CanPlacePossibleEdge(currentEdge.SecondCell, scNextDirection) &&
                currentEdge.SecondCell.GetEdge((scNextDirection + 5) % 6) != null &&
                currentEdge.SecondCell.GetEdge((scNextDirection + 5) % 6).IsActual)
            {
                // Second Cell Next Possible Edge
                var scNPE = Instantiate(PossibleEdgePrefab);
                currentEdge.SecondCell.PossibleEdges[scNextDirection] = scNPE;
                scNPE.IsActual = false;
                scNPE.name = currentEdge.SecondCell != null
                    ? "scN Possible Edge between " + currentEdge.FirstCell.CellNumber + " & " +
                      currentEdge.SecondCell.CellNumber
                    : "scN Possible Edge between " + currentEdge.FirstCell.CellNumber + " & --";

                if (currentEdge.SecondCell.GetNeighbor(scNextDirection) != null &&
                    currentEdge.SecondCell.GetNeighbor(scNextDirection).PossibleEdges[
                        (int) ((HexDirection) scNextDirection).Opposite()] == null)
                {
                    currentEdge.SecondCell.GetNeighbor(scNextDirection).PossibleEdges[
                        (int) ((HexDirection) scNextDirection).Opposite()] = scNPE;
                }
                scNPE.transform.SetParent(currentEdge.SecondCell.transform.Find("Empty Edges").transform);
                scNPE.transform.localPosition = scNextPosition;
                scNPE.transform.localRotation = EdgeMetrics.rotations[scNextDirection];
                scNPE.SetPosition_FC(scNextPosition);
                currentEdge.PossibleNeighbors.Add(scNPE);
            }
        }
    }

    private static bool CanPlacePossibleEdge(HexCell cell, int direction)
    {
        // Return false if there's ANY possible edge in the list of possible edges
        // that has the same position as the one you're trying to place.
        // Essentially the method I prevent dups for possible edges.
        return !cell.HasEdgeAtDirection(direction) && !cell.HasPossibleEdgeAtDirection(direction);
    }

    // Straightforward method that is meant for debugging purposes to be able
    // to echo out the neighbors of each cell and make sure that it actually sees its neighbors!
    private void EchoNeighbors(Vector3 position)
    {
        
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * Width + coordinates.Z / 2;
        Debug.Log("My cell number is: " + Cells[index].CellNumber);
        if (Cells[index].GetNeighbor(HexDirection.NE) != null)
        {
            Debug.Log("My NE neighbor is: " + Cells[index].GetNeighbor(HexDirection.NE).CellNumber);
        }
        if (Cells[index].GetNeighbor(HexDirection.E) != null)
        {
            Debug.Log("My E neighbor is: " + Cells[index].GetNeighbor(HexDirection.E).CellNumber);
        }
        if (Cells[index].GetNeighbor(HexDirection.SE) != null)
        {
            Debug.Log("My SE neighbor is: " + Cells[index].GetNeighbor(HexDirection.SE).CellNumber);
        }
        if (Cells[index].GetNeighbor(HexDirection.SW) != null)
        {
            Debug.Log("My SW neighbor is: " + Cells[index].GetNeighbor(HexDirection.SW).CellNumber);
        }
        if (Cells[index].GetNeighbor(HexDirection.W) != null)
        {
            Debug.Log("My W neighbor is: " + Cells[index].GetNeighbor(HexDirection.W).CellNumber);
        }
        if (Cells[index].GetNeighbor(HexDirection.NW) != null)
        {
            Debug.Log("My NW neighbor is: " + Cells[index].GetNeighbor(HexDirection.NW).CellNumber);
        }
        
    }

    // Method for triangulating all cells within the "cells" array of a grid.
    private static void Triangulate(HexCell[] hexCells)
    {
        foreach (var cell in hexCells)
        {
            if (cell != null)
            {
                cell.Triangulate();
            }
        }
    }
}
