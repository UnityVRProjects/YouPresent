using UnityEngine;
using UnityEngine.UI;

public class SlideshowManager : MonoBehaviour
{
    [SerializeField] Canvas slideshow;
    Image[] slides;
    Image currSlide;
    int currIndex = 0;
    bool isPresenting = false;
    void Start()
    {
        slides = GetComponentsInChildren<Image>();

        foreach (Image image in slides)
        {
            image.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            NextSlide();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartPresentation();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PrevSlide();
        }
    }

    public void StartPresentation()
    {
        isPresenting = true;
        currIndex = 0;
        currSlide = slides[currIndex];
        currSlide.enabled = true;
    }

    public void NextSlide()
    {
        if (isPresenting)
        {
            if(currIndex < slides.Length - 1)
            {
                currSlide.enabled = false;
                currIndex++;
                currSlide = slides[currIndex];
                currSlide.enabled = true;
            }
            else
            {
                EndPresentation();
            }
        }
    }

    public void PrevSlide()
    {
        if (isPresenting && currIndex != 0)
        {
            currSlide.enabled = false;
            currIndex--;
            currSlide = slides[currIndex];
            currSlide.enabled = true;
        }
    }

    public void EndPresentation()
    {
        currSlide.enabled = false;
        isPresenting = false;
        currIndex = 0;
        currSlide = null;
    }
}
