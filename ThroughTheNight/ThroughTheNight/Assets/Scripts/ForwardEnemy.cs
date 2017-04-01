using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardEnemy :  Entity {

	public SteeringForces forces;

	// Use this for initialization
	protected override void Start () {
		forces = GetComponent<SteeringForces> ();
		speed = 30f;
		attack = 5f;
		health = 25f;
		direction = transform.forward;
		velocity = new Vector3(0,0,0);
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
	}

	//method to move the entity
	protected override void Move(){
		forces.Seek (velocity, speed);
		forces.ApplyForce ();
		forces.UpdatePosition (velocity, direction);
		forces.SetTransform (direction);
	}

	//method to handle when the entity dies
	protected override void Death(){

	}

	//method to handle when the entity is attacked
	protected override void Damaged(){

	}

	//method to handle when the entity attacks
	protected override void Attack(){

	}
}