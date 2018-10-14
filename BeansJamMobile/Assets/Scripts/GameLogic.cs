using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public enum GameLogicState { Start, Play, End }

    private GameLogicState State = GameLogicState.Start;

    private bool _timeCritical = false;

    private int _counter = 0;
    private int _score = 0;
    private float _colorPoints = 0;

    private float _maxTime;
    private float _time;

    private CanvasGroup _endCanvas;
    private Text _mainText;
    private Text _scoreText;
    private Text _sendScoreText;
    private RectTransform _timeTransform;
    private Animator _timeAnimator;

    // Use this for initialization
    void Start()
    {
        _endCanvas = transform.Find("EndButtons").GetComponent<CanvasGroup>();
        _mainText = transform.Find("MainText").GetComponent<Text>();
        _scoreText = transform.Find("ScoreText").GetComponent<Text>();
        _sendScoreText = transform.Find("SendScoreText").GetComponent<Text>();
        _timeTransform = transform.Find("TimePanel").GetComponent<RectTransform>();
        _timeAnimator = transform.Find("TimePanel").GetComponent<Animator>();

        _sendScoreText.text = "";

        StartCoroutine(Countdown());

        _endCanvas.alpha = 0.0f;
        _endCanvas.interactable = false;
        _endCanvas.blocksRaycasts = false;

        if (Settings.Mode == GameModes.Sprint)
        {
            _maxTime = 30;
            _time = _maxTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameLogicState.Play)
        {
            checkMatch();

            _colorPoints = Mathf.Max(_colorPoints - Time.deltaTime * 50, 10);

            _time -= Time.deltaTime;

            _timeTransform.anchorMax = new Vector2(_time / _maxTime, _timeTransform.anchorMax.y);

            if (!_timeCritical && _time / _maxTime <= 0.2)
            {
                _timeCritical = true;
                _timeAnimator.SetBool("critical", true);
            }
            if (_timeCritical && _time / _maxTime > 0.2)
            {
                _timeCritical = false;
                _timeAnimator.SetBool("critical", false);
            }

            if (_time <= 0.0f)
            {
                GameMixer.Instance.ClearColor();
                State = GameLogicState.End;
                _mainText.text = "FIN";
                _endCanvas.alpha = 1.0f;
                _endCanvas.interactable = true;
                _endCanvas.blocksRaycasts = true;

                if (string.IsNullOrEmpty(Settings.PlayerName))
                {
                    _sendScoreText.text = "please set a player name in the main menu to submit high scores";
                }
                else
                {
                    _sendScoreText.text = "submitting score...";

                    StartCoroutine(HighscoreManager.PostScore(Settings.PlayerName, _score, (int)Settings.Mode, Application.isMobilePlatform, () =>
                    {
                        _sendScoreText.text = "score sucessfully submitted"+Environment.NewLine+"loading placement...";

                        StartCoroutine(HighscoreManager.GetPlacement(Settings.PlayerName, (int)Settings.Mode, Application.isMobilePlatform, s =>
                           {
                               _sendScoreText.text = "score sucessfully submitted" + Environment.NewLine + s;
                           }, s =>
                         {
                               _sendScoreText.text = "score sucessfully submitted" + Environment.NewLine + s;
                           }));
                    }, s =>
                    {
                        _sendScoreText.text = s;
                    }));
                }
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("Main");
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }

    private void checkMatch()
    {
        if (PlayerMixer.Instance.CurrentColors == GameColors.None || GameMixer.Instance.CurrentColors == GameColors.None)
            return;
        if (PlayerMixer.Instance.CurrentColors != GameMixer.Instance.CurrentColors)
            return;

        nextColor();
    }

    private IEnumerator Countdown()
    {
        float delay = 0.5f;

        _mainText.text = "3";
        yield return new WaitForSeconds(delay);
        _mainText.text = "2";
        yield return new WaitForSeconds(delay);
        _mainText.text = "1";
        yield return new WaitForSeconds(delay);
        _mainText.text = "GO";
        nextColor();
        State = GameLogicState.Play;
        yield return new WaitForSeconds(delay);
        _mainText.text = "";
    }

    private void nextColor()
    {
        _counter++;

        if (Settings.Mode == GameModes.Marathon)
        {
            float marathonTime = getMarathonTime();

            _maxTime = marathonTime;
            _time = marathonTime;
        }

        _score += (int)_colorPoints;
        _scoreText.text = _score.ToString();
        _colorPoints = 100;
        GameMixer.Instance.NextColor();
        MixerTrigger.MixerTriggers.ForEach(t => t.Reset());
    }

    private float getMarathonTime()
    {
        if (_counter < 5)
            return 10;

        if (_counter < 10)
            return 5;

        if (_counter < 25)
            return 3;

        if (_counter < 50)
            return 2;

        if (_counter < 100)
            return 1;

        return 0.5f;
    }
}