using TMPro;
using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    [SerializeField] Transform leftHand, rightHand;
    [SerializeField] TextMeshPro leftRotDisplay, rightRotDisplay;

    TimeManager timeManager;

    const float ROTATION_BOUND_MIN = 40.0f;
    const float ROTATION_BOUND_MAX = 80.0f;

    void Start()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 l_RotationAngles = leftHand.eulerAngles;
        Vector3 r_RotationAngles = rightHand.eulerAngles;
        leftRotDisplay.text = l_RotationAngles.ToString();
        rightRotDisplay.text = r_RotationAngles.ToString();

        if(isHandDown(l_RotationAngles) && isHandDown(r_RotationAngles))
        {
            timeManager.SetAreHandsDown(true);
        }
        else
        {
            timeManager.SetAreHandsDown(false);
        }
    }

    bool isHandDown(Vector3 rotVec)
    {
        return rotVec.x >= ROTATION_BOUND_MIN && rotVec.x <= ROTATION_BOUND_MAX;
    }
}
