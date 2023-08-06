using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Scene nextScene;
    public void NextScene()
    {
        //SceneManager.LoadScene(nextScene.buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
