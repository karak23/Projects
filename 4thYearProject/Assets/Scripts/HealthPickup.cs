using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    GameObject helthSpawn;

    private void Start()
    {
        helthSpawn =  GameObject.FindGameObjectWithTag("HealthSpawn");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "EnemyPlayer")
        {
            other.gameObject.GetComponent<PhotonView>().RPC("AddHealth", PhotonTargets.All, 10f);
            helthSpawn.GetComponent<HealthSpawn>().spawned = false;
        }
        Destroy(gameObject);
    }
}
