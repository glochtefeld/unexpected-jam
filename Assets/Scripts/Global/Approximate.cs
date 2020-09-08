using UnityEngine;

public class Approximate : MonoBehaviour
{
    public static bool Vector(Vector2 a, Vector2 b)
    {
        if (Mathf.Abs(a.x) - Mathf.Abs(b.x) < 0.1f 
            && Mathf.Abs(a.y) - Mathf.Abs(b.y) < 0.1f)
            return true;
        return false;
    }
}
// TODO: Remove.
/* Unused. Returns true if two values are approximately equal.*/
