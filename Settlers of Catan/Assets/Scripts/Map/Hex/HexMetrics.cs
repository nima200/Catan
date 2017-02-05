using UnityEngine;

public static class HexMetrics
{
    // http://catlikecoding.com/unity/tutorials/hex-map-1/about-hexagons/hexagon.png
    // Hexes have inner and outer radius. The inner is derived from the outer.
    // Using pythagorean theorem, you can do the triangles needed to see that
    // if outer radius is x, inner radius becomes x * (sqrt(3)/2).
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;

    // generic locations of corners according to the inner and outer radius set.
	public static Vector3[] corners = {
        // The order of these vertices determines whether we have a pointy edge of
        // the hexes on top when generated or a flat edge. 

        // Looking at the image linked above should help visualize what these positions mean.
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
    };
}