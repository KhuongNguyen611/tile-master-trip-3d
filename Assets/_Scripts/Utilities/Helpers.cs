using UnityEngine;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t)
            Object.Destroy(child.gameObject);
    }

    public static float CheckAngle(float angle)
    {
        if (angle > 0)
        {
            if (angle > -90)
            {
                angle = 0;
            }
            else
            {
                angle = -180;
            }
        }
        else
        {
            if (angle < 90)
            {
                angle = 0;
            }
            else
            {
                angle = 180;
            }
        }
        return angle;
    }

    public static Vector3 CheckRotation(Vector3 rotation)
    {
        float x = CheckAngle(rotation.x);
        float y = CheckAngle(rotation.y);
        float z = CheckAngle(rotation.z);

        return new Vector3(x, y, z);
    }
}
