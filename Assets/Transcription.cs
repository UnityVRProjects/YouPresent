using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;

public class Transcription : MonoBehaviour
{
    private string apiKey = "AIzaSyBPi7ETtMkduzgd5_1hbdg-_5YBS0QI5io";  // Replace with your actual API key

    void Start()
    {
        StartCoroutine(SynthesizeSpeech("Hello from Google TTS!"));
    }

    IEnumerator SynthesizeSpeech(string text)
    {
        string url = $"https://texttospeech.googleapis.com/v1/text:synthesize?key={apiKey}";

        TTSRequest requestBody = new TTSRequest
        {
            input = new TTSInput { text = text },
            voice = new TTSVoice { languageCode = "en-US", ssmlGender = "FEMALE" },
            audioConfig = new TTSConfig { audioEncoding = "LINEAR16" }
        };

        string json = JsonUtility.ToJson(requestBody);
        Debug.Log("Request JSON: " + json);

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("TTS request failed: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("TTS response: " + jsonResponse);

            string base64Audio = JsonUtility.FromJson<AudioResponse>(jsonResponse).audioContent;
            byte[] audioBytes = System.Convert.FromBase64String(base64Audio);

            string filePath = Path.Combine(Application.persistentDataPath, "tts_output.wav");
            File.WriteAllBytes(filePath, audioBytes);
            Debug.Log("Saved TTS to: " + filePath);

            StartCoroutine(PlayAudio(filePath));
        }
    }

    IEnumerator PlayAudio(string path)
    {
        using (var www = new WWW("file://" + path))
        {
            yield return www;
            AudioClip clip = www.GetAudioClip(false, true);
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log("Audio is now playing!");
        }
    }

    [System.Serializable]
    public class TTSRequest
    {
        public TTSInput input;
        public TTSVoice voice;
        public TTSConfig audioConfig;
    }

    [System.Serializable]
    public class TTSInput
    {
        public string text;
    }

    [System.Serializable]
    public class TTSVoice
    {
        public string languageCode;
        public string ssmlGender;
    }

    [System.Serializable]
    public class TTSConfig
    {
        public string audioEncoding;
    }

    [System.Serializable]
    public class AudioResponse
    {
        public string audioContent;
    }
}
