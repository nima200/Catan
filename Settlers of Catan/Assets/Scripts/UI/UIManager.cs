using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiManager : MonoBehaviour
{

    private static UiManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public static UiManager GetInstance()
    {
        return _instance;
    }
    // BUILD MENU
    public GameObject BuildMenu;
    public Selectable BuildRoadButton;
    public Selectable BuildShipButton;
    public Selectable BuildSettleButton;
    public Selectable BuildCityButton;
    public Dropdown DirectionDropdown;



}
