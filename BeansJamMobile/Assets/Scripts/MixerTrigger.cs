using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MixerTrigger : EventTrigger {

    public static List<MixerTrigger> MixerTriggers = new List<MixerTrigger>();

    private Animator _animator;

    private GameColors _color;
    private string _key;

    public MixerTrigger()
    {
        MixerTriggers.Add(this);
    }

    // Use this for initialization
    void Start ()
    {
        _animator = GetComponent<Animator>();

        if (gameObject.name.Contains("White"))
        {
            _color = GameColors.White;
            _key = "q";
        }
        else if(gameObject.name.Contains("Blue"))
        {
            _color = GameColors.Blue;
            _key = "w";
        } 
        else if (gameObject.name.Contains("Black"))
        {
            _color = GameColors.Black;
            _key = "e";
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(_key))
            onDown();
        if (Input.GetKeyUp(_key))
            onUp();
	}

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        
        onDown();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        onUp();
    }

    public void Reset()
    {
        onUp();
    }

    private void onDown()
    {
        PlayerMixer.Instance.AddColor(_color);

        _animator.SetBool("pressed", true);
    }

    private void onUp()
    {
        PlayerMixer.Instance.RemoveColor(_color);

        if (_animator != null)
            _animator.SetBool("pressed", false);
    }
}
