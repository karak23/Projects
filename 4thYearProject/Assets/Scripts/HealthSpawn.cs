using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawn : MonoBehaviour {

    public GameObject health;
    public Transform healthSpawn;
    public bool spawned = false;
    float timeToSpawn = 10f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!spawned)
        {
            timeToSpawn -= Time.deltaTime;
            if(timeToSpawn <= 0)
            {
                PhotonNetwork.Instantiate(health.name, healthSpawn.transform.position, health.transform.rotation, 0);
                spawned = true;
                timeToSpawn = 10f;
            }
        }
	}
}
