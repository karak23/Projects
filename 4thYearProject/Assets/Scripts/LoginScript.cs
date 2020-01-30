using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoginScript : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public photonConnect pho;
    public bool noConnection;

    public Button submitButton;

    public void CallLogin()
    {
        if (noConnection)
        {
            NoLogin();
        }
        else
        {
            StartCoroutine(Login());
        }
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", usernameField.text);
        form.AddField("password", passwordField.text);

        WWW www = new WWW("http://192.168.1.6/sqlconnect/login.php", form);
        yield return www;

        if(www.text[0] == '0')
        {
            Debug.Log("Login Succesfull");
            photoHandler.instance.connectToPhoton();
            PhotonNetwork.playerName = usernameField.text;
        }
        else
        {
            Debug.Log("Login Failed: " + www.text);
        }

    }

    public void NoLogin()
    {
        Debug.Log("Login Succesfull");
        photoHandler.instance.connectToPhoton();
        PhotonNetwork.playerName = usernameField.text;
    }
}
