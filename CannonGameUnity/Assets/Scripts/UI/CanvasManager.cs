using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject startingMenu;
    public GameObject optionsMenu;
    public GameObject inGameUI;
    public GameObject centralMenuButtonPrefab;

    public CannonPlatform cannonPlatform;

    public TextMeshProUGUI scoreText;
    public int score = 0;
    public TextMeshProUGUI shotsText;
    public int shots = 0;
    public TextMeshProUGUI distanceText;

    public AudioClip buttonClick;
    public AudioSource audioSource;

    List<GameObject> startingMenuButtons = new List<GameObject>();
    List<GameObject> optionMenuButtons = new List<GameObject>();

    string[] startingMenuButtonNames = new string[] {
        "Start",
        "Options"
    };

    string[] optionMenuButtonNames = new string[] {
        "Resume",
        "Back to main menu",
        "Mute / unmute game",
        "Reset Longest Shot"
    };

    private void Start()
    {
        foreach (string buttonNmae in startingMenuButtonNames)
        {
            GameObject newButton = Instantiate(centralMenuButtonPrefab, startingMenu.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonNmae;
            startingMenuButtons.Add(newButton);
        }
        startingMenuButtons[0].GetComponent<Button>().onClick.AddListener(StartGame);
        startingMenuButtons[1].GetComponent<Button>().onClick.AddListener(SeeOptionsMenu);
        foreach (string buttonNmae in optionMenuButtonNames)
        {
            GameObject newButton = Instantiate(centralMenuButtonPrefab, optionsMenu.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonNmae;
            optionMenuButtons.Add(newButton);
        }
        optionMenuButtons[0].GetComponent<Button>().onClick.AddListener(Resume);
        optionMenuButtons[1].GetComponent<Button>().onClick.AddListener(SeeStartingMenu);
        optionMenuButtons[2].GetComponent<Button>().onClick.AddListener(MuteGame);
        optionMenuButtons[3].GetComponent<Button>().onClick.AddListener(ResetLongestShot);
        Button[] allButtons = FindObjectsOfType<Button>();
        foreach(Button button in allButtons)
        {
            button.onClick.AddListener(PlayButtonClickSound);
        }
        optionsMenu.SetActive(false);
        inGameUI.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        startingMenu.SetActive(false);
        optionsMenu.SetActive(false);
        inGameUI.SetActive(true);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        cannonPlatform.ResetCannons();
        gameManager.DestroyAllShips();
        foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
            Destroy(ps.gameObject);
        if (PlayerPrefs.HasKey("maxShotDistance"))
            UpdateDistance();
        startingMenu.SetActive(false);
        optionsMenu.SetActive(false);
        inGameUI.SetActive(true);
    }

    public void SeeOptionsMenu()
    {
        Time.timeScale = 0f;
        startingMenu.SetActive(false);
        inGameUI.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void SeeStartingMenu()
    {
        Time.timeScale = 0f;
        optionsMenu.SetActive(false);
        inGameUI.SetActive(false);
        startingMenu.SetActive(true);
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

    public void UpdaeScore(int additionalPoints)
    {
        score += additionalPoints;
        scoreText.text = "Score: " + score.ToString() + "/20";
        if (score >= 20)
            StartCoroutine(EndGame());

    }

    public void UpdaeShots()
    {
        shots += 1;
        shotsText.text = "Shots: " + shots.ToString();
    }

    public void UpdateDistance()
    {
        distanceText.text = "Distance of longest shot: " + PlayerPrefs.GetInt("maxShotDistance").ToString();
    }

    IEnumerator EndGame()
    {
        scoreText.text = "YOU WIN!\nScore per shot: " + (Mathf.Round((float)score / (float)shots * 100f) / 100f).ToString();
        scoreText.fontSize = 12;
        shotsText.fontSize = 0;
        distanceText.fontSize = 0;
        yield return new WaitForSeconds(6f);
        score = 0;
        shots = 0;
        scoreText.text = "Score: " + score.ToString() + "/20";
        shotsText.text = "Shots: " + shots.ToString();
        scoreText.fontSize = 30;
        shotsText.fontSize = 18;
        distanceText.fontSize = 18;
        SeeStartingMenu();
        gameManager.DestroyAllShips();
    }
}
