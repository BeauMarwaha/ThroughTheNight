using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a projectile object
/// </summary>
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
        if(GameManager.GM.currentState != State.Message)
        {
            //update the position of the bullets 
            transform.position = transform.position + velocity * Time.deltaTime;
            //limit the lives of the bullets
            LimitLife();
        }
        
	}

    public void Hit()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Ensures that once a bullet leaves the screen it is destroyed
    /// </summary>
    /// <param name="bullet"></param>
    public void LimitLife()
    {
        //find projectiles position in the viewport as opposed to in the world
        Camera cam = Camera.main;
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);
        //destroy bullet if it is outside the viewport
        if (viewportPos.x > 1 || viewportPos.x < 0 || viewportPos.y > 1.5 || viewportPos.y < -.5)
        {
            Destroy(gameObject);
        }
    }
}
