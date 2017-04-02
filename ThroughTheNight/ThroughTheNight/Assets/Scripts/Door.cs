using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Beau Marwaha
/// Represents a Door Object
/// </summary>
public class Door : MonoBehaviour {

    //Attributes
    public string connectedRoom = "Empty";
    private GameObject player;
    private CollisionHandler collisionHandler;

    //Child Info Text Objects
    public GameObject openText;
    public GameObject blockedText;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        collisionHandler = GameObject.Find("GameManager").GetComponent<CollisionHandler>();

        //Update info text
        openText.GetComponent<TextMesh>().text += connectedRoom;
        blockedText.GetComponent<TextMesh>().text += connectedRoom;

        //hide all info text to start
        openText.GetComponent<MeshRenderer>().enabled = false;
        blockedText.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        //Handle if the player is approaching the door
        PlayerApproach();
	}

    //Handles when the player approaches a door
    private void PlayerApproach()
    {
        //check for collision between the player and the door
        if (collisionHandler.AABBCollision(player, gameObject))
        {
            //check to see if there are any enemies left in the game
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                //if there are no enemies left display bright door prompt
                openText.GetComponent<MeshRenderer>().enabled = true;
                blockedText.GetComponent<MeshRenderer>().enabled = false;

                //Handle player entering the door
                EnterDoorCheck();
            }
            else
            {
                //if there are enemies left display blocked door prompt
                openText.GetComponent<MeshRenderer>().enabled = false;
                blockedText.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            //if no collision hide all info text
            openText.GetComponent<MeshRenderer>().enabled = false;
            blockedText.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    /// <summary>
    /// Handle player trying to enter the door
    /// </summary>
    private void EnterDoorCheck()
    {
        //Interacts with door key
        if (Input.GetKey(KeyCode.E) && GameManager.GM.currentState != State.Message)
        {
            GameManager.GM.roomNames.Add(SceneManager.GetActiveScene().name);
            if(gameObject.tag == "LeftDoor")
            {
                player.transform.position = new Vector3(gameObject.transform.position.x + 16.5f , player.transform.position.y, player.transform.position.z);
            }
            else if(gameObject.tag == "RightDoor")
            {
                player.transform.position = new Vector3(gameObject.transform.position.x - 16.5f, player.transform.position.y, player.transform.position.z);

            }
            
            SceneManager.LoadScene(connectedRoom);
            
            //GameManager.GM.ClearRoom();
            
        }
    }
}
