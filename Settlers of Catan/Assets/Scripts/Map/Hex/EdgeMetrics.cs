using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EdgeMetrics {
    public static Quaternion[] rotations = {
        Quaternion.Euler(0f, 120f, 0f), // NE
        Quaternion.Euler(0f, 0f, 0f), // E
        Quaternion.Euler(0f, 60f, 0f), // SE
        Quaternion.Euler(0f, 120f, 0f), //SW
        Quaternion.Euler(0f, 0f, 0f), // W
        Quaternion.Euler(0f, 60f, 0f) // NW
    };
}
