using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapSceneManager : MonoBehaviour
{
    public static BootstrapSceneManager Instance { get; private set; }

    public string firstSceneName;
    public string combatSceneName;
    public string previousSceneName;

    public GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadFirstScene());
    }

    public void LoadNewScene(string current, string next)
    {
        // Handle the player based on whether or not combat is active
        if (next == combatSceneName)
        {
            // Disable the player when the combat scene becomes active
            if (player != null)
            {
                player.SetActive(false);
            }
        }
        else if (current == combatSceneName)
        {
            // Re-enable the player when leaving the combat scene
            if (player != null)
            {
                player.SetActive(true);
            }
        }

        // Load the scene
        StartCoroutine(LoadScene(next));
    }

    private IEnumerator LoadFirstScene()
    {
        yield return SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstSceneName));
    }

    private IEnumerator LoadScene(string SceneToBeLoaded)
    {
        // Get the current active scene to unload later and set it as the previous scene for reference later
        Scene currentScene = SceneManager.GetActiveScene();
        previousSceneName = currentScene.name;

        // Load the new scene additively
        yield return SceneManager.LoadSceneAsync(SceneToBeLoaded, LoadSceneMode.Additive);

        // Set the new scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneToBeLoaded));

        // Unload the current scene
        yield return SceneManager.UnloadSceneAsync(currentScene);
    }
}
