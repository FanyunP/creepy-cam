using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenSize : MonoBehaviour {
    public int width = 704;
    public int height = 528;

	// Use this for initialization
	void Start () {
		//Screen.SetResolution(width, height, true);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape)) {
            Screen.fullScreen = false;
            
        }
        if (Input.GetKey(KeyCode.K)) {
            Screen.fullScreen = true;
        }
        if (Input.GetKey(KeyCode.F)) {
            Screen.SetResolution(width, height, true);
        }
       
    }

    //private void FixedUpdate()
    //{
    //    Screen.SetResolution(10000,2500,true);
    //}
}
