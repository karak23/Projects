using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 10f;
    public float damage = 1f;
    private float range = 0.3f;
    RaycastHit hit;

    // Use this for initialization


    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Debug.DrawRay(transform.position, transform.forward * range, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.tag == "Player")
            {
                hit.transform.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, damage);
            }
            Destroy(gameObject);
        }

        Destroy(gameObject, 6);
    }

    public void AddScore()
    {
        PhotonNetwork.player.AddScore(1);
    }
}
