// #if UNITY_EDITOR
// using UnityEngine;
// using UnityEngine.InputSystem;
// using UnityEditor.Recorder;
// using UnityEditor.Recorder.Input;
// using System.Linq;

// public class RuntimeRecorder : MonoBehaviour
// {
//     public RecorderControllerSettings recorderSettings;
//     public InputActionProperty startStopAction;

//     private RecorderController controller;
//     private bool isRecording = false;

//     void Start()
//     {
//         if (recorderSettings == null)
//         {
//             Debug.LogError("❌ RecorderControllerSettings not assigned.");
//             return;
//         }

//         controller = new RecorderController(recorderSettings);
//         startStopAction.action.Enable();

//         var movieRecorder = recorderSettings.RecorderSettings.FirstOrDefault() as MovieRecorderSettings;
//         if (movieRecorder is MovieRecorderSettings && movieRecorder.ImageInputSettings is CameraInputSettings camInput)
//         {
//             Debug.Log("🎥 Recorder is set to use tagged camera: " + camInput.CameraTag);
//         }
//         else
//         {
//             Debug.LogError("❌ Could not find a valid MovieRecorder with CameraInputSettings.");
//         }
//     }

//     void Update()
//     {
//         if (startStopAction.action.WasPressedThisFrame())
//         {
//             if (!isRecording)
//             {
//                 controller.PrepareRecording();
//                 controller.StartRecording();
//                 isRecording = true;
//                 Debug.Log("▶️ Recording started.");
//             }
//             else
//             {
//                 controller.StopRecording();
//                 isRecording = false;
//                 Debug.Log("⏹️ Recording stopped.");
//             }
//         }
//     }
// }
// #endif

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System;
using System.IO;

public class VRRecorderControllerNoAsset : MonoBehaviour
{
    public InputActionProperty startStopAction;
    public Camera recordingCamera;  // Assign your TestRecordingCamera in Inspector

    private RecorderController recorderController;
    private bool isRecording = false;

    void Start()
    {
        if (recordingCamera == null)
        {
            Debug.LogError("❌ Recording Camera is not assigned.");
            return;
        }

        // Create controller settings in code
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        recorderController = new RecorderController(controllerSettings);

        // Create movie recorder settings
        var movieRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        movieRecorder.name = "RuntimeMovieRecorder";
        movieRecorder.Enabled = true;
        movieRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;

        // Optional: Adjust resolution and quality
        movieRecorder.AudioInputSettings.PreserveAudio = true;

        // Use tagged camera directly
        var cameraInput = new CameraInputSettings
        {
            Source = ImageSource.TaggedCamera,
            CameraTag = "TestRecorder",
            OutputWidth = 1920,
            OutputHeight = 1080
        };
        movieRecorder.ImageInputSettings = cameraInput;

        // Setup dynamic output path
        var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var outputPath = Path.Combine("Recordings", $"recording_{date}");
        movieRecorder.OutputFile = outputPath;

        // Finalize recorder
        controllerSettings.AddRecorderSettings(movieRecorder);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 50.0f;

        // Input
        startStopAction.action.Enable();
    }

    void Update()
    {
        if (startStopAction.action.WasPressedThisFrame())
        {
            if (!isRecording)
            {
                recorderController.PrepareRecording();
                recorderController.StartRecording();
                isRecording = true;
                Debug.Log("▶️ Recording started.");
            }
            else
            {
                recorderController.StopRecording();
                isRecording = false;
                Debug.Log("⏹️ Recording stopped.");
            }
        }
    }
}
#endif
