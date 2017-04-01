using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Entity {
	
	// Use this for initialization
	protected override void Start () {
		speed;
		attack;
		health;
		velocity;
		direction;	
	}

	// Update is called once per frame
	protected void Update () {

	}

	//method to spawn entity into the game
	protected override void Spawn(Vector3 location, Vector3 rotation){
	}

	//method to move the entity
	protected override void Move(){

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