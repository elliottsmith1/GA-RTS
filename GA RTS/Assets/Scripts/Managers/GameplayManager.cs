using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    [SerializeField] PlayerManager playerManager;
    [SerializeField] AIManager aiManager;
    [SerializeField] UIManager uiManager;

    private bool gameOver = false;

    private float timeElapsed = 0.0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GameState();
    }

    private void GameState()
    {
        if (!gameOver)
        {
            timeElapsed += Time.deltaTime;

            if (playerManager.GetBuildingManager().GetPlayerBuildings().Count < 1)
            {
                gameOver = true;
                uiManager.GameOver(false, timeElapsed);
            }

            if (aiManager.GetEnemyBuildings().Count < 1)
            {
                gameOver = true;
                uiManager.GameOver(true, timeElapsed);
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public bool GetGameOver()
    {
        return gameOver;
    }
}
