using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public GameObject parent;
    private Vector3 velocity;
    private Vector3 direction;

	// Use this for initialization
	void Start ()
    {
        direction = parent.transform.right;//set the direction vector of the bullet to the parent shooter's right vector
        //this is essentially its forward in 2D in this case
        velocity = direction * speed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = transform.position + velocity * Time.deltaTime;
	}

    void Hit()
    {
        Destroy(this);
    }
}
