using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour {

    //TODO: Player curPlayer reference (-> bool isTurn)

    private static DiceRoll instance = null;
    public Button RollBtn;                // WARNING: public attribute does not look wellformed...
    public AlchemistMenu AlchemistMenu;

    private int[] _intRolls;
    private EventDie _eventRoll;
    private bool _isRolled;
    public bool HasAlchemist;

    void Awake() {
        if (instance == null)
        {
            instance = this;
            _isRolled = false;
            _intRolls = new int[2];
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /* ========================
     *        INIT DICE
     * ========================
     * 
     * called when it is this player's turn (eventlistener ?)
     * TODO: link to player's activation
     */
    public void InitDice() {
        RollBtn.interactable = true;
    }

    /* ========================
   *    ROLL TRIGGER(onclick)
   *   ========================
   *    TODO: link this method with ResourceSolver?
   */
    public void RollTrigger () {
        if (!_isRolled){
            if (HasAlchemist){
                AlchemistMenu.gameObject.SetActive(true); // action will happen in RollAlchemist
            }

            else{
                RollIntDice();
                RollEventDie();
                _isRolled = true;
            }
            RollBtn.interactable = false;
        }
    }

    /* ===================
     *     ROLL Alchemist
     * ===================
     * Die 1 = red
     * Die 2 = yellow       
     */
    public void RollAlchemist() {
        _intRolls[0] = AlchemistMenu.GetRedDieValue();
        Debug.Log("ALCHEMIST : Red die " + 0 + " :" + _intRolls[0] + "\n");
        _intRolls[1] = AlchemistMenu.GetYellowDieValue();
        Debug.Log("ALCHEMIST : Yellow die " + 1 + " :" + _intRolls[1] + "\n");
        AlchemistMenu.gameObject.SetActive(false);
        RollEventDie();
    }


    /* ===================
     *     ROLL INT DICE
     * ===================
     * Die 1 = red
     * Die 2 = yellow       
     */
    private void RollIntDice() {
        for (int i = 0; i < 2; i++) {
            _intRolls[i] = Random.Range(1, 7);
            Debug.Log("Die " + i + " :" + _intRolls[i] + "\n");
        }
    }
    /* ==================
    *    ROLL EVENT DIE
    * ===================
    *  -> 1,2,3 : barabarians move
    *  -> 4 : market 
    *  -> 5 : politics
    *  -> 6 : science
    */
    private void RollEventDie() {
        int roll = Random.Range(1, 7);
        if (roll == 1 || roll == 2 || roll == 3) {
            _eventRoll = EventDie.BARBMOVE;
            Debug.Log("Event Die: " + _eventRoll.ToString() + "\n");
        }
        else if (roll == 4) {
            _eventRoll = EventDie.TRADE;
            Debug.Log("Event Die: " + _eventRoll.ToString() + "\n");
        }
        else if (roll == 5) {
            _eventRoll = EventDie.POLITICS;
            Debug.Log("Event Die: " + _eventRoll.ToString() + "\n");
        }
        else if (roll == 6) {
            _eventRoll = EventDie.SCIENCE;
            Debug.Log("Event Die: " + _eventRoll.ToString() + "\n");
        }
    }

    public int[] getIntRoll() {
        if (_isRolled)
        {
            return _intRolls;
        }
        else return null;
    }


    public EventDie getEventRoll()
    {
        return _eventRoll;
    }


    /*==========================
     *   RESET(endTurnButton)
     *==========================
     */
    public void ResetDice() {
        _isRolled = false;
        RollBtn.interactable = true; // TO REMOVE: when player ready/unready behaviour is implemented
        // COMMENT: don't have to reset values, since next roll will update them
    }


    public static DiceRoll getInstance()
    {
        return instance;
    }
}
