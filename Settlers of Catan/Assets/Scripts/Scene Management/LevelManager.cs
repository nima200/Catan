using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public void LoadScene(string name)
    {
        Debug.Log(name + " Scene Requested");
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Debug.Log("Exit requested");
        Application.Quit();
    }
}
