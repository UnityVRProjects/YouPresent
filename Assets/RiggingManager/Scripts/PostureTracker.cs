using UnityEngine;

public class PostureTracker : MonoBehaviour
{
    public Transform head;
    public Transform hips;

    private float timer = 0f;
    public float interval = 2f; // log every 2 seconds

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;

            float slouchAngle = Vector3.Angle((head.position - hips.position).normalized, Vector3.up);
            Debug.Log("Slouch angle: " + slouchAngle);
        }
    }
}
