public enum HexDirection {
	NE, E, SE, SW, W, NW
}

public static class HexDirectionExtensions {

    // Returns the opposite direction of the direction it was given.
    // Since enums have int values underlying, an enum can be casted to an int.
    // So if it's < 3 we return direction + 3, and direction - 3 if not.

    // ? Operator
    // https://msdn.microsoft.com/en-us/library/ty67wk28.aspx

    // Extension methods
    // https://www.codeproject.com/Tips/709310/Extension-Method-In-Csharp

    public static HexDirection Opposite (this HexDirection direction) {
		return (int)direction < 3 ? (direction + 3) : (direction - 3);
	}
}