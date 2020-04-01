using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{

    public static GameSceneManager current;
    // Start is called before the first frame update
    private void Awake()
    {
        if(GameSceneManager.current == null)
        {
            GameSceneManager.current = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameScene()
    {

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void LoadLoseScene()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(1);
    }
}
