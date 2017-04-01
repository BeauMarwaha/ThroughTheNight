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

        //Keep menu audio manager between all menus
        DontDestroyOnLoad(GameObject.Find("Audio Manager"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadGame()
    {
        SceneManager.LoadScene(1);

        //Destroy menu audio manager when starting game
        Destroy(GameObject.Find("Audio Manager"));
    }

    void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
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
