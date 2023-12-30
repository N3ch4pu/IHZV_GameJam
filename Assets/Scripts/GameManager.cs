using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float levelDuration;
    public float timeLeft;

    public TMP_Text timeText;
    public TMP_Text scoreText;

    public float score;

    public List<TextMeshProUGUI> names;
    public List<TextMeshProUGUI> scores;

    public int level;

    public GameObject leaderboardUI;
    public GameObject gameplayUI;

    public TextMeshProUGUI finalScore;

    private string leaderboardPublicKey = "";

    public TMP_InputField inputName;

    public UnityEvent<string, int> submitScore;

    public bool gameEnded;

    void Start()
    {
        if (level == 1)
        {
            leaderboardPublicKey = "7d021c8e66ce363886fbc452235079893fc2aa1c44621dcf7078c103d9cb6e8e";
        }
        else if (level == 2) 
        {
            leaderboardPublicKey = "fe25c7d8878fe8c62943ecd88d2181359eaf126ff4c5df15910e089b8a90ec7b";
        }
        else if (level == 3)
        {
            leaderboardPublicKey = "8c77f0645632fb2bf332d2fc2acbecc0f2684a0231e5e30ff23353ad6acb33fb";
        }
        else if (level == 4)
        {
            leaderboardPublicKey = "ef1421d5b6346d8be243461cfd4ef2e20ea89f5fa99bf04c9dfff84777c0aed6";
        }

        timeLeft = levelDuration;
        score = 0;
        scoreText.text = "0";

        leaderboardUI.SetActive(false);
        gameplayUI.SetActive(true);
        gameEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime; 

        if (timeLeft <= 0 && !gameEnded)
        {
            EndGame();
            gameEnded = true;
        }

        var minutes = (int) (timeLeft / 60);
        var seconds = (int) (timeLeft % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void EndGame()
    {
        // Disable player movement
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        foreach(PlayerInput player in players)
        {
            player.enabled = false;
        }

        finalScore.text = "Your Score: " + scoreText.text;

        leaderboardUI.SetActive(true);
        gameplayUI.SetActive(false);
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(leaderboardPublicKey, ((message) =>
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (i < message.Length)
                {
                    names[i].text = message[i].Username;
                    scores[i].text = message[i].Score.ToString();
                }
                else
                {
                    names[i].text = "";
                    scores[i].text = "";
                }
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(leaderboardPublicKey, username, score, ((message) =>
        {
            GetLeaderboard();
        }));
    }

    public void SubmitScore()
    {
        submitScore.Invoke(inputName.text, (int)score);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
