using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Author(s): Beau Marwaha
/// Handles end game screens
/// </summary>
public class GameOverScreen : MonoBehaviour {

    //attributes
    public Button exitButtton;
    public Button playAgainButtton;

    // Use this for initialization
    void Start () {
        exitButtton.onClick.AddListener(Exit);
        playAgainButtton.onClick.AddListener(PlayAgain);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Exit the game
    private void Exit()
    {
        Application.Quit();
    }

    //Play the game again
    private void PlayAgain()
    {
        //Destroy menu audio manager when starting game
        Destroy(GameObject.Find("Audio Manager"));

        //Restart game
        SceneManager.LoadScene(1);
    }
}
