using UnityEngine;
using Google.Cloud.TextToSpeech.V1;
using System;
using System.IO;

public class Speaking : MonoBehaviour
{

    void Start()
    {
        // Setup authentication
        string credentialPath = Path.Combine(Application.streamingAssetsPath, "sentimental-sonar-55e824d61f3b.json");
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

        // Create the client
        var client = TextToSpeechClient.Create();

        // Input text to synthesize
        var input = new SynthesisInput
        {
            Text = "Hello! This is Google Cloud Text to Speech working in Unity!"
        };

        // Select the voice and language
        var voice = new VoiceSelectionParams
        {
            LanguageCode = "en-US",
            SsmlGender = SsmlVoiceGender.Female
        };

        // Audio output configuration
        var config = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Linear16  // WAV format
        };

        // Synthesize speech
        var response = client.SynthesizeSpeech(input, voice, config);

        // Save audio to a file
        string outputPath = Path.Combine(Application.persistentDataPath, "tts_output.wav");
        File.WriteAllBytes(outputPath, response.AudioContent.ToByteArray());
        Debug.Log("TTS audio saved to: " + outputPath);

        // Play the audio
        StartCoroutine(PlayAudio(outputPath));
        Debug.Log("We are playing audio now!");
    }

    // Play audio from file
    System.Collections.IEnumerator PlayAudio(string path)
    {
        using (var www = new WWW("file://" + path))
        {
            yield return www;
            AudioClip clip = www.GetAudioClip(false, true);
            var audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    
}
