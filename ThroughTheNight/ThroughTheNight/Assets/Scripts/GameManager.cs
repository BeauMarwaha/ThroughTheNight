﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {


    public static GameManager GM;

    public Text health;
    public int healthNum;

    public Text time;
    public double timeNum;

    void Awake()
    {
        CreateGameManager();
    }


    void CreateGameManager()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else
        {
            if (GM != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Use this for initialization
    void Start () {
        health = GameObject.Find("Health").GetComponent<Text>();
        health.text = "Health: " + healthNum;

        time = GameObject.Find("Time").GetComponent<Text>();
        time.text = "Time: " + timeNum;
    }
	
	// Update is called once per frame
	void Update () {
        timeNum -= Time.deltaTime;
        time.text = "Time: " + timeNum.ToString("F2");
    }
}
