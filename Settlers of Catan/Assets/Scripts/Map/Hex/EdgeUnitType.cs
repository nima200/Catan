using UnityEngine;

public enum EdgeUnit
{
    Disabled,
    Open,
    Road,
    Ship,
    Hidden
};

public class EdgeUnitType : MonoBehaviour
{
    public EdgeUnit Unit;
}
