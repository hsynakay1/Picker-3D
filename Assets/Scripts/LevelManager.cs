using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public int levelIndex = 0;
    [SerializeField] GameObject[] levels;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        levelIndex = PlayerPrefs.GetInt(nameof(levelIndex), levelIndex);
        LevelCounter();
    }
    
    public void NextLevel()
    {
        levelIndex++;
        LevelCounter();
        PlayerPrefs.SetInt(nameof(levelIndex), levelIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        
        LevelCounter();
    }
    
    public void LevelCounter()
    {
        if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
        }
        foreach (var level in levels)
        {
            level.gameObject.SetActive(false);
        }
        levels[levelIndex].SetActive(true);
    }

}
