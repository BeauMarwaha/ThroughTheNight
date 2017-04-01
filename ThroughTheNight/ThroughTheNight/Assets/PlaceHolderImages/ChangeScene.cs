using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Loads the UI and doesn't allow it to be destroyed
/// </summary>
public class ChangeScene : MonoBehaviour {

    //public Text health;
    //public int healthNum;

    
    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(GameObject.Find("Player"));

    }
	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("Bedroom");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            //GameManager.GM.healthNum++;
            //GameManager.GM.health.text = "Health: " + GameManager.GM.healthNum;
            GameManager.GM.ChangeHealth(1);
           
        }
	}
}
