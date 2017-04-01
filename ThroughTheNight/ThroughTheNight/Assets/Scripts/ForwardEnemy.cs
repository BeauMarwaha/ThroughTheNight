using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardEnemy :  Entity {
	public SteeringForces steering;
	private Vector3 force;
	public Vector3 dir;

	// Use this for initialization
	protected override void Start () {
		steering = GetComponent<SteeringForces> ();
		speed = 50f;
		attack = 5f;
		health = 25f;
		direction = transform.forward;
		velocity = new Vector3(0,0,0);
	}

	// Update is called once per frame
	protected override void Update () {
		Death ();
		Move ();
		//TakeDamage ();
		Attack ();
	}

	public override void Spawn(Vector3 location, Vector3 rotation){

	}


	//method to spawn entity into the game
	public GameObject Spawn(GameObject prefab, Vector3 location, Vector3 rotation){
		return (GameObject)Instantiate (prefab, location, Quaternion.Euler(rotation));
	}

	//method to move the entity
	protected override void Move(){
		force += steering.SeekPlayer (velocity, speed) * 1500f;
		force = Vector3.ClampMagnitude (force, 950f);
		steering.ApplyForce (force);
		steering.UpdatePosition (velocity, direction);
		steering.SetTransform (direction);
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
	public override void TakeDamage(int damageTaken){
		// TO-DO
		// check for collision between player bullet and game object

		// handle collison if so
	}

	//method to handle when the entity attacks
	protected override void Attack(){

	}

	public float GetAttack(){
		return attack;
	}
}