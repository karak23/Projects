using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeDamage : MonoBehaviour {

    public float explosionRadius = 15f;
    public ParticleSystem ps;
    public GameObject player;

    void Start()
    {
        DealDamage();
    }

    private void Update()
    {
        if (ps)
        {
            if(!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }

    void DealDamage()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider col in coll)
        {
            if(col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerHealth>())
            {
                col.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, 10f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
