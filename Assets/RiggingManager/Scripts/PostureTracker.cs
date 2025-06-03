using UnityEngine;

public class PostureTracker : MonoBehaviour
{
    public Transform head;
    public Transform hips;

    void Update()
    {
        float slouchAngle = Vector3.Angle((head.position - hips.position).normalized, Vector3.up);
        Debug.Log("Slouch angle: " + slouchAngle);
    }
}