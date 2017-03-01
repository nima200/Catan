using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using System.Linq;

public class PlayerManager : MonoBehaviour
{

	private static PlayerManager instance = null;
    public GameManager _gameManager;
    public GameObject Players;
	public Player playerPrefrab;
	public CardInventory cardInventoryPrefab;
	public List<Player> myPlayers = new List<Player> ();
    private int nbOfPlayers;
    private int pointer;


    public LobbyManager _lobbyManager;
    private List<LobbyPlayer> intermiedateList;
    private IEnumerable<LobbyPlayer> IEnumberableLobbyplayerList;

    public LobbyPlayerList _lobbyplayerlist;

    //Make Player Manager Singleton
    void Awake ()
	{
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (gameObject);    
		}
		DontDestroyOnLoad (gameObject);

        // nbOfPlayers = 4;                    // Set to 4 for now
        //createPlayer ();

        _lobbyManager = FindObjectOfType<LobbyManager>();
        _lobbyplayerlist = FindObjectOfType<LobbyPlayerList>();
        intermiedateList = _lobbyplayerlist.PlayerList;
        IEnumberableLobbyplayerList = intermiedateList;

        int totalPlayers = IEnumberableLobbyplayerList.Count();
        Debug.Log("There were " + IEnumberableLobbyplayerList.Count() + " Players in the lobby");
        PlayerManager.getInstance().SetNumberOfPlayers(totalPlayers);

        pointer = 0;
        Debug.Log("Player Manager created");                   
	}

	public static PlayerManager getInstance()
	{
		return instance;
	}
    
    // would go away. No need of making 4 players
    // no need for vector transform . 
	private void createPlayer ()
	{
        Player player;
        int offset = 100;
		for (int i = 0; i < nbOfPlayers; i++) {
			player = Instantiate (playerPrefrab, Players.transform);
            player.transform.position = new Vector3(200+(offset*i),200,0);
			player.Initialize(i,cardInventoryPrefab);
            bool e = player.enabled;
			myPlayers.Add(player);
		}
	}

    public Player getLocalPlayer() {
        foreach (Player p in myPlayers) {
            if (p.isLocalPlayer) {
                return p;
            }
        }
        return null;
    }

    public Player getPlayer (int index)
	{
        Debug.Log("Player at index is : "+ myPlayers[index]);
		return myPlayers[index];
	}

    public int getNbOfPlayer() {
        return nbOfPlayers;
    }

    public Player getNextPlayer()
    {
        pointer = (pointer + 1) % nbOfPlayers;
        return getPlayer(pointer);
    }

    public void AddtoList(Player _player)
    {
        myPlayers.Add(_player);
        int i = myPlayers.IndexOf(_player);
        _player.Initialize(i, cardInventoryPrefab);
        Debug.Log("Player index:" + i + " and List length:" + myPlayers.Count);

        if (myPlayers.Count == nbOfPlayers) {
            Instantiate(_gameManager);
            _gameManager.gameObject.SetActive(true);
        }
    }

    public void SetNumberOfPlayers(int i) {
        nbOfPlayers = i;
    }
}
