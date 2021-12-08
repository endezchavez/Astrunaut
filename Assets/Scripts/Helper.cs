using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static bool IsOffScreen(Transform pos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos.position);

        return screenPos.x < 0;
    }
}
