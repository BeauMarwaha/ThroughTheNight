using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Title Screen button controls
/// </summary>
public class TitleScreen : MonoBehaviour {

    public Button play;
    public Button credits;
    public Button extras;

    //Funny extra
    public Image catsby;

	// Use this for initialization
	void Start ()
    {
		play.onClick.AddListener(LoadGame);
        credits.onClick.AddListener(LoadCredits);
        extras.onClick.AddListener(LoadExtra);

        catsby.enabled = false;
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
    void LoadExtra()
    {
        if (catsby.enabled)
        {
            catsby.enabled = false;
        }
        else
        {
            catsby.enabled = true;
        }
        
        
    }
}
