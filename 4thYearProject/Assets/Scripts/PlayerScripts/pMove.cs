using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pMove : Photon.MonoBehaviour
{

    public PhotonView pV;
    public PhotonView nameTag;

    public float moveSpeed = 10f;
    public float inputHori;

    public float fireRate = 10f;
    public float nextTimeToFire = 0f;

    private Vector3 selfPos;

    private Rigidbody rb;

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;

    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Quaternion realRotation;

    private float inputVertical;
    public string verticalAxis = "Vertical";

    public Gun gun;
    public GranadeThrow granadeThrow;

    public Animator anim;

    public GameObject underline;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if(pV.isMine)
        {
            gun.enabled = true;
            granadeThrow.enabled = true;
            anim = GetComponent<Animator>();
            underline.SetActive(true);
        }
        else
        {
            gameObject.tag = "EnemyPlayer";
        }
        //nameTag.RPC("SetName", PhotonTargets.AllBuffered, PhotonNetwork.playerName);
    }

    private void Update()
    {
        if (pV.isMine)
        {
            checkInput();
        }
        else
            smoothNetMovement();
    }

    private void checkInput()
    {
        inputVertical = SimpleInput.GetAxis(verticalAxis);
        inputHori = SimpleInput.GetAxis("Horizontal");
        float input2Hori = SimpleInput.GetAxis("HorizontalJoy");
        float input2Vertical = SimpleInput.GetAxis("VerticalJoy");

        Move(inputHori, inputVertical);

        rb.velocity = new Vector3(inputHori* moveSpeed, rb.velocity.y, inputVertical* moveSpeed);
        Vector3 playerDirection = Vector3.right * inputHori + Vector3.forward * inputVertical;
        Vector3 aimingDirection = Vector3.right * input2Hori + Vector3.forward * input2Vertical;

        //Debug.Log(playerDirection.normalized);

        if (playerDirection.sqrMagnitude > 0 && aimingDirection.sqrMagnitude <= 0)
        {
            anim.SetBool("isWalking", true);
            rb.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }
        else if(aimingDirection.sqrMagnitude > 0)
        {
            rb.rotation = Quaternion.LookRotation(aimingDirection, Vector3.up);
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                gun.Shoot();
            }

            if(playerDirection.normalized.z <= -0.1)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isWalkingBack", true);
            }
            else if(playerDirection.normalized.z >= 0.1)
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isWalkingBack", false);
            }
            else
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isWalkingBack", false);
            }

        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isWalkingBack", false);
        }
    }

    private void smoothNetMovement()
    {
        syncTime += Time.deltaTime;
        rb.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        rb.rotation = Quaternion.Lerp(rb.rotation, realRotation, 0.1f);
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
            stream.SendNext(rb.rotation);
        }
        else
        {
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            Vector3 syncVelocity = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = syncPosition;
        }
    }

    private void Move(float x, float y)
    {
        anim.SetFloat("Horizontal_F", x);
        anim.SetFloat("Vertical_f", y);
    }
}
