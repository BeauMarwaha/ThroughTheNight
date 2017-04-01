using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestAccess : MonoBehaviour {

    //private Text health;

    //ChangeScene script;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            GameManager.GM.healthNum--;
            GameManager.GM.health.text = "Health: " + GameManager.GM.healthNum;

        }
	}
}
