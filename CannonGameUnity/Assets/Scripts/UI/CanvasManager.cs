using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject startingMenu;
    public GameObject startingMenuButtonContainer;
    public GameObject optionsMenu;
    public GameObject levelTypeSelectMenu;
    public GameObject levelTypeSelectButtonContainer;
    public GameObject levelNumberSelectMenu;
    public GameObject levelNumberSelectButtonContainer;
    public GameObject inGameUI;
    public GameObject levelStats;
    public GameObject confetti;
    public TextMeshProUGUI levelStatsText;
    public GameObject centralMenuButtonPrefab;
    public GameObject titleText;

    public CannonPlatform cannonPlatform;

    public int score = 0;
    public TextMeshProUGUI shotsText;
    public int shots = 0;
    public TextMeshProUGUI distanceText;

    public AudioClip buttonClick;
    public AudioSource audioSource;

    List<GameObject> startingMenuButtons = new List<GameObject>();
    List<GameObject> optionMenuButtons = new List<GameObject>();
    List<GameObject> levelNumberMenuButtons = new List<GameObject>();

    float levelStartTime;

    string[] startingMenuButtonNames = new string[] {
        "Levels",
        "Options"
    };

    string[] optionMenuButtonNames = new string[] {
        "Resume",
        "Back to main menu",
        "Mute / unmute game",
        "Reset Longest Shot"
    };

    string[] levelTypeSelectButtonNames = new string[] {
        "Vikings",
        "Sailors",
        "Pirates",
        "Navy"
    };

    private void Start()
    {
        centralMenuButtonPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
        foreach (Button button in FindObjectsOfType<Button>())
        {
            button.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        foreach (string buttonName in startingMenuButtonNames)
        {
            GameObject newButton = Instantiate(centralMenuButtonPrefab, startingMenuButtonContainer.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonName;
            startingMenuButtons.Add(newButton);
        }
        startingMenuButtons[0].GetComponent<Button>().onClick.AddListener(SeeLevelTypeMenu);
        startingMenuButtons[1].GetComponent<Button>().onClick.AddListener(SeeOptionsMenu);
        foreach (string buttonName in optionMenuButtonNames)
        {
            GameObject newButton = Instantiate(centralMenuButtonPrefab, optionsMenu.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonName;
            optionMenuButtons.Add(newButton);
        }
        optionMenuButtons[0].GetComponent<Button>().onClick.AddListener(Resume);
        optionMenuButtons[1].GetComponent<Button>().onClick.AddListener(SeeStartingMenu);
        optionMenuButtons[2].GetComponent<Button>().onClick.AddListener(MuteGame);
        optionMenuButtons[3].GetComponent<Button>().onClick.AddListener(ResetLongestShot);
        foreach (string buttonName in levelTypeSelectButtonNames)
        {
            GameObject newButton = Instantiate(centralMenuButtonPrefab, levelTypeSelectButtonContainer.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonName;
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SetLevelType(buttonName); });
            newButton.GetComponent<Button>().onClick.AddListener(SeeLevelNumberMenu);
        }
        for (int i = 0; i < 20; i++)
        {
            int levelNumber = i; // Create a local variable and assign the current value of i
            GameObject newButton = Instantiate(centralMenuButtonPrefab, levelNumberSelectButtonContainer.transform);
            // Use the local variable levelNumber instead of i in the lambda function
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SetLevelNumber(levelNumber); });
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SeeLevelStats(false); });
            levelNumberMenuButtons.Add(newButton);
        }

        Button[] allButtons = FindObjectsOfType<Button>();
        foreach(Button button in allButtons)
        {
            button.onClick.AddListener(PlayButtonClickSound);
        }
        SeeStartingMenu();
    }

    void HideAllMenus()
    {
        startingMenu.SetActive(false);
        optionsMenu.SetActive(false);
        inGameUI.SetActive(false);
        levelTypeSelectMenu.SetActive(false);
        levelNumberSelectMenu.SetActive(false);
        levelStats.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        HideAllMenus();
        inGameUI.SetActive(true);
    }

    public void StartGame()
    {
        confetti.SetActive(false);
        Time.timeScale = 1f;
        levelStartTime = Time.time;
        cannonPlatform.Reset();
        gameManager.InitializeLevel();
        if (PlayerPrefs.HasKey("maxShotDistance"))
            UpdateDistance();
        HideAllMenus();
        inGameUI.SetActive(true);
    }

    public void SeeOptionsMenu()
    {
        Time.timeScale = 0f;
        HideAllMenus();
        optionsMenu.SetActive(true);
    }

    public void SeeLevelTypeMenu()
    {
        Time.timeScale = 0f;
        HideAllMenus();
        levelTypeSelectMenu.SetActive(true);
    }

    public void SeeLevelNumberMenu()
    {
        Time.timeScale = 0f;
        confetti.SetActive(false);
        HideAllMenus();
        if (gameManager.levelType == "Vikings")
        {
            levelNumberMenuButtons[0].GetComponent<Button>().interactable = true;
            PlayerPrefs.SetInt(gameManager.levelType + "0", 1);
        }
        for(int i = 0; i < levelNumberMenuButtons.Count; i ++)
        {
            levelNumberMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            if (PlayerPrefs.GetInt(gameManager.levelType + i.ToString()) != 1)
            {
                levelNumberMenuButtons[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                levelNumberMenuButtons[i].GetComponent<Button>().interactable = true;
            }
        }
        levelNumberSelectMenu.SetActive(true);
    }

    public void SeeStartingMenu()
    {
        Time.timeScale = 0f;
        HideAllMenus();
        startingMenu.SetActive(true);
    }

    public void SeeLevelStats(bool completedGame)
    {
        Time.timeScale = 0f;
        HideAllMenus();
        if (PlayerPrefs.GetInt(gameManager.levelType + gameManager.levelNumber.ToString()) != 1)
        {
            levelStatsText.text = (gameManager.levelNumber + 1).ToString() + "Uncompleted";
        }
        else
        {
            string levelString = "";
            if (completedGame)
            {
                levelString += "Congradulations on completing Cannon Balls\n";
                Time.timeScale = 1f;
                confetti.SetActive(true);
            }
            string levelShots = PlayerPrefs.GetInt(gameManager.levelType + gameManager.levelNumber.ToString() + "shots").ToString();
            string levelTime = PlayerPrefs.GetInt(gameManager.levelType + gameManager.levelNumber.ToString() + "time").ToString();
            levelString += "<size=100%>" + (gameManager.levelNumber + 1).ToString() + "<size=50%>\nShots: " + levelShots + "\nTime: " + levelTime;
            levelStatsText.text = levelString;
        }
        levelStats.SetActive(true);
    }

    public void MuteGame()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void ResetLongestShot()
    {
        if (PlayerPrefs.HasKey("maxShotDistance"))
        {
            PlayerPrefs.SetInt("maxShotDistance", 0);
            UpdateDistance();
        }
    }

    public void PlayButtonClickSound()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
    }

    public void UpdateScore()
    {
        score += 1;
    }

    public void UpdateShots()
    {
        shots += 1;
        shotsText.text = "Shots: " + shots.ToString();
    }

    public void UpdateDistance()
    {
        distanceText.text = "Distance of longest shot: " + PlayerPrefs.GetInt("maxShotDistance").ToString();
    }

    void UnlockNextLevel()
    {
        if (gameManager.levelNumber != 19)
            PlayerPrefs.SetInt(gameManager.levelType + (gameManager.levelNumber + 1).ToString(), 1);
        else if (gameManager.levelType != "Navy")
        {
            if (gameManager.levelType == "Vikings")
                PlayerPrefs.SetInt("Sailors" + "0", 1);
            else if (gameManager.levelType == "Sailors")
                PlayerPrefs.SetInt("Pirates" + "0", 1);
            else
                PlayerPrefs.SetInt("Navy" + "0", 1);
        }
    }

    public void WinGame()
    {
        UnlockNextLevel();
        if (!PlayerPrefs.HasKey(gameManager.levelType + gameManager.levelNumber.ToString() + "shots") || shots < PlayerPrefs.GetInt(gameManager.levelType + gameManager.levelNumber.ToString() + "shots"))
        {
            Debug.Log(shots);
            PlayerPrefs.SetInt(gameManager.levelType + gameManager.levelNumber.ToString() + "shots", shots);
        }
        if (!PlayerPrefs.HasKey(gameManager.levelType + gameManager.levelNumber.ToString() + "time") || (int)Mathf.Floor(Time.time - levelStartTime) < PlayerPrefs.GetInt(gameManager.levelType + gameManager.levelNumber.ToString() + "time"))
            PlayerPrefs.SetInt(gameManager.levelType + gameManager.levelNumber.ToString() + "time", (int)Mathf.Floor(Time.time - levelStartTime));
        StartCoroutine(EndGame("YOU WIN!"));
    }

    public void LoseGame()
    {
        StartCoroutine(EndGame("You lose..."));
    }

    IEnumerator EndGame(string message)
    {
        shotsText.text = message + "\nShots per hit: " + (Mathf.Round((float)shots / (float)score  * 100f) / 100f).ToString();
        distanceText.fontSize = 0;
        yield return new WaitForSeconds(6f);
        score = 0;
        shots = 0;
        shotsText.text = "Shots: " + shots.ToString();
        distanceText.fontSize = 18;
        bool completedGame = message == "YOU WIN!" && gameManager.levelType == "Navy" && gameManager.levelNumber == 19 ? true : false;
        SeeLevelStats(completedGame);
        gameManager.EndLevel();
    }

    void SetLevelType(string type)
    {
        gameManager.levelType = type;
    }

    void SetLevelNumber(int num)
    {
        gameManager.levelNumber = num;
    }
}
