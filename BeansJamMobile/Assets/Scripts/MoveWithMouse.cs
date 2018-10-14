using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            pos.z = 1;
            transform.position = pos;
        }
    }
}
