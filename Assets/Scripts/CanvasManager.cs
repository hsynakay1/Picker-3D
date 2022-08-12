using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManager : Singleton<CanvasManager>
{
    public Canvas[] Canvases;
    public int index;
    public TextMeshProUGUI levelText;

    private void Awake()
    {
        if (LevelManager.Instance.levelIndex != 0)
        {
            index = 1; 
            GameManager.Instance.Status();
        }
        else
            index = 0;
    }
    private void Start()
    {
        int level = LevelManager.Instance.levelIndex + 1;
        levelText.text = level.ToString();  
    }
    private void Update()
    {
        foreach (Canvas canvas in Canvases)
        {
            canvas.gameObject.SetActive(false);

        }
        Canvases[index].gameObject.SetActive(true);

        
    }
    public void TabToStart()
    {
        GameManager.Instance.Status();
        index++;
    }
    public void MainCanvas()
    {
        index = 1;
    }
    public void LoseCanvas()
    {
        index = 2;
    }
    public void WinCanvas()
    {
        index = 3;
    }
    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        index = 1;
    }
    public void Retry()
    {
        LevelManager.Instance.RestartLevel();
        index = 1;
    }
    public void Quit()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }

}
