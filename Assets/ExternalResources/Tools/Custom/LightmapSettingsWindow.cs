using UnityEditor;

public class LightmapSettingsWindow : EditorWindow
{
  private static readonly int[] kSizeValues = { 512, 1024, 2048, 4096 };
  private static readonly string[] kSizeStrings = { "512", "1024", "2048", "4096" };

  void OnGUI()
  {
    LightmapEditorSettings.maxAtlasWidth  = EditorGUILayout.IntPopup("Max Atlas Width",  LightmapEditorSettings.maxAtlasWidth,  kSizeStrings, kSizeValues);
    LightmapEditorSettings.maxAtlasHeight = EditorGUILayout.IntPopup("Max Atlas Height", LightmapEditorSettings.maxAtlasHeight, kSizeStrings, kSizeValues);

    LightmapEditorSettings.resolution = EditorGUILayout.FloatField("Resolution" ,LightmapEditorSettings.resolution);
  }

  [MenuItem("Custom Tools/Lightmap Settings")]
  static void ShowWindow()
  {
    GetWindow<LightmapSettingsWindow>("LM Settings");
  }
}