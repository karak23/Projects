using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour {

    public float health = 50f;
    private float maxHealth = 50f;
    public PhotonView photonView;
    public Image healthBar;

    // Use this for initialization
    [PunRPC]
	public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / 50;
        if(health <= 0f && photonView.isMine)
        {
            GameSetup.GS.isReadyToRespawn = true;
            foreach(PhotonPlayer p in PhotonNetwork.playerList)
            {
               if(!p.IsLocal)
                {
                    p.AddScore(1);
                }
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void AddHealth(float amount)
    {
        if(health != maxHealth)
        {
            health += amount;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }
        healthBar.fillAmount = health / 50;
    }
}
