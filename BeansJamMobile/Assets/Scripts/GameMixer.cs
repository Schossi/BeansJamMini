using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMixer : MonoBehaviour {

    private static GameMixer _instance;
    public static GameMixer Instance
    {
        get
        {
            return _instance;
        }
    }

    private GameColors _currentColors = GameColors.None;
    public GameColors CurrentColors
    {
        get
        {
            return _currentColors;
        }
    }

    private Image _image;
    private Animator _animator;

    public GameMixer()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextColor()
    {
        GameColors nextColors = _currentColors;
        while(nextColors==_currentColors)
            nextColors = (GameColors)Random.Range(1, 8);
        setColor(nextColors);
    }

    public void ClearColor()
    {
        setColor(GameColors.None);
    }

    private void setColor(GameColors color)
    {
        _currentColors = color;
        _animator.SetTrigger("change");
    }

    public void ChangeMid()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(PlayerMixer.GetHexColor(_currentColors), out color))
            _image.color = color;
    }
}
