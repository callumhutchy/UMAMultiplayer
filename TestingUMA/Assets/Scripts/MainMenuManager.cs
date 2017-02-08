using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    
    WWW patchwww;
    string updateVersion;
    
    public GameObject registerPanel;
    public GameObject mainMenuPanel;

    public void Start()
    {
        CheckForUpdates();
        GetUser();
    }

   

    public void OnRegisterClick()
    {
        registerPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        registerPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void ShowAccountScreen()
    {
        registerPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        
    }

    void GetUser()
    {
        StartCoroutine(CheckDatabase("callumhutchy"));
    }

    IEnumerator CheckDatabase(string username)
    {
        WWWForm logform = new WWWForm();
        logform.AddField("Username", username);
        WWW logw = new WWW("192.168.1.108/loginCheck.php?", logform);
        yield return logw;
        Debug.Log(logw.text);
    }

    void CheckForUpdates()
    {
        

       // StartCoroutine(FinishDownload(gameLatestVersionURL));
        
       

    }

    IEnumerator FinishDownload(string url)
    {
        string currentVersion = "0.0.1";
        
        patchwww = new WWW(url);
        yield return patchwww;
        updateVersion = (patchwww.text).Trim();
        if (updateVersion == currentVersion)
        {
            Debug.Log("Currently up to date");

        }
        else
        {
            string patch = updateVersion;
            Debug.Log("Update available. \n\n Current version: " + currentVersion + "\n Patch Version: " + patch + "\n Would you like to down updates?");
        }
    }
}
