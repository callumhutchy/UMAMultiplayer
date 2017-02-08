using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Validate : MonoBehaviour {

    public InputField usernameInput;
    public InputField validationCodeInput;
    


    public void OnValidateClick()
    {
        StartCoroutine(Validation());
    }

    IEnumerator Validation()
    {

        WWWForm logform = new WWWForm();
        logform.AddField("user", usernameInput.text);
        logform.AddField("valCode", validationCodeInput.text);
        WWW logw = new WWW("192.168.1.108/ValidateUser.php?", logform);
        yield return logw;
        Debug.Log(logw.text);
       if(logw.text == "validated")
        {
            this.gameObject.SetActive(false);
        }
        else
        {

        }



    }

}
