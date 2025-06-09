using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class RecognitionConfig
{
    public string encoding;
    public int sampleRateHertz;
    public string languageCode;
}

[Serializable]
public class RecognitionAudio
{
    public string content;
}

[Serializable]
public class RecognizeRequest
{
    public RecognitionConfig config;
    public RecognitionAudio audio;
}

[Serializable]
public class GoogleAlt { public string transcript; }
[Serializable]
public class GoogleResult { public GoogleAlt[] alternatives; }
[Serializable]
public class GoogleResponse { public GoogleResult[] results; }

public class SpeechToText : MonoBehaviour
{
    [Header("Google STT key")]
    [Tooltip("Set your API key here in the Inspector")]
    public string apiKey;

    [Tooltip("Hz")]
    public int sampleRate = 16000;
    [Tooltip("Seconds")]
    public float recordSeconds = 5f;

    [Header("Gemini script")]
    public UnityAndGeminiV3 geminiManager;

    public void StartTranscription() => StartCoroutine(RecordAndTranscribe());

    private IEnumerator RecordAndTranscribe()
    {
        // 1) Record
        AudioClip clip = Microphone.Start(null, false,
                            Mathf.CeilToInt(recordSeconds),
                            sampleRate);
        yield return new WaitForSeconds(recordSeconds);
        Microphone.End(null);

        // 2) Encode to WAV PCM16 + base64
        byte[] wav = ConvertClipToWav(clip);
        string b64 = Convert.ToBase64String(wav);

        // 3) Build URL & JSON using our typed RecognizeRequest
        string url = $"https://speech.googleapis.com/v1/speech:recognize?key={apiKey}";
        var req = new RecognizeRequest
        {
            config = new RecognitionConfig
            {
                encoding = "LINEAR16",
                sampleRateHertz = sampleRate,
                languageCode = "en-US"
            },
            audio = new RecognitionAudio { content = b64 }
        };
        string json = JsonUtility.ToJson(req);

        // 4) Send it
        using var www = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        //  ⇒ if it still errors, we’ll see Google’s message
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"STT Error: {www.error}\nBody: {www.downloadHandler.text}");
            yield break;
        }

        // 5) Parse response
        var resp = JsonUtility.FromJson<GoogleResponse>(www.downloadHandler.text);
        if (resp.results?.Length > 0 && resp.results[0].alternatives?.Length > 0)
        {
            string transcript = resp.results[0].alternatives[0].transcript;
            Debug.Log("🗣️ Heard: " + transcript);
            geminiManager.SendUserMessage(transcript);
        }
        else
        {
            Debug.LogWarning("No speech recognized.");
        }
    }

    private byte[] ConvertClipToWav(AudioClip clip)
    {
        // pull samples
        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // float → 16-bit PCM
        var intData = new Int16[samples.Length];
        var bytesData = new byte[samples.Length * 2];
        float rescale = 32767f;
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescale);
            BitConverter.GetBytes(intData[i]).CopyTo(bytesData, i * 2);
        }

        // write RIFF/WAV header correctly
        using var ms = new System.IO.MemoryStream();
        using var bw = new System.IO.BinaryWriter(ms);

        int hz = clip.frequency;
        int ch = clip.channels;
        int br = hz * ch * 2;

        bw.Write("RIFF".ToCharArray());
        bw.Write(36 + bytesData.Length);
        bw.Write("WAVE".ToCharArray());

        bw.Write("fmt ".ToCharArray());
        bw.Write(16);                // fmt sub-chunk size
        bw.Write((short)1);          // PCM
        bw.Write((short)ch);
        bw.Write(hz);
        bw.Write(br);
        bw.Write((short)(ch * 2));
        bw.Write((short)16);         // bits per sample

        bw.Write("data".ToCharArray());
        bw.Write(bytesData.Length);
        bw.Write(bytesData);

        bw.Flush();
        return ms.ToArray();
    }
}
