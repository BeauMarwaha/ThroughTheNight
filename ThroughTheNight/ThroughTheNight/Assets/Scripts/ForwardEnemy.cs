using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardEnemy :  Entity {
	public CollisionHandler ch;
	private SteeringForces steering;
	private Vector3 force;
	public Vector3 dir;

	// Use this for initialization
	protected override void Start () {
		steering = GetComponent<SteeringForces> ();
		ch = GameObject.Find ("GameManager").GetComponent<CollisionHandler> ();
		speed = 50f;
		attack = 1;
		health = 5f;
		direction = transform.forward;
		velocity = new Vector3(0,0,0);
	}

	// Update is called once per frame
	protected override void Update () {
        if (GameManager.GM.currentState != State.Message)
        {
            Death();
            Move();
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            TakeDamage(1);
            Attack();
        }
	}

	public override void Spawn(Vector3 location, Vector3 rotation){

	}


	//method to spawn entity into the game
	public GameObject Spawn(GameObject prefab, Vector3 location, Vector3 rotation){
		return (GameObject)Instantiate (prefab, location, Quaternion.Euler(rotation));
	}

	//method to move the entity
	protected override void Move(){
		if (Vector3.Dot (steering.player.transform.position, transform.position) > 0) {
			force += steering.Arrival (steering.player.transform.position , velocity, speed) * 300f;
		} else {
			force += steering.Arrival (steering.player.transform.position, velocity, speed) * 300f;
		}
		//if (steering.player.activeInHierarchy == false) return;
		Debug.Log (Vector3.Dot (steering.player.transform.right, transform.position) );
		force = Vector3.ClampMagnitude (force, 100f);
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
		// check for collision between player bullet and game object
		GameObject[] pBullets = GameObject.FindGameObjectsWithTag("pBullet");
		if (pBullets.Length == 0) return;
		for (int i = 0; i < pBullets.Length; i++) {
			if (ch.AABBCollision (gameObject, pBullets [i])) {
				health -= damageTaken;
			}
		}
	}

	//method to handle when the entity attacks
	protected override void Attack(){

	}

	public float GetAttack(){
		return attack;
	}
}