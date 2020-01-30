using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour {

    public Rigidbody rig;
    private Transform target;
    public float force;
    public GameObject granadeExplosion;
    public GameObject player;
    public float distance;
    public string user;
    public int userID;


    // Use this for initialization
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("EnemyPlayer").transform;
        ThrowBallAtTargetLocation(target.position, force);
    }

    // Throws ball at location with regards to gravity (assuming no obstacles in path) and initialVelocity (how hard to throw the ball)
    public void ThrowBallAtTargetLocation(Vector3 targetLocation, float initialVelocity)
    {
        if (target != null)
        {
            Vector3 direction = (targetLocation - transform.position).normalized;
            distance = Vector3.Distance(targetLocation, transform.position);
            if (distance < 2.5)
            {
                float firingElevationAngle = FiringElevationAngle(Physics.gravity.magnitude, distance, initialVelocity);
                Vector3 elevation = Quaternion.AngleAxis(firingElevationAngle, transform.right) * transform.up;
                float directionAngle = AngleBetweenAboutAxis(transform.forward, direction, transform.up);
                Vector3 velocity = Quaternion.AngleAxis(directionAngle, transform.up) * elevation * initialVelocity;

                // ballGameObject is object to be thrown
                rig.AddForce(velocity, ForceMode.VelocityChange);
            }
        }
    }

    // Helper method to find angle between two points (v1 & v2) with respect to axis n
    public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    // Helper method to find angle of elevation (ballistic trajectory) required to reach distance with initialVelocity
    // Does not take wind resistance into consideration.
    private float FiringElevationAngle(float gravity, float distance, float initialVelocity)
    {
        float angle = 0.5f * Mathf.Asin((gravity * distance) / (initialVelocity * initialVelocity)) * Mathf.Rad2Deg;
        return angle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
            GameObject explosion = Instantiate(granadeExplosion, transform.position, granadeExplosion.transform.rotation);
            explosion.GetComponent<GranadeDamage>().player = player;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(rig.position);
            stream.SendNext(rig.velocity);
            stream.SendNext(rig.rotation);
        }
        else
        {
            rig.position = (Vector3)stream.ReceiveNext();
            rig.velocity = (Vector3)stream.ReceiveNext();
            rig.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
