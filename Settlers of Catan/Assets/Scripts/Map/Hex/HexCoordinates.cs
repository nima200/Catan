using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    // THESE ARE AXIAL COORDINATES!

    // Serializing to survive through recompiles in play mode.
    [SerializeField]
    private int x, z;

    // Public readonly methods
    // http://answers.unity3d.com/questions/295972/how-to-create-read-only-variables.html

    // Making the coordinates immutable.
    public int X
    {
        get
        {
            return x;
        }
    }

    // The coordinate components add up to zero for every coordinate.
    // So Y can be derived from X and Z.
    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }
    }

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // Given an x and y point, this creates a HexCoordinate and 
    // assigns the values to it through the constructor.
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - (z / 2) , z);
    }

    // This method creates a HexCoordinate given a position.
    // If you look at where it's called (HexGrid.cs line 168) you shall see that
    // it's used where mouse input is involved. We store the location where mouse 
    // was clicked in a Vector3 data type and derive the coordinates that it falls onto from there.
    // it's like a mapping from the cartesian coordinates (X,Y) of the screen onto the axial coordinate
    // system we have implemented for the hex grid.

    // The math is pretty annoying and complicated. You don't need to really get it.
    // I don't either. Just know that given a Vector3, it replies back with the HexCoordinate mapping.
    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }
        

        return new HexCoordinates(iX, iZ);
    }

    // My custom ToString() overrides for editor purposes. Not too important.
    public override string ToString()
    {
        return "(" +
            X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}
