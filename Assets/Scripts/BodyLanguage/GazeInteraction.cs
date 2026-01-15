using UnityEngine;

public class GazeInteraction : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    TimeManager timeManager;
    float gazeReach = 50.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * gazeReach, Color.yellow);
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, gazeReach, layerMask))
        {
            timeManager.SetIsLookingAtAudience((hit.collider.tag == "Audience"));
            timeManager.SetIsLookingAtProjector((hit.collider.tag == "Projector"));
        }
        else
        {
            timeManager.SetIsLookingAtAudience(false);
            timeManager.SetIsLookingAtProjector(false);
        }
    }
}
