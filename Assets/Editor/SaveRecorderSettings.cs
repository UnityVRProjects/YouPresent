#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

public static class GenerateCameraRecorderAsset
{
[MenuItem("Tools/Generate Camera Recorder Asset")]
    public static void Generate()
    {
        // Create main controller settings
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();

        // Create movie recorder settings
        var movieRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        movieRecorder.name = "TestRecordingCameraRecorder";
        movieRecorder.Enabled = true;

        // Output format
        movieRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        movieRecorder.AudioInputSettings.PreserveAudio = true;

        // Input: capture from a tagged camera named "TestRecorder"
        var cameraInput = new CameraInputSettings
        {
            Source = ImageSource.TaggedCamera,
            CameraTag = "TestRecorder",
            OutputWidth = 1920,
            OutputHeight = 1080
        };
        movieRecorder.ImageInputSettings = cameraInput;

        // Output file path settings (matches image)
        movieRecorder.OutputFile = "Recordings/<Recorder>_<Take>_<Date>";

        // Finalize settings
        controllerSettings.AddRecorderSettings(movieRecorder);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 50.0f;

        // Save asset
        AssetDatabase.CreateAsset(controllerSettings, "Assets/RecorderSettings/TaggedCameraRecording.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("✅ Saved: Assets/RecorderSettings/TaggedCameraRecording.asset");
    }
}
#endif