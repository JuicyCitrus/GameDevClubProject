using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public string indoorSceneName;

    public override void Interact()
    {
        SceneManager.LoadScene(indoorSceneName);
    }
}
