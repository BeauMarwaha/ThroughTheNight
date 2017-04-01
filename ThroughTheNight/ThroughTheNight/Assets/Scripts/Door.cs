using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Beau Marwaha
/// Represents a Door Object
/// </summary>
public class Door : MonoBehaviour {

    //Attributes
    private string connectedRoom = "Empty";
    private GameObject player;
    private CollisionHandler collisionHandler;

    //Child Info Text Objects
    public GameObject openText;
    public GameObject blockedText;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        collisionHandler = GameObject.Find("GameManager").GetComponent<CollisionHandler>();

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
}
