using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForces : MonoBehaviour {

	// needed for referencing the player location
	public GameObject player;

	// vectors needed for movement 
	protected Vector3 acceleration;
	protected Vector3 position;
	protected Vector3 desired;
	protected Vector3 steer;
	public float distToPlayer;

	// Use this for initialization
	void Start () {
		acceleration = position = desired = steer = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// update the position of the enemy
	public void UpdatePosition(Vector3 velocity, Vector3 direction){
		// grab the world positon from the transform 
		position = gameObject.transform.position;

		// add accel * dt to velocity
		velocity += acceleration * Time.deltaTime;

		// add velocity * dt to position
		position += velocity * Time.deltaTime;

		// make sure the enemy is always facing the player
		direction = -player.transform.forward;

		// start fresh with new forces each frame
		acceleration = Vector3.zero;

		// calculate distance to player
		distToPlayer =  Vector3.Distance(player.transform.position, position);
	}

	// set the transform component to reflect the local position vector
	public void SetTransform(Vector3 direction){
		gameObject.transform.position = position;
		transform.forward = direction;
	}

	// adds to the acceleration
	public void ApplyForce(){
		acceleration += steer;
	}


	public void Seek(Vector3 velocity, float speed){
		// find the vector pointing from the enemy to the player
		desired = player.transform.position - position;

		// scale the magnitute by speed to move relative to player type
		desired = desired.normalized * speed;

		// find the steering force
		steer = desired - velocity;
	}
}
