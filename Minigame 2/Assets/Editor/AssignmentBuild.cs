using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class AssignmentBuild
{
    public static void PrepareScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        var original = GameObject.Find("Prop");
        if (!original) throw new Exception("Original Prop missing");
        AddChest(original, "Prop - Layer 2", new Vector3(2.5f, 8f, -.43f), "Layer 2");
        AddChest(original, "Prop - Layer 3", new Vector3(-4.5f, 12f, .43f), "Layer 3");
        var spell = UnityEngine.Object.FindFirstObjectByType<Spell>(FindObjectsInactive.Include);
        var serialized = new SerializedObject(spell);
        serialized.FindProperty("_life").floatValue = 1f;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        spell.gameObject.SetActive(false);
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("SCENE_PREPARED: three chests, three layers, lifetime=1");
        EditorApplication.Exit(0);
    }

    static void AddChest(GameObject original, string name, Vector3 position, string layer)
    {
        var chest = GameObject.Find(name) ?? UnityEngine.Object.Instantiate(original);
        chest.name = name;
        chest.transform.position = position;
        foreach (var child in chest.GetComponentsInChildren<Transform>(true))
            if (child.gameObject.layer != LayerMask.NameToLayer("UI"))
                child.gameObject.layer = LayerMask.NameToLayer(layer);
        foreach (var renderer in chest.GetComponentsInChildren<SpriteRenderer>(true))
            renderer.sortingLayerName = layer;
    }

    public static void WebBuild()
    {
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.decompressionFallback = false;
        PlayerSettings.companyName = "Minigame";
        PlayerSettings.productName = "Minigame 2";
        var output = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../WebGL"));
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = new[] { "Assets/Scenes/Main.unity" },
            locationPathName = output,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        });
        Debug.Log("WEB_BUILD_RESULT: " + report.summary.result);
        EditorApplication.Exit(report.summary.result == BuildResult.Succeeded ? 0 : 1);
    }
}
