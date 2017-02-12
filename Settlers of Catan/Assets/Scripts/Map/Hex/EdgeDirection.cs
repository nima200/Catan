public enum EdgeDirection
{
    NE, SE, SW, NW
}

public static class EdgeDirectionExtensions
{
    public static EdgeDirection Opposite(this EdgeDirection direction)
    {
        return (int)direction < 2 ? (direction + 2) : (direction - 2);
    }
}