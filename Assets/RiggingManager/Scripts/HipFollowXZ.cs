using UnityEngine;

public class HipFollowXZ : MonoBehaviour
{
    public Transform mainCam;

    public float hipOffsetY = -1.0f; // How far below the head the hips should be

    public bool rotateWithHead = true;

    void LateUpdate()
    {
        Vector3 pos = mainCam.position;
        pos.y = hipOffsetY;
        transform.position = pos;

        if (rotateWithHead)
        {
            Vector3 forward = mainCam.forward;
            forward.y = 0f;
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }
}