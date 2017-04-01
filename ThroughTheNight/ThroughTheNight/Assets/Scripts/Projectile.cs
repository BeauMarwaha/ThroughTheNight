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
        //find velocity given direction and speed
        velocity = -transform.right * speed;
        velocity.z = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //update the position of the bullets 
        transform.position = transform.position + velocity * Time.deltaTime;
	}

    void Hit()
    {
        Destroy(this);
    }
}
