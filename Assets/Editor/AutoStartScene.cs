#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class EditorStartSceneSetter
{
    // Đường dẫn đến scene bạn muốn chạy đầu tiên
    private const string StartScenePath = "Assets/Scenes/Bootstrapper.unity";

    static EditorStartSceneSetter()
    {
        // Tìm scene asset từ đường dẫn
        SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(StartScenePath);

        if (startScene != null)
        {
            // Gán scene này làm scene khởi động khi nhấn Play
            EditorSceneManager.playModeStartScene = startScene;
            Debug.Log($"<color=cyan><b>Play Mode Start Scene</b> đã được đặt thành <b>{startScene.name}</b>.</color>");
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy scene tại đường dẫn: '{StartScenePath}'. Không thể đặt scene khởi động.");
        }
    }
}
#endif