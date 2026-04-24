using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimSceneChange : MonoBehaviour
{
    public string sceneName; // Type your scene name in Inspector
    public float TimeOfScene;

    private void Update()
    {
        if (TimeOfScene > 0)
        {
            TimeOfScene -= Time.deltaTime;
            print(TimeOfScene);
        }

        if (TimeOfScene <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}