using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AccountManagement : MonoBehaviour {

    public UserStats user;

    void Start()
    {
        user = GameObject.Find("_User").GetComponent<UserStats>();
    }

    public void OnCharacterCreationClick()
    {
        SceneManager.LoadScene("CharacterCreation");
    }

    public void OnCharacterSelectClick()
    {

        StartCoroutine(CheckCharacters());

        

        
    }

    IEnumerator CheckCharacters()
    {
        WWWForm logform = new WWWForm();
        logform.AddField("user", user.currentUser);
        WWW logw = new WWW("192.168.1.108/GetCharacterCount.php?", logform);

        
        yield return logw;
        int num = int.Parse(logw.text);
        if (num > 0)
        {
            Debug.Log(num);
            user.numberOfCharacters = num;
            SceneManager.LoadScene("CharacterSelect");
        }
        else
        {
            SceneManager.LoadScene("CharacterCreation");
        }


    }

}
