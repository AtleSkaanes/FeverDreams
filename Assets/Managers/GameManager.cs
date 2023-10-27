using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public UnityAction<Vector3> OnNoise;

    new void Awake()
    {
        base.Awake();
    }

    public void RestartLvl()
    {
        Debug.Log("Restarting...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
