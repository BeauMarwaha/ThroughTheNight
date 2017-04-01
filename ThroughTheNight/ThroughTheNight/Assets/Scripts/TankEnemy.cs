using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Entity {
	//public GameObject tankPrefab;
	public SteeringForces forces;

	// Use this for initialization
	protected override void Start () {
		forces = GetComponent<SteeringForces> ();
		speed = 10f;
		attack = 10f;
		health = 50f;
		direction = transform.forward;
		velocity = new Vector3(0,0,0);
		//Spawn (new Vector3(Random.Range(5,10),0,1),-direction);
	}

	// Update is called once per frame
	protected override void Update () {
		Death ();
		Move ();
		Damaged ();
		Attack ();
	}

	//method to spawn entity into the game
	protected override void Spawn(Vector3 location, Vector3 rotation){
		//GameObject obj = (GameObject)Instantiate (tankPrefab, location, Quaternion.Euler(rotation));
	}

	//method to move the entity
	protected override void Move(){
		if (forces.DistToPlayer() > 5f) {
			forces.Seek (velocity, speed);
			forces.ApplyForce ();
			forces.UpdatePosition (velocity, direction);
			forces.SetTransform (direction);
		}
	}

	//method to handle when the entity dies
	protected override void Death(){
		// destroy the game object 
		if (health <= 0) {
			// TO-DO: increment player currency


			// destroy enemy object
			Destroy (gameObject);
		}
	}

	//method to handle when the entity is attacked
	protected override void Damaged(){
		// TO-DO
		// check for collision between player bullet and game object

		// handle collison if so
	}

	//method to handle when the entity attacks
	protected override void Attack(){
		//TO-DO
		// create bullet

		// move bullet in the x direction only
	}
}