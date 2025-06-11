using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlideCounter : MonoBehaviour
{
    public TextMeshProUGUI slideCounter;
    public Button increment;
    public Button decrement;
    [SerializeField] SlideshowManager slideshowManager;
    [SerializeField] static int slide;
    public float count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = slideshowManager.getMyTimer(slide);
       increment.onClick.AddListener(IncrementCount);
       decrement.onClick.AddListener(DecrementCount);
    }

    public void IncrementCount()
    {
        count += 0.1f;
        slideshowManager.setMyTimer(slide, count);
        UpdateCount();
    }

    public void DecrementCount()
    {
        count -= 0.1f;
        slideshowManager.setMyTimer(slide, count);
        UpdateCount();
    }

    public void UpdateCount() {
        slideCounter.text = count.ToString() + "min";
    }
}
