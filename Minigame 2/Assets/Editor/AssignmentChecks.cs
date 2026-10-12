using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public static class AssignmentChecks
{
    public static void InspectMap()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        foreach (var map in UnityEngine.Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
        {
            if (!map.name.Contains("Grass")) continue;
            var cells = map.cellBounds.allPositionsWithin;
            string result = "CELLS " + map.name + ": ";
            foreach (var cell in cells)
                if (map.HasTile(cell)) result += cell.x + "," + cell.y + ";";
            Debug.Log(result);
        }
        EditorApplication.Exit(0);
    }
    const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
    static int failures;
    static void Check(bool condition, string message)
    {
        Debug.Log((condition ? "PASS: " : "FAIL: ") + message);
        if (!condition) failures++;
    }
    static void Call(object target, string method, params object[] args) =>
        target.GetType().GetMethod(method, Private).Invoke(target, args);

    public static void Run()
    {
        failures = 0;
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        var chest = UnityEngine.Object.FindFirstObjectByType<ColorChange>();
        var chests = UnityEngine.Object.FindObjectsByType<ColorChange>(FindObjectsSortMode.None);
        Check(chests.Length >= 3, "At least three complete chests");
        Check(chests.Select(c => c.gameObject.layer).Distinct().Count() >= 2, "At least two collision layers");
        foreach (var prop in chests)
        {
            Check(prop.GetComponent<SpriteRenderer>().sortingLayerName == LayerMask.LayerToName(prop.gameObject.layer),
                prop.name + " sorting and collision layers match");
            var references = new SerializedObject(prop);
            var assignedText = references.FindProperty("_healthText").objectReferenceValue as TMP_Text;
            Check(assignedText && assignedText.transform.IsChildOf(prop.transform), prop.name + " owns its health text");
        }
        var text = chest.GetComponentInChildren<TMP_Text>(true);
        var renderer = chest.GetComponent<SpriteRenderer>();
        Call(chest, "Start");
        Check(!text.gameObject.activeSelf, "Health text starts hidden");
        var spell = UnityEngine.Object.FindFirstObjectByType<Spell>(FindObjectsInactive.Include);
        for (int health = 3; health >= 0; health--)
        {
            Call(chest, "OnTriggerEnter2D", spell.GetComponent<Collider2D>());
            if (health == 0) Check(!chest.gameObject.activeSelf, "Chest disappears at zero health");
            else
            {
                Check(text.gameObject.activeSelf && text.text == "h = " + health, "Health text " + health);
                float red = health == 3 ? 1f : health == 2 ? .5f : 0f;
                Check(renderer.color == new Color(red, .2f, .2f), "Color stage " + health);
            }
        }
        spell.gameObject.SetActive(true);
        Call(spell, "OnEnable");
        Check((float)typeof(Spell).GetField("_timeLeft", Private).GetValue(spell) == 1f,
            "Each activation resets lifetime to one second");
        typeof(Spell).GetField("_timeLeft", Private).SetValue(spell, 0f);
        Call(spell, "Update");
        Check(!spell.gameObject.activeSelf && !spell.GetComponent<Collider2D>().enabled,
            "Expired spell and collider disable");
        foreach (var map in UnityEngine.Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
            Debug.Log("MAP " + map.name + " " + map.cellBounds + " position=" + map.transform.position);
        Debug.Log("ASSIGNMENT_CHECKS failures=" + failures);
        EditorApplication.Exit(failures == 0 ? 0 : 1);
    }
}
