using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Manager Singleton
/// Allows the access of variables at all times no matter the current scene
/// </summary>
public enum State { Day, Buy, Night, Message, Over, Secret }
public class GameManager : MonoBehaviour {


    private List<string> messages = new List<string>();
    private int currentMessage = 0;
    private string winMessage = "You cleared all of the Inanis, I suppose you are suitable to be my replacement.";
    private string lossMessage = "If that's what you call trying I guess it's a good thing you are never waking up.";
    private string secretMessage = "I told you to NOT go back to sleep.";

    public AudioClip[] audioClips;
    private AudioSource aSource;

    bool win;
    bool displayed = false;
    public List<string> roomNames = new List<string>();

    public static GameManager GM;
    


    public Image[] hearts;

    //public Text health;
    public int healthNum;

    public Text time;
    public double timeNum;

    public Text remaining;
    public int remainNum;

    public Text objective;

    public State currentState;

    public Text[] textElements;
    public Font[] fonts;

    //Currently Unused
    private List<string> objectives = new List<string>();
    private List<string> currentObjectives;
    private int objectivesTotal;

    public Image background;
    public Image largeCat;
    public Text message;
    public Image secretImage;

    void Awake()
    {
        CreateGameManager();
        aSource = GameObject.Find("Canvas").GetComponent<AudioSource>();
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

        HideMessage();
        PopulatesMessages();
        DisplayMessage();

        

        objective = GameObject.Find("Objectives").GetComponent<Text>();
        objectives = new List<string>();
        objectives.Add("Cook");
        objectives.Add("Clean");
        objectives.Add("Shower");
        
        
        //health = GameObject.Find("Health").GetComponent<Text>();
        //health.text = "Health: " + healthNum;

        time = GameObject.Find("Time").GetComponent<Text>();
        time.text = "Time: " + timeNum.ToString("F2");

        remaining = GameObject.Find("Remaining").GetComponent<Text>();
        remaining.text = remainNum.ToString();

        for(int i = 0; i < objectives.Count; i++)
        {
            objective.text += "     " + objectives[i] + "\n";
        }
        objectivesTotal = objectives.Count;

        currentObjectives = new List<string>(objectives);

        RemoveObjective("Cook");

        for(int i = 0; i <hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
        for (int i = 0; i < healthNum; i++)
        {
            hearts[i].enabled = true;
        }
        //Starting at Day
        ChangeState(State.Message);
        
    }
	
	// Update is called once per frame
	void Update () {
        
        if (currentState != State.Message && currentState != State.Over && currentState != State.Secret)
        {
            ClearRoom();
            if (currentState == State.Night)
            {
                timeNum -= Time.deltaTime;
                time.text = "Time: " + timeNum.ToString("F2");
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (currentState == State.Night)
                {
                    ChangeState(State.Day);
                }
                else if (currentState == State.Day)
                {
                    ChangeState(State.Night);
                }
            }
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    if (currentState == State.Night)
            //    {
            //        DisplayMessage();
            //    }
            //    else if (currentState == State.Message)
            //    {
            //        HideMessage();
            //    }
            //}
            if(healthNum == 0 || timeNum <= 0 )
            {

                //Move to end screen
                win = false;
                ChangeState(State.Over);
                //DisplayMessage();
            }
            if (remainNum == 0)
            {

                //Move to end screen
                win = true;
                ChangeState(State.Over);
                //DisplayMessage();
            }
            
        }
        else if(currentState == State.Message)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideMessage();
            }
            if (Input.GetMouseButtonDown(0) && messages.Count -1 != currentMessage)
            {
				aSource.Stop ();
                currentMessage++;
                message.text = messages[currentMessage];
                aSource.PlayOneShot(audioClips[currentMessage]);
            }
            else if(Input.GetMouseButtonDown(0) && messages.Count - 1 == 3)
            {
                HideMessage();
            }
        }else if (currentState == State.Secret)
        {
            if (!displayed)
            {
                DisplayMessage();
                currentState = State.Secret;
                displayed = true;
                message.text = secretMessage;
                largeCat.sprite = secretImage.sprite;
                aSource.PlayOneShot(audioClips[6]);
            }
            else if(Input.GetMouseButtonDown(0))
            {
                Destroy(GameObject.Find("Player"));
                Destroy(GameObject.Find("Canvas"));
                Destroy(GameObject.Find("GameManager"));
                SceneManager.LoadScene(11);
            }
        }
        else if(currentState == State.Over)
        {
            if (!displayed)
            {
                DisplayMessage();
                
                displayed = true;
                if (win)
                {
                    message.text = winMessage;
                    aSource.PlayOneShot(audioClips[4]);
                }
                else
                {
                    message.text = lossMessage;
                    aSource.PlayOneShot(audioClips[5]);
                }
            }else
            {
                    if (win)
                    {
                        //HideMessage();
                        Destroy(GameObject.Find("Player"));
                        Destroy(GameObject.Find("Canvas"));
                        Destroy(GameObject.Find("GameManager"));
                        SceneManager.LoadScene(12);
                    }
                    else
                    {
                        //HideMessage();
                        Destroy(GameObject.Find("Player"));
                        Destroy(GameObject.Find("Canvas"));
                        Destroy(GameObject.Find("GameManager"));
                        SceneManager.LoadScene(11);
                    }
            }                
                    
            
        }
        
    }

    /// <summary>
    /// Change the health of the player. Use negatives to decrease health
    /// </summary>
    /// <param name="health">Amount of health the player loses/gains </param>
    public void ChangeHealth(int health)
    {
        if(health == 0)//if input is zero set health to zero instead of changing it by zero
        {
            GameManager.GM.healthNum = health;
        }
        else
        {
            GameManager.GM.healthNum += health;
        }

        if(healthNum > 16)
        {
            healthNum = 16;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }
        for (int i = 0; i < healthNum; i++)
        {
            hearts[i].enabled = true;
        }
        //GameManager.GM.health.text = "Health: " + GameManager.GM.healthNum;
    }
    
    /// <summary>
    /// Called to decrease remaining enemy count once and enemy has been destroyed
    /// </summary>
    public void DefeatEnemy()
    {
        remainNum--;
        remaining.text = remainNum.ToString();

        //if enemy count runs out the player wins
        
    }


    /// <summary>
    /// Change the game between it's seperate states
    /// </summary>
    /// <param name="change">The state to change the game to</param>
    void ChangeState(State change)
    {
        currentState = change;
        switch (currentState)
        {
            case State.Day:
                { 
                    for(int i = 0; i < textElements.Length; i++)
                    {
                        textElements[i].GetComponent<Text>().font = fonts[1];
                    }                   
                    Camera.main.backgroundColor = new Color(155, 155, 155);
                    remaining.text = "0";
                    return;
                }
            case State.Night:
                {
                    for (int i = 0; i < textElements.Length; i++)
                    {
                        textElements[i].GetComponent<Text>().font = fonts[0];                        
                    }
                    Camera.main.backgroundColor = new Color(0, 0, 0);
                    remaining.text = remainNum.ToString();
                    return;
                }
            case State.Message:
                {
                    message.text = messages[currentMessage];
                    if(currentMessage == 0)
                    {
                        aSource.PlayOneShot(audioClips[0]);
                    }             
                    return;
                }
        }

    }

    /// <summary>
    /// Removes a completed objective from the current list of objectives
    /// </summary>
    /// <param name="obj"></param>
    void RemoveObjective(string obj)
    {
        if (currentObjectives.Contains(obj))
        {
            currentObjectives.Remove(obj);
            objective.text = "Objectives: \n";
            for (int i = 0; i < currentObjectives.Count; i++)
            {
                objective.text += "     " + objectives[i] + "\n";
            }
           
        }
    }

    void HideMessage()
    {
        background.enabled = false;
        largeCat.enabled = false;
        message.enabled = false;
        ChangeState(State.Night);
    }

    void DisplayMessage()
    {
        ChangeState(State.Message);
        background.enabled = true;
        largeCat.enabled = true;
        message.enabled = true;
        
    }

    void PopulatesMessages()
    {
        messages.Add("Welcome to your dream *cough* nightmare *cough*. I recommend you don't try going back to sleep.");
        messages.Add("I'm Harenae, the Inanis have invaded your dream. You see that number in the bottom left corner?");
        messages.Add("It's how many still remain in your dream. If any are left by morning...");
        messages.Add("You aren't waking up.");
        
        
    }

    public void ClearRoom()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        
        if (roomNames.Contains(SceneManager.GetActiveScene().name))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length;i++)
            {
                Destroy(enemies[i]);
            }
        }
        
    }
}
