using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{

    private int _score = 0;
    private float _colorPoints = 0;

    private Text _mainText;
    private Text _scoreText;

    // Use this for initialization
    void Start()
    {
        _mainText = transform.Find("MainText").GetComponent<Text>();
        _scoreText = transform.Find("ScoreText").GetComponent<Text>();

        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        checkMatch();

        _colorPoints = Mathf.Max(_colorPoints - Time.deltaTime * 50, 10);
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
        yield return new WaitForSeconds(delay);
        _mainText.text = "";
    }

    private void nextColor()
    {
        _score += (int)_colorPoints;
        _scoreText.text = _score.ToString();
        _colorPoints = 100;
        GameMixer.Instance.NextColor();
        MixerTrigger.MixerTriggers.ForEach(t => t.Reset());
    }
}
