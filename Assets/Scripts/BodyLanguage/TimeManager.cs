using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TextMeshPro elapsedText, audienceText, projectorText, warningText, handsDownText;

    bool isLookingAtAudience = false;
    bool isLookingAtProjector = false;
    bool isLookingAtNothing = false;
    bool areHandsDown = false;
    bool areHandsDisengaged = false;

    const float LOOKING_AT_NOTHING_LIMIT = 6.0f;
    const float HANDS_DOWN_LIMIT = 10.0f;

    float elapsedTime = 0;
    float audienceTime = 0;
    float projectorTime = 0;
    float handsDownTime = 0;

    float timeLookingAtNothing, timeDisengagedHands;

    int lookingAtNothing_num, disengagedHands_num = 0;

    private void Start()
    {
        timeLookingAtNothing = LOOKING_AT_NOTHING_LIMIT;
        timeDisengagedHands = HANDS_DOWN_LIMIT;
        warningText.enabled = false;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        elapsedText.text = "Elapsed: " + FormattedTime(elapsedTime) + " (" + lookingAtNothing_num.ToString() + ")";

        if (isLookingAtAudience)
        {
            audienceTime += Time.deltaTime;
            audienceText.text = "Audience: " + FormattedTime(audienceTime);
        }

        if (isLookingAtProjector)
        {
            projectorTime += Time.deltaTime;
            projectorText.text = "Projector: " + FormattedTime(projectorTime);
        }

        if (areHandsDown) {
            handsDownTime += Time.deltaTime;
            timeDisengagedHands -= Time.deltaTime;
            handsDownText.text = "Hands down: " + FormattedTime(handsDownTime) + " (" + disengagedHands_num.ToString() + ")"; 
        }

        if (!isLookingAtAudience && !isLookingAtProjector)
            timeLookingAtNothing -= Time.deltaTime;
        else
            timeLookingAtNothing = LOOKING_AT_NOTHING_LIMIT;

        if (timeDisengagedHands <= 0)
        {
            if (!areHandsDisengaged)
            {
                disengagedHands_num++;
                areHandsDisengaged = true;
            }
        }
        else
        {
            areHandsDisengaged = false;
        }

        if (timeLookingAtNothing <= 0) {
            warningText.enabled = true;
            if (!isLookingAtNothing) {
                lookingAtNothing_num++;
                isLookingAtNothing = true;
            }
        }
        else
        {
            warningText.enabled = false;
            isLookingAtNothing = false;
        }

    }

    string FormattedTime(float timeVal)
    {
        int minutes = Mathf.FloorToInt(timeVal / 60);
        int decSeconds = Mathf.FloorToInt(timeVal / 10) % 6;
        int seconds = Mathf.FloorToInt(timeVal % 10);

        return minutes.ToString() + ":" + decSeconds.ToString() + seconds.ToString();
    }

    public void SetIsLookingAtAudience(bool val) { 
        isLookingAtAudience=val;
    }

    public void SetIsLookingAtProjector(bool val) { 
        isLookingAtProjector=val; 
    }

    public void SetAreHandsDown(bool val)
    {
        areHandsDown=val;
        if (!val)
        {
            timeDisengagedHands = HANDS_DOWN_LIMIT;
        }
    }

    public Dictionary<string, string> CollectedData() {

        Dictionary<string, string> data = new Dictionary<string, string> {
            { "gaze_elapsedTime" , elapsedTime.ToString() },
            { "gaze_audienceTime" , audienceTime.ToString() },
            { "gaze_projectorTime" , projectorTime.ToString() },
            { "gaze_lookingAtNothing_num", lookingAtNothing_num.ToString() },
            { "hands_handsDownTime", handsDownTime.ToString() },
            { "hands_disengagedHands_num", disengagedHands_num.ToString() }
        };
        
        return data;
    }
}
