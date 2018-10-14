using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMixer : MonoBehaviour
{
    private static PlayerMixer _instance;
    public static PlayerMixer Instance
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

    public PlayerMixer()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {
        _image = GetComponent<Image>();
        onCurrentColorsChanged();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddColor(GameColors color)
    {
        _currentColors = _currentColors | color;
        onCurrentColorsChanged();
    }

    public void RemoveColor(GameColors color)
    {
        _currentColors = _currentColors & ~color;
        onCurrentColorsChanged();
    }

    private void onCurrentColorsChanged()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(GetHexColor(_currentColors), out color))
            _image.color = color;
    }

    public static string GetHexColor(GameColors colors)
    {
        switch (colors)
        {
            case GameColors.None:
                return "#00000000";
            case GameColors.White:
                return "#FFFFFF";
            case GameColors.Blue:
                return "#0047ab";
            case GameColors.Black:
                return "#000000";
            case GameColors.White | GameColors.Blue:
                return "#80A3D5";
            case GameColors.Blue | GameColors.Black:
                return "#002456";
            case GameColors.White | GameColors.Black:
                return "#808080";
            case GameColors.White | GameColors.Blue | GameColors.Black:
                return "#556D8E";
            default:
                return "#00000000";
        }
    }
}
