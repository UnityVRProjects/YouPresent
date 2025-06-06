using UnityEngine;

public class VRAvatarRig : MonoBehaviour
{
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    public Transform headBone;
    public Transform leftHandBone;
    public Transform rightHandBone;

    void LateUpdate()
    {
        if (headTarget != null)
            headBone.position = headTarget.position;

        if (leftHandTarget != null)
            leftHandBone.position = leftHandTarget.position;

        if (rightHandTarget != null)
            rightHandBone.position = rightHandTarget.position;
    }
}
