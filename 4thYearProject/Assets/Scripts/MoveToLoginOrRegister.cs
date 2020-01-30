using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLoginOrRegister : Photon.MonoBehaviour {

    public GameObject MainMenuCan;
    public GameObject LoginCan;
    public GameObject RegisterCan;

    public GameObject menuCan;
    public GameObject gameCan;
    public GameObject userCan;

    public GameObject photoManager;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (photoHandler.instance == null)
            Instantiate(photoManager);
    }

    public void LoadLogin()
    {
        MainMenuCan.SetActive(false);
        LoginCan.SetActive(true);
        RegisterCan.SetActive(false);
    }

    public void LoadRegister()
    {
        MainMenuCan.SetActive(false);
        RegisterCan.SetActive(true);
        LoginCan.SetActive(false);
    }

    public void Back()
    {
        MainMenuCan.SetActive(true);
        RegisterCan.SetActive(false);
        LoginCan.SetActive(false);
    }

    public void EnableGame()
    {
        gameCan.SetActive(true);
        menuCan.SetActive(false);
        userCan.SetActive(false);
    }

    public void EnableUser()
    {
        gameCan.SetActive(false);
        menuCan.SetActive(false);
        userCan.SetActive(true);
    }

    public void GoBackToMenu()
    {
        gameCan.SetActive(false);
        menuCan.SetActive(true);
        userCan.SetActive(false);
    }

}
