using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{

    private static TurnManager _instance = null;
    
    public BoardManager Board;
    private Player _localPlayer;                            // --> the player running this instance of the game
    private Player _currentPlayer;                         // --> the player currently PLAYING
    public UiManager Ui = UiManager.GetInstance();


    // Use this for initialization
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Debug.Log("TurnManager created");

    }

    private void Update()
    {
        // Mouse Button 0 = Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        // Always trying to change state on a mouse click but not doing anything if not player's turn
        var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(inputRay, out hit)) return;
        // Do not handle input if local player and current player aren't the sme
        if (!_currentPlayer.isLocalPlayer) return;
        // Check on player's current phase
        switch (_currentPlayer.MyTurnPhase)
        {
            case TurnPhase.Sandbox1:
            case TurnPhase.Sandbox2:
            case TurnPhase.Build:
                switch (Board.BuildMode)
                {
                    case BuildMode.Off:
                        break;
                    case BuildMode.Settlement:
                        Board.BuildCornerUnit(hit.point, (HexDirection)Ui.DirectionDropdown.value, CornerUnit.Settlement, _currentPlayer);
                        break;
                    case BuildMode.City:
                        Board.BuildCornerUnit(hit.point, (HexDirection)Ui.DirectionDropdown.value, CornerUnit.City, _currentPlayer);
                        break;
                    case BuildMode.Road:
                        Board.BuildEdgeUnit(hit.point, (HexDirection)Ui.DirectionDropdown.value, EdgeUnit.Road, _currentPlayer);
                        break;
                    case BuildMode.Ship:
                        Board.BuildEdgeUnit(hit.point, (HexDirection)Ui.DirectionDropdown.value, EdgeUnit.Ship, _currentPlayer);
                        break;
                    case BuildMode.Knight:
                        break;
                }
                break;
            case TurnPhase.WaitForTurn:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    public void SetFirstPlayer()
    {
        int firstplayerindex = PlayerManager.getInstance().SetRandomFirst(); // TODO : make it syntax correct
        _localPlayer = PlayerManager.getInstance().getLocalPlayer();
        _currentPlayer = PlayerManager.getInstance().getPlayer(firstplayerindex);
        _currentPlayer.MyTurnPhase = TurnPhase.Sandbox1;
        UpdateUi(_currentPlayer);
        for (int i = 0; i < PlayerManager.getInstance().getNbOfPlayer(); i++)
        { // Initialize UIs of all players
            if (PlayerManager.getInstance().getPlayer(i) != _currentPlayer)
            {
                PlayerManager.getInstance().getPlayer(i).MyTurnPhase = TurnPhase.WaitForTurn;
                UpdateUi(PlayerManager.getInstance().getPlayer(i));
            }
        }
    }

    public static TurnManager getInstance()
    {
        return _instance;
    }

    public void NextTurn()
    {
        DiceRoll.getInstance().ResetDice();
        Player previousCurrentPlayer = _currentPlayer;                                   // TODO : assigning players? 

        Debug.Log("Previous  player : " + previousCurrentPlayer.GetPlayerId());
        SetCurrentToNextPlayer();
        Debug.Log("New current player : " + _currentPlayer.GetPlayerId());


        previousCurrentPlayer.NextPhase();              //update phases of the 2 players
        UpdateUi(previousCurrentPlayer);
        _currentPlayer.NextPhase();
        UpdateUi(_currentPlayer);

    }


    private void UpdateUi(Player p)
    {
        Ui = UiManager.GetInstance();
        if (!p.isLocalPlayer) return;
        Debug.Log("big dick bandito in da club");   
        switch (p.MyTurnPhase)
        {
            case TurnPhase.Sandbox1:
                Ui.MainMenu.SetActive(false);
                Ui.BuildMenu.SetActive(true);
                foreach (var selectable in Ui.BuildMenu.GetComponentsInChildren<Selectable>())
                {
                    selectable.interactable = false;
                }
                foreach (var child in Ui.DiceMenu.GetComponentsInChildren<Selectable>())
                {
                    child.interactable = false;
                }
                Ui.DirectionDropdown.interactable = true;
                Board.Build("Settlement", p);
                foreach (var selectable in Ui.DiceMenu.GetComponentsInChildren<Selectable>())
                {
                    selectable.interactable = false;
                }
                break;
            case TurnPhase.Sandbox2:
                Ui.MainMenu.SetActive(false);
                Board.Build("City", p);
                foreach (var selectable in Ui.DiceMenu.GetComponentsInChildren<Selectable>())
                {
                    selectable.interactable = false;
                }
                break;
            case TurnPhase.Build:
                Ui.MainMenu.SetActive(false);
                Ui.BuildMenu.SetActive(true);
                foreach (var child in Ui.DiceMenu.GetComponentsInChildren<Selectable>())
                {
                    child.interactable = false;
                }
                break;
            case TurnPhase.WaitForTurn:
                Ui.MainMenu.SetActive(true);
                Ui.BuildMenu.SetActive(false);
                foreach (var selectable in Ui.MainMenu.GetComponentsInChildren<Button>())
                {
                    Debug.Log("hello");
                    selectable.interactable = false;
                }
                foreach (var child in Ui.DiceMenu.GetComponentsInChildren<Selectable>())
                {
                    child.interactable = false;
                }
                break;
        }
    }

    private void SetInteractable(bool b, List<Selectable> buttonList)
    {
        //TODO
    }

    public Player getMainPlayer()
    {
        return _localPlayer;
    }

    public Player getCurrentPlayer()
    {
        return _currentPlayer;
    }

    public void SetCurrentToNextPlayer()
    {
        _currentPlayer = PlayerManager.getInstance().getNextPlayer();
    }
    public void SetCurrentToPreviousPlayer()
    {
        _currentPlayer = PlayerManager.getInstance().getPreviousPlayer();
    }
}
/*Selectable[] allUtilitySelectable = MainMenu.GetComponentsInChildren<Selectable>();
foreach (Selectable s in allUtilitySelectable)
{
    s.interactable = true;
}
Button[] allDiceButtons = DiceMenu.GetComponentsInChildren<Button>();
foreach (Button b in allDiceButtons)
{
    b.interactable = true;
}
Debug.Log("ACTIVATED: I am the new current player");*/
