using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{  
    public TextMeshProUGUI score;
    public TextMeshProUGUI lives;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI firstScore;
    public TextMeshProUGUI secondScore;
    public TextMeshProUGUI thirdScore;

    public GameObject Panel;
    public GameObject Heart1;
    public GameObject Heart2;
    public GameObject Heart3;

    public bool GameOverTriggered;

    void Start()
    {
        Panel.SetActive(false);
    }

    void Update()
    {
        score.SetText(GameManager.playerScore.ToString());
        //lives.SetText(PlayerController.playerLives.ToString());
        GameOver();

        if (GameManager.playerLives < 3)
        {
            if (GameManager.playerLives == 2) 
            {
                Heart3.SetActive(false);
            }
            else if(GameManager.playerLives == 1)
            {
                Heart2.SetActive(false);
            }
            else if (GameManager.playerLives == 0)
            {
                Heart1.SetActive(false);
            }
        }
    }


    void UpdateHighScore()
    {
        if (PlayerPrefs.HasKey("SavedFirstScore") || PlayerPrefs.HasKey("SavedSecondScore") || PlayerPrefs.HasKey("SavedThirdScore"))
        {                      
            if (GameManager.playerScore > PlayerPrefs.GetInt("SavedThirdScore"))
            {
                PlayerPrefs.SetInt("SavedThirdScore", GameManager.playerScore);
                //Debug.Log(PlayerPrefs.GetInt("SavedThirdScore"));
            }
            if (PlayerPrefs.GetInt("SavedThirdScore") > PlayerPrefs.GetInt("SavedSecondScore"))
            {
                int secondScore = PlayerPrefs.GetInt("SavedSecondScore");
                PlayerPrefs.SetInt("SavedSecondScore", PlayerPrefs.GetInt("SavedThirdScore"));
                PlayerPrefs.SetInt("SavedThirdScore", secondScore);
                //Debug.Log(PlayerPrefs.GetInt("SavedSecondScore"));
                //Debug.Log(PlayerPrefs.GetInt("SavedThirdScore"));
            }
            if (PlayerPrefs.GetInt("SavedSecondScore") > PlayerPrefs.GetInt("SavedFirstScore"))
            {
                int firstScore = PlayerPrefs.GetInt("SavedFirstScore");
                PlayerPrefs.SetInt("SavedFirstScore", PlayerPrefs.GetInt("SavedSecondScore"));
                PlayerPrefs.SetInt("SavedSecondScore", firstScore);
                //Debug.Log(PlayerPrefs.GetInt("SavedFirstScore"));
                //Debug.Log(PlayerPrefs.GetInt("SavedSecondScore"));
                //Debug.Log(PlayerPrefs.GetInt("SavedThirdScore"));
            }
        }
        else
        {
            PlayerPrefs.SetInt("SavedFirstScore", GameManager.playerScore);
            PlayerPrefs.SetInt("SavedSecondScore", 0);
            PlayerPrefs.SetInt("SavedThirdScore", 0);
        }
    }

    private void GameOver()
    {
        if (GameManager.playerLives <= 0 && !GameOverTriggered)
        {
            GameOverTriggered = true;
            Time.timeScale = 0;

            UpdateHighScore();

            Panel.SetActive(true);

            finalScore.SetText("Score: " + GameManager.playerScore.ToString());
            firstScore.SetText("First: " + PlayerPrefs.GetInt("SavedFirstScore"));
            secondScore.SetText("Second: " + PlayerPrefs.GetInt("SavedSecondScore"));
            thirdScore.SetText("Third: " + PlayerPrefs.GetInt("SavedThirdScore"));
        }
    }

    public void RestartUI()
    {
        GameOverTriggered = false;
        Panel.SetActive(false);
        Heart3.SetActive(true);
        Heart2.SetActive(true);
        Heart1.SetActive(true);

    }
}
