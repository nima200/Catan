using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class DiceRoll : MonoBehaviour {

    //TODO: Player curPlayer reference (-> bool isTurn)
    private enum EventDie {BarbMove, Market,Politics, Science};

    public Button rollBtn;                // WARNING: public attribute does not look wellformed...
    public Button endTurnBtn;
    public Button resetBtn;
    public GameObject choicePanelObj;
    public ChoicePanel choicePanel;

    private int[] intRolls;
    private EventDie eventRoll;
    private bool isRolled;
    public bool hasMerchant;

    /* ========================
     *          START
     * ========================
     * called at the beginning of the game
     */
    void Start() {
        rollBtn.onClick.AddListener(RollTrigger);     // COMMENT: instead of dice roll class with reference to DiceButton, just put everything in dice button ?
        endTurnBtn.onClick.AddListener(ResetDice);
        resetBtn.onClick.AddListener(ResetDice);
        choicePanel.submit.onClick.AddListener(RollMerchant);

        isRolled = false;
        //hasMerchant = true;                          // TO REMOVE: when player hasMerchant attribute is implemented
        intRolls = new int[2];
    }

    /* ========================
     *        INIT DICE
     * ========================
     * 
     * called when it is this player's turn (eventlistener ?)
     * TODO: link to player's activation
     */
    public void initDice() {
        rollBtn.interactable = true;
    }

    /* ========================
   *    ROLL TRIGGER(onclick)
   *   ========================
   *    TODO: link this method with ResourceSolver?
   */
    public void RollTrigger () {
        if (!isRolled){
            if (hasMerchant){
                //TODO : offer to choose red and yellow dice outcome
                choicePanelObj.SetActive(true); // action will happen in RollMerchant
                //RollEventDie();
            }

            else{
                RollIntDice();
                RollEventDie();
            }
            isRolled = true;
            rollBtn.interactable = false;
        }
    }

    /* ===================
     *     ROLL Merchant
     * ===================
     * Die 1 = red
     * Die 2 = yellow       
     */
    void RollMerchant() {
        intRolls[0] = choicePanel.GetRedDieValue();
        Debug.Log("MERCHANT : Die " + 0 + " :" + intRolls[0] + "\n");
        intRolls[1] = choicePanel.GetYellowDieValue();
        Debug.Log("MERCHANT : Die " + 1 + " :" + intRolls[1] + "\n");
        RollEventDie();
        choicePanelObj.SetActive(false);
    }


    /* ===================
     *     ROLL INT DICE
     * ===================
     * Die 1 = red
     * Die 2 = yellow       
     */
    private void RollIntDice() {
        for (int i = 0; i < 2; i++) {
            intRolls[i] = Random.Range(1, 7);
            Debug.Log("Die " + i + " :" + intRolls[i] + "\n");
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
            eventRoll = EventDie.BarbMove;
            Debug.Log("Event Die: " + eventRoll.ToString() + "\n");
        }
        else if (roll == 4) {
            eventRoll = EventDie.Market;
            Debug.Log("Event Die: " + eventRoll.ToString() + "\n");
        }
        else if (roll == 5) {
            eventRoll = EventDie.Politics;
            Debug.Log("Event Die: " + eventRoll.ToString() + "\n");
        }
        else if (roll == 6) {
            eventRoll = EventDie.Science;
            Debug.Log("Event Die: " + eventRoll.ToString() + "\n");
        }
    }

    /*==========================
     *   RESET(endTurnButton)
     *==========================
     */
    public void ResetDice() {
        isRolled = false;
        rollBtn.interactable = true; // TO REMOVE: when player ready/unready behaviour is implemented
        // COMMENT: don't have to reset values, since next roll will update them
    }
}
