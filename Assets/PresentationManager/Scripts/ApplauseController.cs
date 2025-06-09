using UnityEngine;

public class ApplauseController : MonoBehaviour
{

    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Applause()
    {
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            animator.SetBool("Clap", true);
        }
        else 
        {
            animator.SetBool("Yell", true);
        }
    }

    public void StopApplause()
    {
        animator.SetBool("Yell", false);
        animator.SetBool("Clap", false);
    }
}
