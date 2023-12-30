using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    public int index = 0;

    public GameObject recipe;
    public GameObject score;
    public GameObject time;

    public float counter = 6f;

    // Start is called before the first frame update
    void Start()
    {
        recipe.SetActive(false); 
        score.SetActive(false); 
        time.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < popUps.Length; i++) 
        {
            if (i == index)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }

        if (index == 0)
        {
            // Splitting

            // Wait 6 sec
            if (counter <= 0)
            {
                index++;
                counter = 6f;
            }
            else
            {
                counter -= Time.deltaTime;
            }
        }
        else if (index == 1)
        {
            // Player Movement

            // Wait for player input
            var players = FindObjectsOfType<PlayerMovement>();
        
            foreach(PlayerMovement player in players)
            {
                if (player.movementInput.magnitude > 0)
                {
                    index++;
                    break;
                }
            }
        }
        else if (index == 2)
        {
            // Mining

            // Wait for destroyed rock

            if (FindObjectOfType<OreController>().mined)
            {
                index++;
            }
        }
        else if (index == 3)
        {
            // Recipe & Smelting

            // Wait for smelted ingot
            recipe.SetActive(true);

            if (FindObjectOfType<SmelterController>().outputCount == -1)
            {
                index++;
            }

        }
        else if (index == 4)
        {
            // Submitting & Score

            // Wait for score to increase
            score.SetActive(true);

            if (FindObjectOfType<GameManager>().score > 0)
            {
                index++;
            }

        }
        else if (index == 5)
        {
            // Time

            // Wait 6 seconds
            time.SetActive(true);

            if (counter <= 0)
            {
                index++;
            }
            else
            {
                counter -= Time.deltaTime;
            }

            // Update the timer
            foreach (Transform child in time.transform)
            {
                if (child.gameObject.name == "Time Left")
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = string.Format("00:{0:00}", (int)counter);
                }
            }
        }
        else if (index > 5)
        {
            // End of Tutorial

            // Show screen with retry or return to main menu
            
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
