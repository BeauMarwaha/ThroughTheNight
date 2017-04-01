using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Entity {
	public SteeringForces steering;
	public GameObject orb;
	private Vector3 force;
	private float period;

	// Use this for initialization
	protected override void Start () {
		steering = GetComponent<SteeringForces> ();
		speed = 10f;
		attack = 10;
		health = 50f;
		direction = transform.forward;
		velocity = new Vector3(0,0,0);
	}

	// Update is called once per frame
	protected override void Update () {
		Death ();
		Move ();
		//TakeDamage ();

		InvokeRepeating ("Attack", 1f, 1.5f);
	}

	public override void Spawn(Vector3 location, Vector3 rotation){
	
	}


	//method to spawn entity into the game
	public GameObject Spawn(GameObject prefab, Vector3 location, Vector3 rotation){
		return (GameObject)Instantiate (prefab, location, Quaternion.Euler(rotation));
	}

	//method to move the entity
	protected override void Move(){
		// move closer to the player up to a certain distance
		if (steering.DistToPlayer() > 10f) {
			steering.SeekPlayer (velocity, speed);
			force += Vector3.ClampMagnitude (force, 10f);
			steering.ApplyForce (force);
			steering.UpdatePosition (velocity, direction);
			steering.SetTransform (direction);
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
	public override void TakeDamage(int damageTaken){
		// TO-DO
		// check for collision between player bullet and game object

		// handle collison if so
	}

	//method to handle when the entity attacks
	protected override void Attack(){
		//TO-DO
		// create bullet
		GameObject bullet = (GameObject)Instantiate(orb, transform.position,Quaternion.identity);
		Debug.Log ("Attacking");
		// move bullet in the x direction only
	}

	public float GetAttack(){
		return attack;
	}
}