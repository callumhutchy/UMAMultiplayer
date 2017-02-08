using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class L:MonoBehaviour
{
	public static string UserVer = "-";
	public static string UserID = "-";
	public static string ExeParam = "-";
    public static string network_Status = "-";
	private Rect windowRect = new Rect ((Screen.width - 200)/2, (Screen.height - 300)/2, 200, 100);
	private bool show = false;
	public GUIText staticText;

  IEnumerator Start() {

      String[] Data = Environment.GetCommandLineArgs();
      if (Data.Length == 4)
      {
          var MyStatusTxt = GameObject.Find("staticText");

          UserVer = Data[1];
          UserID = Data[2];
          ExeParam = Data[3];

          WWWForm form = new WWWForm();
          form.AddField("A1", UserVer);
          form.AddField("A2", UserID);
          form.AddField("A3", ExeParam);
          WWW www = new WWW("http://95.130.174.71/login/client.php", form);
          yield return www;
          network_Status = www.text;
       

			switch (network_Status)
			{
			case "0x1050":
				MyStatusTxt.GetComponent<GUIText>().text = "Login Success!";
				Console.WriteLine("success");
				break;
			case "0x0503":
				MyStatusTxt.GetComponent<GUIText>().text = "Login Error!";
				Console.WriteLine("NoLogin");
                show = true;
				break;
            case "0x0603":
                Application.Quit();
                break;
			}

          //Application.LoadLevel("Login");
      }
      else
      {
          var Logo = GameObject.Find("Unk");
          var Bg = GameObject.Find("Cube");
          Logo.SetActiveRecursively(false);
          Bg.SetActiveRecursively(false);
          show = true;

      }

    }



	void WindowFunction (int windowID) {
		float y = 70;
		GUI.Label(new Rect(20, 30, windowRect.width+200, 20), "Please Run Game Launcher");

		if(GUI.Button(new Rect(10,y, windowRect.width - 20, 20), "Exit"))
		{
			Application.Quit();
			show = false;
		}
	}


	void OnGUI(){
		if (show) {
		windowRect = GUI.Window (0, windowRect, WindowFunction, "Login Game");
				}else{
         GUI.Label(new Rect(25, 25, 150, 25), UserVer);
         GUI.Label(new Rect(25, 50, 150, 25), UserID);
		GUI.Label (new Rect(25, 75, 150, 25), ExeParam);
		}
	}
	

}
