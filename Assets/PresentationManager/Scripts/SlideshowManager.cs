using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class SlideshowManager : MonoBehaviour
{
    [SerializeField] Canvas slideshow;
    Image[] slides;
    float[] slideTimers;
    Image currSlide;
    int currIndex = 0;
    bool isPresenting = false;

    public enum mode
    {
        menu,
        training,
        free,
        practice
    }
    mode currMode = mode.menu;
    mode selectedMode;

    void Start()
    {
        slides = GetComponentsInChildren<Image>();

        foreach (Image image in slides)
        {
            image.enabled = false;
        }

        slideTimers = new float[slides.Length];
        for(int x = 0; x < slides.Length; x++)
        {
            slideTimers[x] = 0f;
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

        if (isPresenting)
        {
            slideTimers[currIndex] += Time.deltaTime;
        }

        // Select modes
        if (Input.GetKeyDown(KeyCode.I))
        {
            selectedMode = mode.training;
            Debug.Log("TRAINING mode selected");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            selectedMode = mode.free;
            Debug.Log("FREE mode selected");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            selectedMode = mode.practice;
            Debug.Log("PRACTICE mode selected");
        }

        if (Input.GetKeyDown(KeyCode.M) && !isPresenting)
        {
            currMode = selectedMode;
            Debug.Log(currMode + " was confirmed.");
        }



    }

    public void StartPresentation()
    {
        isPresenting = true;
        currIndex = 0;
        currSlide = slides[currIndex];
        currSlide.enabled = true;
        for (int x = 0; x < slides.Length; x++)
        {
            slideTimers[x] = 0f;
        }

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

        for (int x = 0; x < slides.Length; x++)
        {
            float time = slideTimers[x];
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            Debug.Log("Slide " + x + " time is: " + minutes + "m and " + seconds + " seconds.");
        }
    }

    public float[] getSlideTime()
    {
        return slideTimers;
    }
}
