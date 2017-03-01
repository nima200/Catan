using UnityEngine;

/* ENUM for representing the state of a corner. */
public enum CornerUnit
{
    Disabled,
    Open,
    Settlement,
    City,
    Hidden
}

/* Script that is attached to every possible prefab of an edge */
public class CornerUnitType : MonoBehaviour
{
    public CornerUnit Type;
}
