using UnityEngine;

public class LewisManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    [SerializeField] Transform stage;
    [SerializeField] Transform audience;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stage()
    {
        this.transform.position = stage.position;
        this.transform.rotation = stage.rotation;
        animator.SetBool("Yell", false);
        animator.SetBool("Clap", false);
        animator.SetBool("Standing", true);
    }

    public void Audience()
    {
        this.transform.position = audience.position;
        this.transform.rotation = audience.rotation;
        animator.SetBool("Standing", false);
    }

    public void StartTalk()
    {
        animator.SetBool("Talk", true);
    }
    public void StopTalk()
    {
        animator.SetBool("Talk", false);
    }
}
