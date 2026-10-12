using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SpellPlayCheck
{
    static Spell spell;
    static float started;
    static int activations;
    static double deadline;
    static SpellPlayCheck()
    {
        if (SessionState.GetBool("MG2.PlayCheck", false))
        {
            deadline = EditorApplication.timeSinceStartup + 60;
            EditorApplication.update += Tick;
        }
    }
    public static void Run()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
        SessionState.SetBool("MG2.PlayCheck", true);
        EditorApplication.EnterPlaymode();
    }
    static void Tick()
    {
        if (EditorApplication.timeSinceStartup > deadline) Finish(false, "Play mode timeout");
        if (!EditorApplication.isPlaying || EditorApplication.isCompiling) return;
        if (!spell)
        {
            spell = Object.FindFirstObjectByType<Spell>(FindObjectsInactive.Include);
            spell.transform.localPosition = new Vector3(50, 50, 0);
            started = Time.time;
            spell.gameObject.SetActive(true);
            return;
        }
        if (spell.gameObject.activeSelf) return;
        float elapsed = Time.time - started;
        if (elapsed < .98f || elapsed > 1.15f || spell.GetComponent<Collider2D>().enabled)
        {
            Finish(false, "Spell lifetime=" + elapsed);
            return;
        }
        Debug.Log("PASS: Real play-mode spell activation " + (++activations) + " lasted " + elapsed + " seconds");
        if (activations == 2) Finish(true, "Two one-second activations and disabled colliders");
        else
        {
            started = Time.time;
            spell.gameObject.SetActive(true);
        }
    }
    static void Finish(bool success, string message)
    {
        SessionState.SetBool("MG2.PlayCheck", false);
        EditorApplication.update -= Tick;
        Debug.Log("SPELL_PLAY_CHECK " + (success ? "PASS: " : "FAIL: ") + message);
        EditorApplication.Exit(success ? 0 : 1);
    }
}
