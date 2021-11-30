using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController Current;
    public bool gameActive = false;

    public GameObject startMenu, gameMenu, gameOverMenu, finishMenu;
    public Text scoreText, finishScoreText, currentLevelText, nextLevelText,startingMenuMoneyText,gameOverMenuMoneyText, finishGameMenuMoneyText;

    int currentLevel;
    int score;

    public Slider levelProgressBar;
    public float maxDistance;
    public GameObject finisLine;

    public AudioSource gameAudioSource;
    public AudioClip victoryAudioClip;
    public AudioClip gameOverAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        Current = this;
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        if (SceneManager.GetActiveScene().name != "Level " + currentLevel)
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
        else
        {
            currentLevelText.text = (currentLevel + 1).ToString();
            nextLevelText.text = (currentLevel + 2).ToString();
            int money = PlayerPrefs.GetInt("money");
            startingMenuMoneyText.text = money.ToString();
        }
        gameAudioSource = Camera.main.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            PlayerController player = PlayerController.current;
            float distance= finisLine.transform.position.z - PlayerController.current.transform.position.z;
            levelProgressBar.value = 1 - (distance/maxDistance);
        }
    }

    public void StartLevel()
    {
        maxDistance = finisLine.transform.position.z - PlayerController.current.transform.position.z;

        PlayerController.current.ChangeSpeed(PlayerController.current.runningSpeed);
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        gameActive = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level "+(currentLevel+1));
    }

    public void GameOver()
    {
        gameOverMenuMoneyText.text = PlayerPrefs.GetInt("money").ToString();
        gameAudioSource.Stop();
        gameAudioSource.PlayOneShot(gameOverAudioClip);
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        gameActive = false;
    }

    public void FinishGame()
    {
        int money = PlayerPrefs.GetInt("money");
        PlayerPrefs.SetInt("money", money + score);
        finishGameMenuMoneyText.text = PlayerPrefs.GetInt("money").ToString();
        gameAudioSource.Stop();
        gameAudioSource.PlayOneShot(victoryAudioClip);
        PlayerPrefs.SetInt("currentLevel",currentLevel+1);
        finishScoreText.text = score.ToString();
        gameMenu.SetActive(false);
        finishMenu.SetActive(true);
        gameActive = false;
    }

    public void ChangeScore(int increment)
    {
        score += increment;
        scoreText.text = score.ToString();
    }
}
