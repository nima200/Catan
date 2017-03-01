using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public HexCell CellPrefab;

    public Text CellLabelPrefab;
    public Canvas GridCanvas;
    public Canvas UserInterface;
    public Toggle BuildRoadButton;
    public Toggle BuildShipButton;
    public Toggle BuildSettleButton;
    public Toggle BuildCityButton;
    public HexEdge EdgePrefab;
    public HexVertex VertexPrefab;
    public Dropdown DirectionDropdown;

	private static BoardManager _instance;

    public HexCell[] Cells { get; private set; }
    
    private const int Width = 8;
    private const int Height = 7;
    private int[] _tokens;
    private TurnPhase _phase = TurnPhase.Sandbox1;
    private BuildMode _buildMode = BuildMode.Off;

    private void Awake()
    {
		//creates singleton on awake
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

        MakeTokens();
        Cells = new HexCell[Height * Width];
        DirectionDropdown = UserInterface.GetComponentInChildren<Dropdown>();
        DirectionDropdown.interactable = false;
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
	// Access singleton 
	public static BoardManager GetInstance()
	{
		return _instance;
	}

    private void Start()
    {
        Triangulate(Cells);
        Trim();
        AssignTokens();
        CreateVertices();
        CreateEdges();
        StartGame();
    }

    private void Update()
    {
        // Mouse Button 0 = Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    private void StartGame()
    {
        // We're in phase 1 so we can just get started with building a settlement
        Build("Settlement");
    }

    private void Trim()
    {
        int[] numbers = { 0, 1, 7, 8, 15, 16, 32, 40, 47, 48, 49, 55 };

        foreach (int i in numbers)
        {

            //Debug.Log(cells[i]);
            for (int j = 0; j < 6; j++)
            {
                if (Cells[i].GetNeighbor(j) != null)
                {

                    Cells[i].Neighbors[j].Neighbors[(int)((HexDirection)j).Opposite()] = null;
                }
            }
            Destroy(Cells[i].gameObject);
            Destroy(Cells[i].Label.gameObject);
            Cells[i] = null;
        }
    }

    public void Build(string modeName)
    {
        _buildMode = (BuildMode) Enum.Parse(typeof(BuildMode), modeName);
        switch (_buildMode)
        {
            case BuildMode.Off:
                HidePossibleEdgeUnits();
                HidePossibleCornerUnits();
                UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text = "";
                DirectionDropdown.interactable = false;
                break;
            case BuildMode.Settlement:
                HidePossibleEdgeUnits();
                ShowPossibleCornerUnits();
                DirectionDropdown.interactable = true;
                UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text =
                    "Place yourself a settlement!";
                break;
            case BuildMode.City:
                HidePossibleEdgeUnits();
                ShowPossibleCornerUnits();
                DirectionDropdown.interactable = true;
                UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text =
                    "Place yourself a city!";
                break;
            case BuildMode.Road:
                ShowPossibleEdgeUnits();
                HidePossibleCornerUnits();
                DirectionDropdown.interactable = true;
                UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text =
                    "Place yourself a road";
                break;
            case BuildMode.Ship:
                ShowPossibleEdgeUnits();
                HidePossibleCornerUnits();
                DirectionDropdown.interactable = true;
                UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text =
                    "Place yourself a ship!";
                break;
            case BuildMode.Knight:
                break;
        }
    }

    private void HandleInput()
    {
        var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(inputRay, out hit)) return;
        if (_buildMode == BuildMode.Road)
        {
            BuildEdgeUnit(hit.point, (HexDirection) DirectionDropdown.value, EdgeUnit.Road);
        }
        if (_buildMode == BuildMode.Ship)
        {
            BuildEdgeUnit(hit.point, (HexDirection) DirectionDropdown.value, EdgeUnit.Ship);
        }
        if (_buildMode == BuildMode.Settlement)
        {
            BuildCornerUnit(hit.point, (HexDirection) DirectionDropdown.value, CornerUnit.Settlement);
        }
        if (_buildMode == BuildMode.City)
        {
            BuildCornerUnit(hit.point, (HexDirection) DirectionDropdown.value, CornerUnit.City);
        }
    }

    private void MakeTokens()
    {
        _tokens = TokenGenerator.generate(44);
    }

    private void AssignTokens()
    {
        int tokenIndex = 0;
        foreach (var t in Cells)
        {
            if (t == null) continue;
            t.CellNumber = _tokens[tokenIndex];
            t.Label.text = t.CellNumber.ToString();
            t.gameObject.name = "Hex " + t.CellNumber.ToString();
            tokenIndex++;
        }
    }

    private void ShowPossibleEdgeUnits()
    {
        // Look at all cells
        foreach (var cell in Cells)
        {
            // For those deleted by boardtrimmer
            if (cell == null) continue;

            // For SANDBOX mode.
            if (_phase == TurnPhase.Sandbox1 || _phase == TurnPhase.Sandbox2)
            {
                foreach (var vertex in cell.MyVertices)
                {
                    // Only take care of city and settlement vertices
                    if (vertex.Type != CornerUnit.Settlement && vertex.Type != CornerUnit.City) continue;

                    // For all neighboring edges of a vertex
                    foreach (var edge in vertex.NeighborEdges)
                    {
                        // We only want to show those edges currently hidden
                        if (edge.Type != EdgeUnit.Hidden) continue;

                        switch (_phase)
                        {
                            // SANDBOX: make edges around settlements open
                            case TurnPhase.Sandbox1:
                                edge.Type = EdgeUnit.Open;
                                break;
                            // SANDBOX: make edges around cities open
                            case TurnPhase.Sandbox2:
                                edge.Type = vertex.Type == CornerUnit.City ? EdgeUnit.Open : EdgeUnit.Hidden;
                                break;
                            // Handled phase 3 below
                            case TurnPhase.Build:
                                break;
                        }

                    }
                }
            }
            // Phase 3: The rest of build process throughout the game
            else
            {
                // TODO: Apply the ownership stuff here too.
                // Edges have to follow edges
                foreach (var edge in cell.MyEdges)
                {
                    // Only take care of roads and ships (not hidden, etc)
                    if (edge.Type != EdgeUnit.Road && edge.Type != EdgeUnit.Ship) continue;

                    foreach (var neighbor in edge.Neighbors)
                    {
                        // Make all neighbors available to place on
                        if (neighbor.Type == EdgeUnit.Hidden /* TODO: && head.Owner == currentPlayer */)
                        {
                            neighbor.Type = EdgeUnit.Open;
                        }
                    }
                }
            }
         }
     }

    private void ShowPossibleCornerUnits()
    {
        // Look at all cells
        foreach (var cell in Cells)
        {
            // For those deleted by the boardtrimmer
            if (cell == null) continue;

            switch (_phase)
            {
                // In phase 1 every vertex of the board is open
                // Except if a player before you placed something
                case TurnPhase.Sandbox1:
                    foreach (var vertex in cell.MyVertices)
                    {
                        switch (vertex.Type)
                        {
                            case CornerUnit.Disabled:
                                break;
                            case CornerUnit.Open:
                                break;
                            case CornerUnit.Settlement:
                                foreach (var neighbor in vertex.Neighbors)
                                {
                                    neighbor.Type = CornerUnit.Disabled;
                                }
                                break;
                            case CornerUnit.City:
                                break;
                            case CornerUnit.Hidden:
                                vertex.Type = CornerUnit.Open;
                                break;
                        }
                    }
                    break;
                // In phase 2, consider all settlements are placed in phase 1 <-- apply catan rules
                case TurnPhase.Sandbox2:
                    foreach (var vertex in cell.MyVertices)
                    {
                        switch (vertex.Type)
                        {
                            // The rule is so that within phase 2, when a city is placed
                            // no one can place a city/road within 1 distance of that city
                            case CornerUnit.City:
                                foreach (var neighbor in vertex.Neighbors)
                                {
                                    neighbor.Type = CornerUnit.Disabled;
                                }
                                break;
                            case CornerUnit.Settlement:
                                foreach (var neighbor in vertex.Neighbors)
                                {
                                    neighbor.Type = CornerUnit.Disabled;
                                }
                                break;
                            // Open up all hidden cells for the next placement
                            case CornerUnit.Hidden:
                                vertex.Type = CornerUnit.Open;
                                break;
                            case CornerUnit.Disabled:
                                break;
                            case CornerUnit.Open:
                                break;
                        }
                    }
                    break;
                case TurnPhase.Build:
                    // For all vertices
                    foreach (var vertex in cell.MyVertices)
                    {
                        switch (vertex.Type)
                        {
                            case CornerUnit.Disabled:
                                break;
                            case CornerUnit.Open:
                                break;
                            // If there's a settlement placed, disable all its neighbors
                            case CornerUnit.Settlement:
                                foreach (var neighbor in vertex.Neighbors)
                                {
                                    neighbor.Type = CornerUnit.Disabled;
                                }
                                break;
                            // If there's a city placed, disable all its neighbors
                            case CornerUnit.City:
                                foreach (var neighbor in vertex.Neighbors)
                                {
                                    neighbor.Type = CornerUnit.Disabled;
                                }
                                break;
                            case CornerUnit.Hidden:
                                break;
                        }
                    }
                    // For all edges
                    foreach (var edge in cell.MyEdges)
                    {
                        switch (edge.Type)
                        {
                            case EdgeUnit.Disabled:
                                break;
                            case EdgeUnit.Open:
                                break;
                            case EdgeUnit.Road:
                                foreach (var head in edge.Heads)
                                {
                                    if (head.Type == CornerUnit.Hidden)
                                    {
                                        head.Type = CornerUnit.Open;
                                    }
                                }
                                break;
                            case EdgeUnit.Ship:
                                foreach (var head in edge.Heads)
                                {
                                    if (head.Type == CornerUnit.Hidden)
                                    {
                                        head.Type = CornerUnit.Open;
                                    }
                                }
                                break;
                            // Only if there's a hidden vertex, show it.
                            case EdgeUnit.Hidden:
                                break;
                        }
                    }
                    break;
            }
        }
    }

    private void HidePossibleEdgeUnits()
    {
        // Simply hide every current open edge
        foreach (var cell in Cells)
        {
            if (cell == null) continue;
            foreach (var edge in cell.MyEdges)
            {
                if (edge.Type == EdgeUnit.Open)
                {
                    edge.Type = EdgeUnit.Hidden;
                }
            }
        }
    }

    private void HidePossibleCornerUnits()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;
            foreach (var vertex in cell.MyVertices)
            {
                switch (vertex.Type)
                {
                    case CornerUnit.Disabled:
                        break;
                    // If there's an open (possible) edge shown, hide it
                    case CornerUnit.Open:
                        vertex.Type = CornerUnit.Hidden;
                        break;
                    case CornerUnit.Settlement:
                        break;
                    case CornerUnit.City:
                        break;
                    case CornerUnit.Hidden:
                        break;
                }
            }
        }
    }

    private void CreateCell(int x, int z, int i)
    {
         /*distance between adjacent hexagon cells in the x direction is equal to twice the inner radius of the hex
         distance between adjacent hexagon cells in the z direction (distance between two rows) is equal to 1.5 times the outer radius

         also, hex rows are not directly on top of each other. Each row is offset along the X axis by the inner radius.
         however we need to bring them back cause if for every row we only add half of z, then it just turns into a rhombus in the overall shape
         to get the brick stack effect,  we need to bring back the x offset of every "other" row.

         subtracting by the INT division makes this awesome cause we can essentially subtract every time but the effect is only applied
         once every other row.*/

        //HexVertex position;
        float xCoord = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        float yCoord = 0f;
        float zCoord = z * (HexMetrics.outerRadius * 1.5f);

		var position = new Vector3(xCoord, yCoord, zCoord);

        var cell = Cells[i] = Instantiate<HexCell>(CellPrefab);

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
        label.rectTransform.SetParent(GridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
    }

    private void CreateVertices()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;

            // For all vertex locations of a cell
            for (int i = 0; i < 6; i++)
            {
                // Prevent duplicates (i.e. for all edge locations of a cell that are empty)
                if (cell.MyVertices[i] != null) continue;
                // Create a vertex
                var vertex = Instantiate(VertexPrefab);
                // Assign the location
                cell.MyVertices[i] = vertex;
                // Assign the parent and in game position w.r.t. the cell it belongs to
                vertex.transform.SetParent(cell.transform);
                vertex.transform.localPosition = HexMetrics.corners[i];
                // If there's a neighboring cell at direction i then it needs a reference to this vertex created
                if (cell.GetNeighbor(i) != null)
                {
                    cell.GetNeighbor(i).MyVertices[(int)(((HexDirection)i).Opposite() + 1) % 6] = vertex;
                }
                // If there's a neighboring cell at direction i - 1 (i.e. (i + 5) % 6) then it needs a reference to this vertex created
                if (cell.GetNeighbor((i + 5) % 6) != null)
                {
                    cell.GetNeighbor((i + 5) % 6).MyVertices[(int)(((HexDirection)i).Opposite() + 5) % 6] = vertex;
                }
            }
        }
        AssignVertexNeighbors();
    }

    private void CreateEdges()
    {
        // For all cells
        foreach (var cell in Cells)
        {
            // For taking care of borders
            if (cell == null) continue;

            // For each edge of the hexagon
            for (int i = 0; i < 6; i++)
            {
                // If there's not already a HexEdge there
                if (cell.MyEdges[i] != null) continue;
                // Then create it
                var edge = Instantiate(EdgePrefab);
                // Let the cell know about it
                cell.MyEdges[i] = edge;

                /* VERTEX - EDGE NEIGHBOR STRUCTURE:
                 * 
                 *            |
                 *            v
                 *           / \
                 *  v needs to know about the 3 edges around it
                 */

                // If the cell's vertex at i doesn't have this edge that just got created as a neighbor
                if (!cell.MyVertices[i].NeighborEdges.Contains(edge))
                {
                    // Then add it
                    cell.MyVertices[i].NeighborEdges.Add(edge);
                }
                // And also the cell's vertex at i + 1, since an edge connects two vertices at all times
                if (!cell.MyVertices[(i + 1) % 6].NeighborEdges.Contains(edge))
                {
                    cell.MyVertices[(i + 1) % 6].NeighborEdges.Add(edge);
                }

                /* VERTEX-EDGE-VERTEX NEIGHBOR STRUCTURE:
                 * An edge needs to know about the two vertices that are its heads
                 */

                // So if it doesn't already know about it
                if (!edge.Heads.Contains(cell.MyVertices[i]))
                {
                    // Then add the link
                    edge.Heads.Add(cell.MyVertices[i]);
                }
                // Same story here, except about the other head
                if (!edge.Heads.Contains(cell.MyVertices[(i + 1) % 6]))
                {
                    edge.Heads.Add(cell.MyVertices[(i + 1) % 6]);
                }

                // Place the edge under the correct hierarchy location
                edge.transform.SetParent(cell.transform.Find("My Edges"));
                // Place it at the correct position in local space, with respect to the direction it was placed on the cell
                edge.transform.localPosition = (HexMetrics.corners[i] + HexMetrics.corners[(i + 1) % 6]) / 2;
                // And also apply the respective rotations
                edge.transform.localRotation = EdgeMetrics.rotations[i];
                // If a neighbor at that direction exists
                if (cell.GetNeighbor(i) != null)
                {
                    // Then let the neighboring cell know about this edge placed so it's shared between them.
                    cell.GetNeighbor(i).MyEdges[(int)(((HexDirection)i).Opposite()) % 6] = edge;
                }
            }
        }
        AssignEdgeNeighbors();
    }

    private void BuildCornerUnit(Vector3 position, HexDirection direction, CornerUnit unitType)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * Width + coordinates.Z / 2;

        if (index > Cells.Length - 1) return;

        var cell = Cells[index];

        int directionInt = (int) direction;

        // Prevent duplicates
        if (!cell.MyVertices[directionInt].Type.Equals(CornerUnit.Open)) return;

        switch (_phase)
        {
            // After placing a corner unit, if it was in phase 1 or 2, build a road.
            case TurnPhase.Sandbox1:
            case TurnPhase.Sandbox2:
                if (cell.MyVertices[directionInt].Type != CornerUnit.Open) return;
                cell.MyVertices[directionInt].Type = unitType;
                // cell.MyVertices[directionInt].owner = turnmanager.currentPlayer;  <-- Charlotte apply this when you are trying to make sure that a player has link to the vertices he places.
                Build("Road");
                break;
            // If it was phase 3, turn off the build menu
            case TurnPhase.Build:
                UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text = "";
                if (cell.MyVertices[directionInt].Type != CornerUnit.Open) return;
                cell.MyVertices[directionInt].Type = unitType;
                Build("Off");
                break;
        }
    }

    private void BuildEdgeUnit(Vector3 position, HexDirection direction, EdgeUnit unitType)
    {
        // Transform mouse hit location into a cell index
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * Width + coordinates.Z / 2;

        // If OOB, end.
        if (index > Cells.Length - 1) return;

        // If not, extract the cell
        var cell = Cells[index];

        // Just some useful unitType casting as usual
        int directionInt = (int)direction;

        // If it's not 'possible' to place an edge (i.e. if the edge unitType is not 'open) then end.
        if (!cell.MyEdges[directionInt].Type.Equals(EdgeUnit.Open)) return;

        // Else, can build on that location. So set the unitType to the built unitType passed to function (road/ship/etc...)
        cell.MyEdges[directionInt].Type = unitType;
        // Place it in the correct rotation around the cell while you're at it
        cell.MyEdges[directionInt].gameObject.transform.localRotation = EdgeMetrics.rotations[directionInt];
        // Lil hack to resolve ship rotations not appearing correctly as their .fbx model file was screwed up with rotations
        if (unitType == EdgeUnit.Ship)
        {
            cell.MyEdges[directionInt].gameObject.transform.localRotation *= Quaternion.Euler(0f, 90f, 0f);
        }


        switch (_phase)
        {
            // If done with phase 1, we can proceed to phase 2 and just build a city after the first road was placed in 'sandbox'
            case TurnPhase.Sandbox1:
                _phase = TurnPhase.Sandbox2;
                Build("City");
                break;
            // If done with phase 2, we can proceed to phase 3 and just end the build after the second road placed in 'sandbox'
            case TurnPhase.Sandbox2:
                 _phase = TurnPhase.Build;
//                _phase = TurnPhase.WaitForTurn;
                Build("Off");
                break;
            // And if in phase 3, then we just apply the routine end build state after building a road anywhere
            case TurnPhase.Build:
                Build("Off");
                break;
            case TurnPhase.WaitForTurn:
                break;
        }
    }

    private void AssignVertexNeighbors()
    {
        // For all cells
        foreach (var cell in Cells)
        {
            // For taking care of borders
            if (cell == null) continue;

            // For all vertices
            for (int i = 0; i < 6; i++)
            {
                // Extract the cell
                var vertex = cell.MyVertices[i];
                // Set a cross link to its cell's next vertex, if not already established
                if (!vertex.Neighbors.Contains(cell.MyVertices[(i + 1) % 6]))
                {
                    vertex.Neighbors.Add(cell.MyVertices[(i + 1) % 6]);
                    cell.MyVertices[(i + 1) % 6].Neighbors.Add(vertex);
                }
                // Set a cross link to its cell's previous vertex, if not already established
                if (vertex.Neighbors.Contains(cell.MyVertices[(i + 5) % 6])) continue;

                vertex.Neighbors.Add(cell.MyVertices[(i + 5) % 6]);
                cell.MyVertices[(i + 5) % 6].Neighbors.Add(vertex);
            }
        }
    }

    private void AssignEdgeNeighbors()
    {
        // For all cells
        foreach (var cell in Cells)
        {
            // For taking care of the borders
            if (cell == null) continue;

            // For all edges of a cell
            for (int i = 0; i < 6; i++)
            {
                // Extract the edge
                var edge = cell.MyEdges[i];
                // If the edge doesn't already know about its cell's next edge
                if (!edge.Neighbors.Contains(cell.MyEdges[(i + 1) % 6]))
                {
                    // Then add a cross reference between them
                    edge.Neighbors.Add(cell.MyEdges[(i + 1) % 6]);
                    cell.MyEdges[(i + 1) % 6].Neighbors.Add(edge);
                }
                // If the edge doesn't already know about its cell's previous edge
                if (!edge.Neighbors.Contains(cell.MyEdges[(i + 5) % 6]))
                {
                    // Then add a cross reference between them
                    edge.Neighbors.Add(cell.MyEdges[(i + 5) % 6]);
                    cell.MyEdges[(i + 5) % 6].Neighbors.Add(edge);
                }
                // Some unitType casting that's useful below
                var directionOpposite = ((HexDirection) i).Opposite();
                int directionOppositeInt = (int) directionOpposite;

                // Let's only look at border cells
                if (cell.GetNeighbor(i) != null) continue;
                // And among those let's only look at those that have a neighbor in the next position <-- further narrows down 'border cells' to the ones we need
                if (cell.GetNeighbor((i + 1) % 6) == null) continue;
                // If we already know about that edge that belongs to the next neighbor, then nvm.
                if (edge.Neighbors.Contains(cell.GetNeighbor((i + 1) % 6).MyEdges[(directionOppositeInt + 2) % 6])) continue;
                // But if we don't know about it, well let's know about it!
                // i.e. cross reference like usual
                edge.Neighbors.Add(cell.GetNeighbor((i + 1) % 6).MyEdges[(directionOppositeInt + 2) % 6]);
                cell.GetNeighbor((i + 1) % 6).MyEdges[(directionOppositeInt + 2) % 6].Neighbors.Add(edge);
            }
        }
    }

    public void WaitForTurn()
    {
        _phase = TurnPhase.WaitForTurn;
        UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text = "Please wait for your turn";
    }

    public void IsTurn()
    {
        UserInterface.GetComponentInChildren<Instruction>().GetComponent<Text>().text = "It is now your turn!";
    }
    
    private static void Triangulate(IEnumerable<HexCell> hexCells)
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