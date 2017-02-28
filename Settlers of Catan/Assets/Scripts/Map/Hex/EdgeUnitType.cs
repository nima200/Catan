using UnityEngine;


/* ENUM for representing the state of an edge. */
public enum EdgeUnit
{
    Disabled,
    Open,
    Road,
    Ship,
    Hidden
};
/* Script that is attached to every possible prefab of an edge */
public class EdgeUnitType : MonoBehaviour
{
    public EdgeUnit Unit;
}
