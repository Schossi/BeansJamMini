using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    private InputField _nameInput;

    private Text _sprintHSText;
    private Text _marathonHSText;
    private Text _sprintHSScoresText;
    private Text _marathonHSScoresText;

    // Use this for initialization
    void Start()
    {
        _nameInput = transform.Find("NamePanel").Find("InputField").GetComponent<InputField>();

        var hs = transform.Find("HighscoresPanel");
        _sprintHSText = hs.Find("SprintHSText").GetComponent<Text>();
        _marathonHSText = hs.Find("MarathonHSText").GetComponent<Text>();
        _sprintHSScoresText = hs.Find("SprintHSScoresText").GetComponent<Text>();
        _marathonHSScoresText = hs.Find("MarathonHSScoresText").GetComponent<Text>();

        _nameInput.text = PlayerPrefs.GetString("PlayerName");

        _sprintHSText.text = "loading...";
        _marathonHSText.text = "loading...";
        _sprintHSScoresText.text = "";
        _marathonHSScoresText.text = "";
        
        loadHighscore((int)GameModes.Sprint, _sprintHSText);
        loadHighscore((int)GameModes.Marathon, _marathonHSText);
    }

    private void loadHighscore(int category,Text text)
    {
        StartCoroutine(HighscoreManager.GetScores(category, Application.isMobilePlatform, s => text.text=s, s => text.text = s));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerNameChanged()
    {
        PlayerPrefs.SetString("PlayerName", _nameInput.text);
        Settings.PlayerName = _nameInput.text;
    }

    public void StartSprint()
    {
        startGame(GameModes.Sprint);
    }

    public void StartMarathon()
    {
        startGame(GameModes.Marathon);
    }

    private void startGame(GameModes mode)
    {
        Settings.Mode = mode;
        SceneManager.LoadScene("Main");
    }
}