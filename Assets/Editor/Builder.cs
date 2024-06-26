using UnityEngine;
using UnityEditor;

public class Builder
{
    private static void Build()
    {

        var scenes = new string[]
        {
            "Assets/MainMenuScene.unity",
            "Assets/Scenes/Cityscape.unity",
        };

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "cli-build/ShiTaku.exe",
            target = BuildTarget.StandaloneWindows64,
        };

        BuildPipeline.BuildPlayer(options);
    }
}
