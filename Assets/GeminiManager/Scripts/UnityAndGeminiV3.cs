using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using TMPro;
using System.IO; 
using System;
using System.Linq;
using Unity.VisualScripting;

[System.Serializable]
public class UnityAndGeminiKey
{
    public string key;
}



[System.Serializable]
public class InlineData
{
    public string mimeType;
    public string data;
}

// Text-only part
[System.Serializable]
public class TextPart
{
    public string text;
}

[System.Serializable]
public class ImagePart
{
    public string text;
    public InlineData inlineData;
}

[System.Serializable]
public class TextContent
{
    public string role;
    public TextPart[] parts;
}

[System.Serializable]
public class TextCandidate
{
    public TextContent content;
}

[System.Serializable]
public class TextResponse
{
    public TextCandidate[] candidates;
}

[System.Serializable]
public class ImageContent
{
    public string role;
    public ImagePart[] parts;
}

[System.Serializable]
public class ImageCandidate
{
    public ImageContent content;
}

[System.Serializable]
public class ImageResponse
{
    public ImageCandidate[] candidates;
}



[System.Serializable]
public class ChatRequest
{
    public TextContent[] contents;
    public TextContent system_instruction;
}


public class UnityAndGeminiV3: MonoBehaviour
{
    [Header("JSON API Configuration")]
    public TextAsset jsonApi;

    
    private string apiKey = ""; 
    private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent"; // Edit it and choose your prefer model
    private string imageEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp-image-generation:generateContent"; //End point for image generation

    [Header("ChatBot Function")]
    public TMP_InputField inputField;
    public TMP_Text uiText;
    public string botInstructions;
    private TextContent[] chatHistory;
    public string reply;
    public Transcription TranscriptionManager;
    public SlideshowManager SlidesManager;
    public TimeManager TimeManager;


    [Header("Prompt Function")]
    public string prompt = "";

    [Header("Image Prompt Function")]
    public string imagePrompt = "";
    public Material skyboxMaterial; 

    [Header("Media Prompt Function")]
    
    public string mediaFilePath = "";
    public string mediaPrompt = "";
    public enum MediaType
    {
        Video_MP4 = 0,
        Audio_MP3 = 1,
        PDF = 2,
        JPG = 3,
        PNG = 4
    }
    public MediaType mimeType = MediaType.Video_MP4;
    

    public string GetMimeTypeString()
    {
        switch (mimeType)
        {
            case MediaType.Video_MP4:
                return "video/mp4";
            case MediaType.Audio_MP3:
                return "audio/mp3";
            case MediaType.PDF:
                return "application/pdf";
            case MediaType.JPG:
                return "image/jpeg";
            case MediaType.PNG:
                return "image/png";
            default:
                return "error";
        }
    }


    void Start()
    {

        Debug.Log("Gemini Scene Functionality is inactive due to migration of command to be run by other classes, to retrieve previous functionality please copy contents of runGemini() into Start() of this .cs file");

    }

    //The below method is what I want you guys to test 

    public void runGemini()
    {

        UnityAndGeminiKey jsonApiKey = JsonUtility.FromJson<UnityAndGeminiKey>(jsonApi.text);
        apiKey = jsonApiKey.key;
        TranscriptionManager = GetComponent<Transcription>();
        chatHistory = new TextContent[] { };

        // comment just the line below this comment to remove the performance data eval section if its buggy and just wanting to run Gemini feedback mode from user to ask questions and use runGemini() after presentation is over
        StartCoroutine(SendPerformanceDataToGemini());

        if (prompt != "") { StartCoroutine(SendPromptRequestToGemini(prompt)); }
        ;
        if (imagePrompt != "") { StartCoroutine(SendPromptRequestToGeminiImageGenerator(imagePrompt)); }
        ;
        if (mediaPrompt != "" && mediaFilePath != "") { StartCoroutine(SendPromptMediaRequestToGemini(mediaPrompt, mediaFilePath)); }
        ;

    }

 

    private IEnumerator SendPromptRequestToGemini(string promptText)
    {
        string url = $"{apiEndpoint}?key={apiKey}";
     
        string jsonData = "{\"contents\": [{\"parts\": [{\"text\": \"{" + promptText + "}\"}]}]}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Create a UnityWebRequest with the JSON data
        using (UnityWebRequest www = new UnityWebRequest(url, "POST")){
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError(www.error);
            } else {
                Debug.Log("Request complete!");
                TextResponse response = JsonUtility.FromJson<TextResponse>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                    {
                      
                        string text = response.candidates[0].content.parts[0].text;
                        Debug.Log(text);
                    }
                else
                {
                    Debug.Log("No text found.");

         url = $"{apiEndpoint}?key={apiKey}";                }
            }
        }
    }

    public string TimeManagerDataConcatenate()
    {

        string result = "";

        Dictionary<string, string> data = TimeManager.CollectedData();
        foreach(KeyValuePair<string, string> entry in data)
        {

            result += entry.Key + ":" + entry.Value;

        }

        return result;

    }

    private IEnumerator SendPerformanceDataToGemini()
    {

        float[] slideTimes = SlidesManager.getSlideTime();
        string promptText = "";

        promptText += "Using the inputted data regarding the user's performance while public speaking: please evaluate their performance. Do not include any symbols, when you read an s, that means seconds. ";


        promptText += "For Presentation, here are multiple time values regarding user's eye contact: " + TimeManagerDataConcatenate() + " elapsedTime is time spent total during the presentation, audienceTime is time spent total looking at the audience, projectorTime is time spent looking at the projector, lookingAtNothing_Num is the amount of times the user looks at nothing for more than 6 seconds, and disengagedHands_Num is the number of times the user did nothing with their hands during presentation for 10+ seconds straight";
            
           promptText += "Please proceed and go over each of the criteria separately and explain what they did well, and what could be improved, if anything. " +
            "Finally, at the end of it ask them if they have any questions about your evaluation or if they'd like any advice. Please make your response short to about 3 sentences per section.";

        string url = $"{apiEndpoint}?key={apiKey}";


        string jsonData = "{\"contents\": [{\"parts\": [{\"text\": \"{" + promptText + "}\"}]}]}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Create a UnityWebRequest with the JSON data
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Request complete!");
                TextResponse response = JsonUtility.FromJson<TextResponse>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    //This is the response to your request
                    string text = response.candidates[0].content.parts[0].text;
                    if (text == "")
                    {
                        Debug.Log("error");
                    }
                    else
                    {
                        StartCoroutine(TranscriptionManager.SynthesizeSpeech(text));
                    }
                        Debug.Log(text);
                }
                else
                {
                    Debug.Log("No text found.");

                    url = $"{apiEndpoint}?key={apiKey}";
                }


            }
        }
    }

    public void SendChat()
    {
        string userMessage = inputField.text;
        StartCoroutine( SendChatRequestToGemini(userMessage));
    }

    public void SendUserMessage(string message)
    {
        StartCoroutine(SendChatRequestToGemini(message));
    }

    private IEnumerator SendChatRequestToGemini(string newMessage)
    {

        string url = $"{apiEndpoint}?key={apiKey}";
     
        TextContent userContent = new TextContent
        {
            role = "user",
            parts = new TextPart[]
            {
                new TextPart { text = newMessage }
            }
        };

        TextContent instruction = new TextContent
        {
            parts = new TextPart[]
            {
                new TextPart {text = botInstructions}
            }
        }; 

        List<TextContent> contentsList = new List<TextContent>(chatHistory);
        contentsList.Add(userContent);
        chatHistory = contentsList.ToArray(); 

        ChatRequest chatRequest = new ChatRequest { contents = chatHistory, system_instruction = instruction };

        string jsonData = JsonUtility.ToJson(chatRequest);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Create a UnityWebRequest with the JSON data
        using (UnityWebRequest www = new UnityWebRequest(url, "POST")){
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError(www.error);
            } else {
                Debug.Log("Request complete!");
                TextResponse response = JsonUtility.FromJson<TextResponse>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                    {
                       
                         reply = response.candidates[0].content.parts[0].text;
                    if (TranscriptionManager != null)
                    {
                        StartCoroutine(TranscriptionManager.SynthesizeSpeech(reply));
                    }

                    TextContent botContent = new TextContent
                        {
                            role = "model",
                            parts = new TextPart[]
                            {
                                new TextPart { text = reply }
                            }
                        };

                        Debug.Log(reply);
                        //This part shows the text in the Canvas
                        uiText.text = reply;
                        //This part adds the response to the chat history, for your next message
                        contentsList.Add(botContent);
                        chatHistory = contentsList.ToArray();
                    }
                else
                {
                    Debug.Log("No text found.");
                }
             }
        }  
    }


    private IEnumerator SendPromptRequestToGeminiImageGenerator(string promptText)
    {
        string url = $"{imageEndpoint}?key={apiKey}";
        
    
        string jsonData = $@"{{
            ""contents"": [{{
                ""parts"": [{{
                    ""text"": ""{promptText}""
                }}]
            }}],
            ""generationConfig"": {{
                ""responseModalities"": [""Text"", ""Image""]
            }}
        }}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

      
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.LogError(www.error);
            } 
            else 
            {
                Debug.Log("Request complete!");
                Debug.Log("Full response: " + www.downloadHandler.text); 
                
               
                try 
                {
                    ImageResponse response = JsonUtility.FromJson<ImageResponse>(www.downloadHandler.text);
                    
                    if (response.candidates != null && response.candidates.Length > 0 && 
                        response.candidates[0].content != null && 
                        response.candidates[0].content.parts != null)
                    {
                        foreach (var part in response.candidates[0].content.parts)
                        {
                            if (!string.IsNullOrEmpty(part.text))
                            {
                                Debug.Log("Text response: " + part.text);
                            }
                            else if (part.inlineData != null && !string.IsNullOrEmpty(part.inlineData.data))
                            {
                          
                                byte[] imageBytes = System.Convert.FromBase64String(part.inlineData.data);
                                
                              
                                Texture2D tex = new Texture2D(2, 2);
                                tex.LoadImage(imageBytes);
                                byte[] pngBytes = tex.EncodeToPNG();
                                string path = Application.persistentDataPath + "/gemini-image.png";
                                File.WriteAllBytes(path, pngBytes);
                                Debug.Log("Saved to: " + path);
                                Debug.Log("Image received successfully!");

                              
                                string imagePath = Path.Combine(Application.persistentDataPath, "gemini-image.png");
                                
                                Texture2D panoramaTex = new Texture2D(2, 2);
                                panoramaTex.LoadImage(File.ReadAllBytes(imagePath));

                                Texture2D properlySizedTex = ResizeTexture(panoramaTex, 1024, 512);
                                
                              
                                if (skyboxMaterial != null)
                                {
                                 
                                    skyboxMaterial.shader = Shader.Find("Skybox/Panoramic");
                                    skyboxMaterial.SetTexture("_MainTex", properlySizedTex);
                                    DynamicGI.UpdateEnvironment();
                                    Debug.Log("Skybox updated with panoramic image!");
                                }
                                else
                                {
                                    Debug.LogError("Skybox material not assigned!");
                                }

                                // Another approach but might cause distorsion

                                
                                // Texture2D savedTex = new Texture2D(2, 2);
                                // savedTex.LoadImage(File.ReadAllBytes(path));

                                // // Convert to Cubemap (simplified approach - may distort)
                                // Cubemap newCubemap = new Cubemap(savedTex.width, TextureFormat.RGBA32, false);
                                // for (int i = 0; i < 6; i++)
                                // {
                                //     newCubemap.SetPixels(savedTex.GetPixels(), (CubemapFace)i);
                                // }
                                // newCubemap.Apply();

                                // // Apply to skybox
                                // if (skyboxMaterial != null)
                                // {
                                //     skyboxMaterial.SetTexture("_Tex", newCubemap);
                                //     DynamicGI.UpdateEnvironment();
                                //     Debug.Log("Skybox updated with new image!");
                                // }                            

                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No valid response parts found.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON Parse Error: " + e.Message);
                }
            }
        }
    }


    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(newWidth, newHeight);
        RenderTexture.active = rt;
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }

    private IEnumerator SendPromptMediaRequestToGemini(string promptText, string mediaPath)
    {
       
        byte[] mediaBytes = File.ReadAllBytes(mediaPath);
        string base64Media = System.Convert.ToBase64String(mediaBytes);

        string url = $"{apiEndpoint}?key={apiKey}";

        string mimeTypeMedia = GetMimeTypeString();



        string jsonBody = $@"
        {{
        ""contents"": [
            {{
            ""parts"": [
                {{
                ""text"": ""{promptText}""
                }},
                {{
                ""inline_data"": {{
                    ""mime_type"": ""{mimeTypeMedia}"",
                    ""data"": ""{base64Media}""
                }}
                }}
            ]
            }}
        ]
        }}";


   
        Debug.Log("Sending JSON: " + jsonBody); // For debugging


        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);


        // Create and send the request
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.LogError(www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
            } 
            else 
            {
                Debug.Log("Request complete!");
                TextResponse response = JsonUtility.FromJson<TextResponse>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    string text = response.candidates[0].content.parts[0].text;
                    Debug.Log(text);
                }
                else
                {
                    Debug.Log("No text found.");
                }
            }
        }
    }

}



