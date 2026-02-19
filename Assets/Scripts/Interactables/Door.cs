using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public string indoorSceneName;

    public override void Interact(GameObject player)
    {
        base.Interact(player);

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        // Get the current active scene to unload later
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the new scene additively
        yield return SceneManager.LoadSceneAsync(indoorSceneName, LoadSceneMode.Additive);

        // Set the new scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(indoorSceneName));

        // Reset player position
        playerReference.transform.position = Vector3.zero;

        // Unload the current scene
        yield return SceneManager.UnloadSceneAsync(currentScene);
    }
}
