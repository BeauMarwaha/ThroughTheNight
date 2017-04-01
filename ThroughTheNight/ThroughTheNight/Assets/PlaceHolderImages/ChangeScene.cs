using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {


    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }
	// Use this for initialization
	void Start () {
        SceneManager.LoadSceneAsync(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
