using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        MenuButton button;
        if((button = other.GetComponent<MenuButton>()) != null)
        {
            button.Click();
        }
    }
}
