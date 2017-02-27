using System;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{

    public int Width = 8;
    public int Height = 7;
    public HexCell CellPrefab;
    public Text CellLabelPrefab;
    public Canvas GridCanvas;
    public Canvas UserInterface;
    public Toggle BuildRoadButton;
    public Toggle BuildShipButton;
    public Toggle BuildSettleButton;
    public Toggle BuildCityButton;
    public ToggleGroup ButtonGroup;
    public HexEdge EdgePrefab;
    public HexVertex VertexPrefab;
    public HexCell[] Cells { get; private set; }
    public Dropdown DirectionDropdown;

    private int[] _tokens;
    private SandboxPhase _phase = SandboxPhase.Phase1;
    private BuildMode _buildMode = BuildMode.Off;

    private void Awake()
    {
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



    public void Build(string modeName)
    {
        _buildMode = (BuildMode) Enum.Parse(typeof(BuildMode), modeName);
        switch (_buildMode)
        {
            case BuildMode.Off:
                HidePossibleEdges();
                HidePossibleCornerUnits();
                BuildRoadButton.GetComponentInChildren<Text>().text = "Build Road";
                BuildSettleButton.GetComponentInChildren<Text>().text = "Build Settlement";
                BuildCityButton.GetComponentInChildren<Text>().text = "Build City";
                BuildShipButton.GetComponentInChildren<Text>().text = "Build Ship";

                DirectionDropdown.interactable = false;
//                BuildRoadButton.interactable = true;
//                BuildSettleButton.interactable = true;
//                BuildCityButton.interactable = true;
//                BuildShipButton.interactable = true;
                break;
            case BuildMode.Settlement:
                HidePossibleEdges();
                ShowPossibleCornerUnits();
                BuildRoadButton.GetComponentInChildren<Text>().text = "Build Road";
                BuildSettleButton.GetComponentInChildren<Text>().text = "End Build";
                BuildCityButton.GetComponentInChildren<Text>().text = "Build City";
                BuildShipButton.GetComponentInChildren<Text>().text = "Build Ship";

                DirectionDropdown.interactable = true;
//                BuildRoadButton.interactable = false;
//                BuildSettleButton.interactable = true;
//                BuildCityButton.interactable = false;
//                BuildShipButton.interactable = false;
                break;
            case BuildMode.City:
                HidePossibleEdges();
                ShowPossibleCornerUnits();
                BuildRoadButton.GetComponentInChildren<Text>().text = "Build Road";
                BuildSettleButton.GetComponentInChildren<Text>().text = "Build Settlement";
                BuildCityButton.GetComponentInChildren<Text>().text = "End Build";
                BuildShipButton.GetComponentInChildren<Text>().text = "Build Ship";

                DirectionDropdown.interactable = true;
//                BuildRoadButton.interactable = false;
//                BuildSettleButton.interactable = false;
//                BuildCityButton.interactable = true;
//                BuildShipButton.interactable = false;
                break;
            case BuildMode.Road:
                ShowPossibleEdges();
                HidePossibleCornerUnits();
                BuildRoadButton.GetComponentInChildren<Text>().text = "End Build";
                BuildSettleButton.GetComponentInChildren<Text>().text = "Build Settlement";
                BuildCityButton.GetComponentInChildren<Text>().text = "Build City";
                BuildShipButton.GetComponentInChildren<Text>().text = "Build Ship";

                DirectionDropdown.interactable = true;
//                BuildRoadButton.interactable = true;
//                BuildSettleButton.interactable = false;
//                BuildCityButton.interactable = false;
//                BuildShipButton.interactable = false;
                break;
            case BuildMode.Ship:
                ShowPossibleEdges();
                HidePossibleCornerUnits();
                BuildRoadButton.GetComponentInChildren<Text>().text = "Build Road";
                BuildSettleButton.GetComponentInChildren<Text>().text = "Build Settlement";
                BuildCityButton.GetComponentInChildren<Text>().text = "Build City";
                BuildShipButton.GetComponentInChildren<Text>().text = "End Build";

                DirectionDropdown.interactable = true;
//                BuildRoadButton.interactable = false;
//                BuildSettleButton.interactable = false;
//                BuildCityButton.interactable = false;
//                BuildShipButton.interactable = true;
                break;
            case BuildMode.Knight:
                break;
        }
    }
    // TODO: Call sandbox, when on phase 3, stay there and apply normal functionality of building stuff.
    public void StartGame()
    {
        ShowPossibleCornerUnits();
        
    }

    // Very generic mouse input handle method.
    // Check this out for more info
    // https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
    private void HandleInput()
    {
        var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(inputRay, out hit)) return;
        if (_buildMode == BuildMode.Road)
        {
            BuildEdgeUnit_Sandbox(hit.point, (HexDirection) DirectionDropdown.value, EdgeUnit.Road);
        }
        if (_buildMode == BuildMode.Ship)
        {
            BuildEdgeUnit_Sandbox(hit.point, (HexDirection) DirectionDropdown.value, EdgeUnit.Ship);
        }
        if (_buildMode == BuildMode.Settlement)
        {
            BuildCornerUnit_Sandbox(hit.point, (HexDirection) DirectionDropdown.value);
        }
        if (_buildMode == BuildMode.City)
        {
            BuildCornerUnit_Sandbox(hit.point, (HexDirection) DirectionDropdown.value);
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
        foreach (var t in Cells)
        {
            if (t == null) continue;
            t.CellNumber = _tokens[tokenIndex];
            t.Label.text = t.CellNumber.ToString();
            t.gameObject.name = "Hex " + t.CellNumber.ToString();
            tokenIndex++;
        }
    }

    private void ShowPossibleEdges()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;
            foreach (var vertex in cell.MyVertices)
            {
                if (vertex.Type != CornerUnit.Settlement && vertex.Type != CornerUnit.City) continue;
                foreach (var edge in vertex.MyEdges)
                {
                    if (edge.Type != EdgeUnit.Disabled) continue;

                    if (_phase == SandboxPhase.Phase2)
                    {
                        edge.Type = vertex.Type == CornerUnit.City ? EdgeUnit.Open : EdgeUnit.Disabled;
                    }
                    else
                    {
                        edge.Type = EdgeUnit.Open;
                    }
                }
            }
         }
     }

    public void HidePossibleEdges()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;
            foreach (var edge in cell.MyEdges)
            {
                if (edge.Type == EdgeUnit.Open)
                {
                    edge.Type = EdgeUnit.Disabled;
                }
            }
        }
    }

    private void ShowPossibleCornerUnits()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;
            foreach (var vertex in cell.MyVertices)
            {
                if (_phase == SandboxPhase.Phase1)
                {
                    if (vertex.Type == CornerUnit.Hidden)
                    {
                        vertex.Type = CornerUnit.Open;

                    }
                } else if (_phase == SandboxPhase.Phase2 &&
                         (vertex.Type == CornerUnit.City || vertex.Type == CornerUnit.Settlement))
                {
                    foreach (var neighbor in vertex.Neighbors)
                    {
                        neighbor.Type = CornerUnit.Disabled;
                    }
                } else if (_phase == SandboxPhase.Phase2 && vertex.Type == CornerUnit.Hidden)
                {
                    vertex.Type = CornerUnit.Open;
                } else if (_phase == SandboxPhase.Phase3)
                {
                    
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
                if (vertex.Type == CornerUnit.Open)
                {
                    vertex.Type = CornerUnit.Hidden;
                }
            }
        }
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

    public void BuildCornerUnit_Sandbox(Vector3 position, HexDirection direction)
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
            case SandboxPhase.Phase1:
                if (cell.MyVertices[directionInt].Type == CornerUnit.Open)
                {
                    cell.MyVertices[directionInt].Type = CornerUnit.Settlement;
                    Build("Road");
                }
                break;
            case SandboxPhase.Phase2:
                if (cell.MyVertices[directionInt].Type == CornerUnit.Open)
                {
                    cell.MyVertices[directionInt].Type = CornerUnit.City;
                    Build("Road");
                }
                break;
            case SandboxPhase.Phase3:
                break;
        }
        
    }

    public void CreateVertices()
	{
		foreach (var cell in Cells)
		{
		    if (cell == null) continue;

		    for (int i = 0; i < 6; i++)
		    {
		        if (cell.MyVertices[i] != null) continue;
		        var vertex = Instantiate(VertexPrefab);
		        cell.MyVertices[i] = vertex;
		        vertex.transform.SetParent(cell.transform);
		        vertex.transform.localPosition = HexMetrics.corners[i];
		        if (cell.GetNeighbor(i) != null)
		        {
		            cell.GetNeighbor(i).MyVertices[(int)(((HexDirection)i).Opposite() + 1) % 6] = vertex;
		        }
		        if (cell.GetNeighbor((i + 5) % 6) != null)
		        {
		            cell.GetNeighbor((i + 5) % 6).MyVertices[(int)(((HexDirection)i).Opposite() + 5) % 6] = vertex;
		        }
		    }
		}
	    AssignVertexNeighbors();
	}

    private void AssignVertexNeighbors()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;

            for (int i = 0; i < 6; i++)
            {
                var vertex = cell.MyVertices[i];
                if (!vertex.Neighbors.Contains(cell.MyVertices[(i + 1) % 6]))
                {
                    vertex.Neighbors.Add(cell.MyVertices[(i + 1) % 6]);
                    cell.MyVertices[(i + 1) % 6].Neighbors.Add(vertex);
                }
                if (vertex.Neighbors.Contains(cell.MyVertices[(i + 5) % 6])) continue;

                vertex.Neighbors.Add(cell.MyVertices[(i + 5) % 6]);
                cell.MyVertices[(i + 5) % 6].Neighbors.Add(vertex);
            }
        
        }
    }

    public void CreateEdges()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;

            for (int i = 0; i < 6; i++)
            {
                if (cell.MyEdges[i] != null) continue;
                var edge = Instantiate(EdgePrefab);
                cell.MyEdges[i] = edge;

                if (!cell.MyVertices[i].MyEdges.Contains(edge))
                {
                    cell.MyVertices[i].MyEdges.Add(edge);
                }
                if (!cell.MyVertices[(i + 1) % 6].MyEdges.Contains(edge))
                {
                    cell.MyVertices[(i + 1) % 6].MyEdges.Add(edge);
                }

                if (!edge.MyVertices.Contains(cell.MyVertices[i]))
                {
                    edge.MyVertices.Add(cell.MyVertices[i]);
                }
                if (!edge.MyVertices.Contains(cell.MyVertices[(i + 1) % 6]))
                {
                    edge.MyVertices.Add(cell.MyVertices[(i + 1) % 6]);
                }


                edge.transform.SetParent(cell.transform.Find("My Edges"));
                edge.transform.localPosition = (HexMetrics.corners[i] + HexMetrics.corners[(i + 1) % 6]) / 2;
                edge.transform.localRotation = EdgeMetrics.rotations[i];
                if (cell.GetNeighbor(i) != null)
                {
                    cell.GetNeighbor(i).MyEdges[(int) (((HexDirection) i).Opposite()) % 6] = edge;
                }
            }
        }
        AssignEdgeNeighbors();
    }

    private void AssignEdgeNeighbors()
    {
        foreach (var cell in Cells)
        {
            if (cell == null) continue;

            for (int i = 0; i < 6; i++)
            {
                var edge = cell.MyEdges[i];
                if (!edge.Neighbors.Contains(cell.MyEdges[(i + 1) % 6]))
                {
                    edge.Neighbors.Add(cell.MyEdges[(i + 1) % 6]);
                    cell.MyEdges[(i + 1) % 6].Neighbors.Add(edge);
                }
                if (!edge.Neighbors.Contains(cell.MyEdges[(i + 5) % 6]))
                {
                    edge.Neighbors.Add(cell.MyEdges[(i + 5) % 6]);
                    cell.MyEdges[(i + 5) % 6].Neighbors.Add(edge);
                }
                var directionOpposite = ((HexDirection) i).Opposite();
                int directionOppositeInt = (int) directionOpposite;

                if (cell.GetNeighbor(i) != null) continue;
                if (cell.GetNeighbor((i + 1) % 6) == null) continue;
                if (edge.Neighbors.Contains(cell.GetNeighbor((i + 1) % 6).MyEdges[(directionOppositeInt + 2) % 6])) continue;
                edge.Neighbors.Add(cell.GetNeighbor((i + 1) % 6).MyEdges[(directionOppositeInt + 2) % 6]);
                cell.GetNeighbor((i + 1) % 6).MyEdges[(directionOppositeInt + 2) % 6].Neighbors.Add(edge);
            }
        }
    }

    public void BuildEdgeUnit_Sandbox(Vector3 position, HexDirection direction, EdgeUnit type)
    {
        position = transform.InverseTransformPoint(position);
        var coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * Width + coordinates.Z / 2;

        if (index > Cells.Length - 1) return;

        var cell = Cells[index];

        int directionInt = (int)direction;

        if (!cell.MyEdges[directionInt].Type.Equals(EdgeUnit.Open)) return;

        cell.MyEdges[directionInt].Type = type;
        cell.MyEdges[directionInt].gameObject.transform.localRotation = EdgeMetrics.rotations[directionInt];
        if (type == EdgeUnit.Ship)
        {
            cell.MyEdges[directionInt].gameObject.transform.localRotation *= Quaternion.Euler(0f, 90f, 0f);
        }

        switch (_phase)
        {
            case SandboxPhase.Phase1:
                _phase = SandboxPhase.Phase2;
                Build("City");
                
                break;

            //delete later
            case SandboxPhase.Phase2:
                _phase = SandboxPhase.Phase3;
                Build("Off");
                
                break;
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
