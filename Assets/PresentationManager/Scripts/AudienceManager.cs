using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    ApplauseController[] applauseControllers;
    void Start()
    {
        applauseControllers = GetComponentsInChildren<ApplauseController>();

    }

    public void StartApplause()
    {

        foreach (ApplauseController controller in applauseControllers)
        {
            controller.Applause();
        }
    }

    public void EndApplause()
    {
        foreach (ApplauseController controller in applauseControllers)
        {
            controller.StopApplause();
        }
    }
}
