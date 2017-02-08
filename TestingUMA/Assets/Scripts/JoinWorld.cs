using UnityEngine;
using System.Collections;

public class JoinWorld : MonoBehaviour {

    public UserStats user;
    private ServerConnection con;
    public LoadCharacterInWorld load;

    // Use this for initialization
    void Start () {
        user = GameObject.FindGameObjectWithTag("UserStats").GetComponent<UserStats>();
        con = GameObject.FindGameObjectWithTag("UserStats").GetComponent<ServerConnection>();
        if(user != null)
        {
            load.Load();
        }


	}

    public GameObject player;

	// Update is called once per frame
	void Update () {
        if (con != null)
        {
            con.SendPosition(player.transform.position, player.transform.eulerAngles.y);
        }
	}

    public void CreateOtherPlayer(string name)
    {

        GameObject go = (GameObject)Instantiate(Resources.Load("ThirdPersonUMAOther"));
        go.name = name;
        LoadCharacterInWorld goOther = go.GetComponent<LoadCharacterInWorld>();
        goOther.LoadOther(name);

    }

    public void updateOtherPlayerPosition(Vector3 pos, string name)
    {
        GameObject go = GameObject.Find(name);
        go.transform.position = pos;

    }

}
