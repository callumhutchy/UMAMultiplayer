using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{

    public InputField emailInput;
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField passwordConfirmInput;

    public GameObject emailValidTick;
    public GameObject usernameValidTick;
    public GameObject passwordValidTick;
    public GameObject passwordConfirmValidTick;
    public GameObject emailValidCross;
    public GameObject usernameValidCross;
    public GameObject passwordValidCross;
    public GameObject passwordConfirmValidCross;

    public MainMenuManager mmm;

    public bool emailExists = false;
    public bool usernameExists = false;

    void Update()
    {
        if (emailInput.text != "")
        {
            if (!emailExists)
            {
                emailValidTick.SetActive(true);
                emailValidCross.SetActive(false);
            }
            else
            {
                emailValidTick.SetActive(false);
                emailValidCross.SetActive(true);
            }
        }
        if (usernameInput.text != "")
        {
            if (!usernameExists)
            {
                usernameValidTick.SetActive(true);
                usernameValidCross.SetActive(false);
            }
            else
            {
                usernameValidCross.SetActive(true);
                usernameValidTick.SetActive(false);
            }
        }
        if (passwordInput.text != "" && passwordConfirmInput.text != "")
        {
            if (passwordInput.text == passwordConfirmInput.text)
            {
                passwordConfirmValidTick.SetActive(true);
                passwordConfirmValidCross.SetActive(false);
            }
            else
            {

                passwordConfirmValidTick.SetActive(false);
                passwordConfirmValidCross.SetActive(true);
            }
        }
    }

    public void OnRegisterClick()
    {
        if(!emailExists && !usernameExists && passwordsMatch())
        {
            StartCoroutine(RegisterUserInDatabase());
        }
       
        
    }

    public void OnBackClick()
    {
        mmm.ShowMainMenu();
    }

    public void checkEmail()
    {
        StartCoroutine(CheckEmailInDatabase());
    }

    public void checkUsername()
    {
        StartCoroutine(CheckUsernameInDatabase());
    }

    bool passwordsMatch()
    {
        if (passwordInput.text != "" && passwordConfirmInput.text != "")
        {
            if (passwordInput.text == passwordConfirmInput.text)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    IEnumerator RegisterUserInDatabase()
    {
        WWWForm logform = new WWWForm();
        logform.AddField("email", emailInput.text);
        logform.AddField("user", usernameInput.text);
        logform.AddField("password", md5(passwordConfirmInput.text));
        WWW logw = new WWW("hutchygameserver.ddns.net/RegisterUser.php?", logform);
        yield return logw;
        Debug.Log(logw.text);
        mmm.ShowMainMenu();
    }


    IEnumerator CheckEmailInDatabase()
    {
        WWWForm logform = new WWWForm();
        logform.AddField("email", emailInput.text);
        WWW logw = new WWW("hutchygameserver.ddns.net/CheckEmail.php?", logform);
        yield return logw;
        Debug.Log(logw.text);
        if (logw.text == "emailexists")
        {
            emailExists = true;
        }
        else
        {
            emailExists = false;
        }

    }

    IEnumerator CheckUsernameInDatabase()
    {
        WWWForm logform = new WWWForm();
        logform.AddField("user", usernameInput.text);
        WWW logw = new WWW("hutchygameserver.ddns.net/CheckUsername.php?", logform);
        yield return logw;
        Debug.Log(logw.text);
        if (logw.text == "userexists")
        {
            usernameExists = true;
        }
        else
        {
            usernameExists = false;
        }
    }

    private string md5(string pass)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        return s.ToString();

    }

}
