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
	public Vector3 circleCenter;
	public float distToPlayer;
	protected float step;

	// Use this for initialization
	void Start () {
		acceleration = position = desired = steer = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		player = GameObject.Find ("Player");
	}

	// calculate distance to player
	public float DistToPlayer(){
		return distToPlayer =  Vector3.Distance(player.transform.position, gameObject.transform.position);
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
		direction = player.transform.forward;

		// start fresh with new forces each frame
		acceleration = Vector3.zero;
	}

	// set the transform component to reflect the local position vector
	public void SetTransform(Vector3 direction){
		gameObject.transform.position = position;
		transform.forward = direction;
	}

	// adds to the acceleration
	public void ApplyForce(Vector3 force){
		acceleration += force;
	}

	// seek the player
	public Vector3 SeekPlayer(Vector3 velocity, float speed){
		// find the vector pointing from the enemy to the player
		desired = player.transform.position - position;

		// scale the magnitute by speed to move relative to enemy type
		desired = desired.normalized * speed;

		// find the steering force
		return steer = desired - velocity;
	}

	// seek a spot 
	public Vector3 SeekSpot(Vector3 target, Vector3 velocity, float speed){
		// find the vector pointing from the enemy to the player
		desired = target - position;

		// scale the magnitute by speed to move relative to enemy type
		desired = desired.normalized * speed;

		// find the steering force
		return steer = desired - velocity;
	}

	public Vector3 Arrival(Vector3 target, Vector3 velocity, float speed){
		return SeekSpot (target, velocity, speed) * Vector3.Distance(target, transform.position)/10f ;
	}

	// flee the player
	public Vector3 Flee(Vector3 velocity, float speed){
		return steer = -1 * SeekPlayer (velocity, speed);
	}

	// move enemy in a circular motion
	public Vector3 WanderCircle(Vector3 velocity, float speed){
		circleCenter = velocity.normalized * 100f;
		desired = new Vector3( -Mathf.Cos(Mathf.PI * step), Mathf.Sin (Mathf.PI * step), 0) * 50f + circleCenter;
		step += .05f;
		return steer = SeekSpot (desired, velocity, speed);
	}
}