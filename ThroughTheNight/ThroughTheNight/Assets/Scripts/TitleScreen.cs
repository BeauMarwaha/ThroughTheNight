using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleScreen : MonoBehaviour {

    public Button play;
    public Button credits;

	// Use this for initialization
	void Start ()
    {
		play.onClick.AddListener(LoadGame);
        credits.onClick.AddListener(LoadCredits);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    void LoadCredits()
    {
        SceneManager.LoadScene(3);
    }
}
