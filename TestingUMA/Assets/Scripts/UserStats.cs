using UnityEngine;
using System.Collections;

public class UserStats : MonoBehaviour {

    public string currentUser;
    public string currentCharacter;
    public Vector3 currentPos;
    public Vector3 currentRot;
    public string currentCharFile;
    public GameObject player;
    public GameObject UMAKit;
    public int numberOfCharacters;

    private ServerConnection con;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        SetUMAKit();
	}

    public void SetUMAKit()
    {
        if (UMAKit == null)
        {
            UMAKit = GameObject.FindGameObjectWithTag("UMAKit");
        }
    }

    public void ConnectToServer()
    {
        con = new ServerConnection();
        con.MainMethod(this);
    }
}
