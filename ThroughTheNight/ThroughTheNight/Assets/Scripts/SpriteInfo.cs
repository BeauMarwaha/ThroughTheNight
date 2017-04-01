using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Beau Marwaha
/// Contatins methods that retrieve sprite info
/// </summary>
public class SpriteInfo : MonoBehaviour {
    //gets the minimum x of the sprite object
    public float GetMinX()
    {
        return this.GetComponent<SpriteRenderer>().bounds.min.x;// * this.transform.localScale.x;
    }

    //gets the maximum x of the sprite object
    public float GetMaxX()
    {
        return this.GetComponent<SpriteRenderer>().bounds.max.x; // * this.transform.localScale.x;
    }

    //gets the minimum y of the sprite object
    public float GetMinY()
    {
        return this.GetComponent<SpriteRenderer>().bounds.min.y;// * this.transform.localScale.y;
    }

    //gets the maximum y of the sprite object
    public float GetMaxY()
    {
        return this.GetComponent<SpriteRenderer>().bounds.max.y;// * this.transform.localScale.y;
    }
}
