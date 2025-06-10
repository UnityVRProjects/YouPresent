using UnityEngine;

public class HeadTargetFollowXZ : MonoBehaviour
{
    public Transform headset;      // Typically Main Camera
    public float fixedY = 1.5f;    // Adjust to match standing avatar height

    void LateUpdate()
    {
        Vector3 newPos = headset.position;
        newPos.y = fixedY;  // Override Y
        transform.position = newPos;
        transform.rotation = headset.rotation; // optional: sync rotation
    }
}