using UnityEngine;

public class AudienceManager : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplauseController[] applauseControllers = GetComponentsInChildren<ApplauseController>();

            foreach (ApplauseController controller in applauseControllers)
            {
                controller.Applause();
            }
        }
    }
}
