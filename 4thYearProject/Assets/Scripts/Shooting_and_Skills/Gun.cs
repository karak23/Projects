using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject bulletSpawn;
    public float range = 100f;
    public ParticleSystem muzzleFlash;
    public GameObject bulletTrailPrefab;

    // Use this for initialization
    void Start () {
		
	}

    private void Update()
    {
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        Effect();
    }

    void Effect()
    {
       GameObject bullet =  PhotonNetwork.Instantiate(bulletTrailPrefab.name, bulletSpawn.transform.position, bulletSpawn.transform.rotation, 0) as GameObject;
    }
}
