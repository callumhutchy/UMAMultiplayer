using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour {

    public CharacterSelect charSelect;

    

    public string loadString;

    void Start()
    {
        charSelect = GameObject.FindGameObjectWithTag("CharacterSelect").GetComponent<CharacterSelect>();
    }

	public void OnClick()
    {
        
        charSelect.Load(loadString);
        charSelect.user.currentCharacter = GetComponentInChildren<Text>().text;
        charSelect.user.currentCharFile = loadString;

    }

}
