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

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        collisionHandler = GameObject.Find("Canvas").GetComponent<CollisionHandler>();
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

        }
    }
}
