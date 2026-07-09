using UnityEditor;

public static class BuildScriptCompat
{
    [MenuItem("Build/Windows")]
    public static void BuildWindows()
    {
        BuildScript.BuildWindows();
    }
}