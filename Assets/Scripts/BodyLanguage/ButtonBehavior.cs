using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{

    [SerializeField] Color test;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { 
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        GetComponent<Renderer>().material.color = test;
    }
}
