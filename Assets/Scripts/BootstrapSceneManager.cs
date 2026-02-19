using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapSceneManager : MonoBehaviour
{
    public string firstSceneName;
    public string combatSceneName;
    public string previousSceneName;

    public GameObject player;

    private void Start()
    {
        StartCoroutine(LoadFirstScene());

        SceneManager.activeSceneChanged += (current, next) =>
        {
            LoadNewScene(current, next);
        };
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= (current, next) =>
        {
            LoadNewScene(current, next);
        };
    }

    public void LoadNewScene(Scene current, Scene next)
    {

        if (next.name == combatSceneName)
        {
            // Disable the player when the combat scene becomes active
            if (player != null)
            {
                player.SetActive(false);
            }
        }
        else if (current.name == combatSceneName)
        {
            // Re-enable the player when leaving the combat scene
            if (player != null)
            {
                player.SetActive(true);
            }
        }

        previousSceneName = current.name;
    }

    private IEnumerator LoadFirstScene()
    {
        yield return SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstSceneName));
    }
}
