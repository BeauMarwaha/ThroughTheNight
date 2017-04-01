using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the credits menu in the game
/// </summary>
public class Credits : MonoBehaviour {

    public Button credits;
	// Use this for initialization
	void Start () {
        credits.onClick.AddListener(LoadTitle);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadTitle()
    {
        SceneManager.LoadScene(0);
    }
}
