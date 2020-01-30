using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationScript : MonoBehaviour {

    public InputField usernameField;
    public InputField passwordField;
    public InputField confirmPasswordField;
    public InputField emailField;

    public Button submitButton;

    public void CallRegister()
    {
        if (string.Equals(passwordField.text, confirmPasswordField.text))
        {
            StartCoroutine(Register());
        }
        else
        {
            Debug.Log("Passwords are not matching");
        }
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_name", usernameField.text);
        form.AddField("password", passwordField.text);
        form.AddField("email", emailField.text);

        WWW www = new WWW("http://192.168.1.6/sqlconnect/register.php", form);
        yield return www;

        if(www.text == "0")
        {
            Debug.Log("User created Succesfully.");
        }
        else
        {
            Debug.Log("User creation failed. Error #" + www.text);
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (usernameField.text.Length >= 5 && passwordField.text.Length >= 8 && confirmPasswordField.text.Length >= 8);
    }

}
