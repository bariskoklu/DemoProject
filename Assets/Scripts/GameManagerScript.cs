using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }

    public int numberOfBallsHitGround = 0;
    public GameState currentGameState = GameState.Playing;

    public List<GameObject> allLevels;
    public List<GameObject> levels;
    public GameObject currentLevel;

    private int currentLevelIndex = 0;

    [SerializeField]
    private GameObject defeatUI;
    [SerializeField]
    private GameObject victoryUI;
    [SerializeField]
    private GameObject gameStartUI;
    [SerializeField]
    private GameObject playingUI;
    [SerializeField]
    private Text ballInfoText;
    [SerializeField]
    private Text levelText;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        AddFirstLevels();
    }

    public void AddFirstLevels()
    {
        for (int i = 0; i < allLevels.Count; i++)
        {
            AddLevel(allLevels[i]);
        }
    }

    private void AddLevel(GameObject level)
    {
        //Finding the FinishPlatform object of the current last level, so we can add this level to finish of that level.
        if (levels.Count > 0)
        {
            GameObject lastLevel = levels[levels.Count - 1];

            GameObject finishPlatform = lastLevel.transform.Find("FinishPlatform").gameObject;
            Debug.Log(finishPlatform.transform.parent.position);
            float positionOfWhereLastPlatformFinishes = finishPlatform.GetComponent<Renderer>().bounds.size.z / 2 + finishPlatform.transform.position.z;

            levels.Add(Instantiate(level, new Vector3(lastLevel.transform.position.x, lastLevel.transform.position.y, positionOfWhereLastPlatformFinishes), lastLevel.transform.rotation));
        }
        else
        {
            levels.Add(Instantiate(level, new Vector3(0,0,0), Quaternion.identity));
            currentLevel = levels[0];
        }

    }

    public void ChangeGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameOverVictory:
                currentGameState = GameState.GameOverVictory;
                victoryUI.SetActive(true);
                playingUI.SetActive(false);
                break;
            case GameState.GameOverDefeat:
                currentGameState = GameState.GameOverDefeat;
                defeatUI.SetActive(true);
                playingUI.SetActive(false);
                break;
            case GameState.GameStart:
                //not used
                break;
            case GameState.Playing:
                defeatUI.SetActive(false);
                victoryUI.SetActive(false);
                gameStartUI.SetActive(false);
                playingUI.SetActive(true);

                numberOfBallsHitGround = 0;
                if (currentGameState != GameState.GameStart)
                {
                    AddLevel(allLevels[UnityEngine.Random.Range(0, allLevels.Count - 1)]);
                    currentLevelIndex++;
                    currentLevel = levels[currentLevelIndex];
                }

                levelText.text = "Level : " + (currentLevelIndex + 1).ToString();
                UpdateBallInfoText();
                currentGameState = GameState.Playing;
                break;
            default:
                break;
        }
    }
    //For button
    public void ChangeGameStateToPlaying()
    {
        ChangeGameState(GameState.Playing);
    }

    public void CheckForLevelEnd()
    {
        if (numberOfBallsHitGround >= currentLevel.GetComponent<LevelController>().totalNumberOfBallsToFinish)
        {
            ChangeGameState(GameState.GameOverVictory);
        }
        else
        {
            ChangeGameState(GameState.GameOverDefeat);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void UpdateBallInfoText()
    {
        ballInfoText.text = numberOfBallsHitGround + " / " + currentLevel.GetComponent<LevelController>().totalNumberOfBallsToFinish;
    }
}
