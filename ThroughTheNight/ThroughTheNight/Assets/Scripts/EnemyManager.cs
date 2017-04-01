using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public int enemiesLeft;
	public GameObject flyingPrefab;
	public GameObject forwardPrefab;
	public GameObject tankPrefeab;
	//private List<GameObject> enemiesList;
	private GameObject[] enemiesArray;
	private int totalEnemies;
	private int roomEnemies;

	// Use this for initialization
	void Start () {
		totalEnemies = 5;
		SpawnRoom ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// method to spawn enemies for a room
	public void SpawnRoom(){
		for (int i = 0; i < totalEnemies; i++) {
			GameObject e = (GameObject)Instantiate (tankPrefeab, new Vector3 (Random.Range (5, 10f), -3, 0), Quaternion.identity);
		}
	}

	// check if a room has been cleared
	public bool RoomClear(){
		if (GameObject.FindGameObjectsWithTag ("Enemy").Length == 0) return true;
		return false;
	}

	public bool LevelClear(){
		if (totalEnemies == 0) return true;
		return false;
	}
}
