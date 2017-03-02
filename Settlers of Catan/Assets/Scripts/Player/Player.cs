using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string PlayerName = "";

    public int PlayerId;

    // private bool isTurn;                 We are still debating if it is needed
    [SerializeField] public TurnPhase MyTurnPhase { get; set; }
    public List<HexEdge> MyEdges = new List<HexEdge>();

    public List<HexVertex> MyVertices = new List<HexVertex>();

    public CardInventory CardInventory;
    public List<Harbour> MyHarbour = new List<Harbour>();
    public int Ratio;
    public bool hasAlchemist;                             // TODO : find it in inventory

    public void Initialize(int i, CardInventory cardInventoryPrefab)
    {
        //name = "Player" + i;
        PlayerId = i;
        PlayerName = "Player " + PlayerName;
        //isTurn = false;
        // MyTurnPhase = TurnPhase.WaitForTurn;
        CardInventory = Instantiate(cardInventoryPrefab);
        CardInventory.gameObject.SetActive(true);
        CardInventory.transform.parent = this.transform;
    }

    public CardInventory getCardInventory()
    {
        return CardInventory;
    }

    //The player needs to find a way to determine which harbours he has access to
    public List<Harbour> GetHarbours()
    {
        return MyHarbour;
    }

    public int getMaritimTradeRatio(SteableKind resource)
    {
        Ratio = 4;
        for (var i = 0; i < MyHarbour.Count; i++)
        {
            if (MyHarbour[i].GetType() == typeof(GenericHarbour))
            {
                Ratio = 3;
            }
            if (MyHarbour[i].GetType() == typeof(SpecialHarbour) && ((SpecialHarbour)MyHarbour[i]).steableKind == resource)
            {
                return 2;
            }
        }
        return Ratio;
    }

    public bool Equals(Player otherPlayer)
    {
        return PlayerId == otherPlayer.GetPlayerId();
    }

    public int GetPlayerId()
    {
        return PlayerId;
    }

    public bool HasAlchemist()
    {
        return hasAlchemist;                        // TODO : find alchemist in inventory
    }

    /*    public bool CheckIsTurn() {
            return isTurn;
        }

        public void SetIsTurn(bool b) {
            isTurn = b; 
        } */


    void Awake()
    {
        PlayerManager.getInstance().AddtoList(this);
    }

    /*private void Awake()
    {
        PlayerManager.getInstance().AddtoList(this);
        Debug.Log(playerName);
    }*/
    public void SetIsTurn(bool b)
    {
        // isTurn = b;
    }
}
