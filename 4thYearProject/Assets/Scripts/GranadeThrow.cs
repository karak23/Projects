using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GranadeThrow : MonoBehaviour {

    private Button _button;
    private GameObject grenadeSkill;
    public GameObject granade;
    public Transform granadeSpawn;
    public PhotonView pv;
    public Image img;
    private bool isCooldown = false;
    private float cooldown = 5;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    void Start()
    {
        if (pv.isMine)
        {
            SetUpSkill();
        }
    }

    private void Update()
    {
        if(isCooldown)
        {
            img.fillAmount += 1 / cooldown * Time.deltaTime;
            if(img.fillAmount >= 1)
            {
                img.fillAmount = 0;
                _button.enabled = true;
                isCooldown = false;
            }
        }
    }

    void SpawnGranade()
    {
        isCooldown = true;
        GameObject granadeObj = PhotonNetwork.Instantiate(granade.name, granadeSpawn.position, granade.transform.rotation, 0);
        granadeObj.GetComponent<Granade>().player = gameObject;
        granadeObj.GetComponent<Granade>().userID = gameObject.GetPhotonView().ownerId;
        _button.enabled = false;
    }

    void SetUpSkill()
    {
        grenadeSkill = GameObject.Find("GranadeButton");
        _button = grenadeSkill.GetComponent<Button>();
        _button.onClick.AddListener(() => SpawnGranade());
        img = grenadeSkill.transform.Find("Image").GetComponent<Image>();
        _button.enabled = true;
        img.fillAmount = 0;
    }
}
