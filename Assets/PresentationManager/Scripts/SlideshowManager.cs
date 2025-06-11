using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class SlideshowManager : MonoBehaviour
{
    [SerializeField] Canvas slideshow;
    [SerializeField] TimeManager timeManager;
    [SerializeField] LewisManager lewisManager;
    [SerializeField] AudienceManager audienceManager;
    [SerializeField] UnityAndGeminiV3 avatar;
    [SerializeField] SpeechToText speechToText;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject PresentingMenu;
    Image[] slides;
    float[] slideTimers;
    Image currSlide;
    int currIndex = 0;
    bool isPresenting = false;
    bool isEnded = false;
    bool audienceStop = false;
    float applauseTime = 0f;


    public enum mode
    {
        menu,
        training,
        free,
        practice
    }

    bool[] settings = { false, false, false, false };
    // [record, eye contact, posture, slide pacing]
    mode currMode = mode.menu;
    public mode selectedMode = mode.menu;
    public static float[] myTimers = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

    void Start()
    {
        slides = GetComponentsInChildren<Image>();

        foreach (Image image in slides)
        {
            image.enabled = false;
        }

        slideTimers = new float[slides.Length];
        for (int x = 0; x < slides.Length; x++)
        {
            slideTimers[x] = 0f;
        }
        lewisManager.Stage();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPresenting)
        {
            slideTimers[currIndex] += Time.deltaTime;
        }

        if (isEnded)
        {
            applauseTime += Time.deltaTime;
        }

        if (applauseTime > 8f)
        {
            audienceManager.EndApplause();
            lewisManager.EndApplause();
            lewisManager.Stage();

            MainMenu.SetActive(true);
            PresentingMenu.SetActive(false);
            isEnded = false;
            applauseTime = 0f;
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
        timeManager.StartTime();
        lewisManager.Audience();
    }

    public void NextSlide()
    {
        if (isPresenting)
        {
            if (currIndex < slides.Length - 1)
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

        audienceManager.StartApplause();
        lewisManager.Applause();

        for (int x = 0; x < slides.Length; x++)
        {
            float time = slideTimers[x];
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            Debug.Log("Slide " + x + " time is: " + minutes + "m and " + seconds + " seconds.");
        }
        timeManager.StopTime();
        isEnded = true;
    }

    public float[] getSlideTime()
    {
        return slideTimers;
    }

    public void ToggleTrainingSettings(int setting)
    {
        settings[setting] = !settings[setting];
    }

    public void setMode(string modeSelected)
    {
        if (modeSelected == "training")
        {
            selectedMode = mode.training;
        }

        else
        {
            selectedMode = mode.free;
        }
    }

    public float getMyTimer(int index)
    {
        return myTimers[index];
    }

    public void setMyTimer(int index, float time)
    {
        myTimers[index] = time;
        Debug.Log("Timer " + index + " updated to time " + time);
        return;
    }

    public void TalkAvatar()
    {
        lewisManager.StartTalk();
        avatar.runGemini();
    }

    public void ExitAvatar()
    {
        lewisManager.StopTalk();
    }

}
