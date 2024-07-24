using UnityEngine;
using UnityEditor;

public class Builder
{
    private static void Build()
    {

        var scenes = new string[]
        {
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/Cityscape.unity",
            "Assets/Scenes/Instructional Scene.unity",
            "Assets/Scenes/Pass Scene.unity",
            "Assets/Scenes/Fail Scene.unity",
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
