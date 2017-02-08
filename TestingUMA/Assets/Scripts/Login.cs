using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

    public InputField usernameInput;
    public InputField passwordInput;
    public UserStats user;
    
    public MainMenuManager mmm;

    public GameObject validateScreen;

    private bool login;

    public void OnLoginClick()
    {
        StartCoroutine(LoginCheck());
    }

    IEnumerator LoginCheck()
    {
        WWWForm logform = new WWWForm();
        logform.AddField("user", usernameInput.text);
        logform.AddField("password", md5(passwordInput.text));
        WWW logw = new WWW("192.168.1.108/CheckLogin.php?", logform);
        yield return logw;
        if(logw.text == "notvalidated")
        {
            ShowValidate();
        }else if(logw.text == "login")
        {
            user.currentUser =usernameInput.text;
            SceneManager.LoadScene("AccountManagement");
        }
        
    }
    
    public void ShowValidate()
    {
        validateScreen.SetActive(true);
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
