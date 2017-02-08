using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;


public class ServerConnection : MonoBehaviour
{  
    private StreamWriter swSender;
    //private TcpClient tcpServer;
    private Thread thrMessaging;
    private bool Connected;

    private Vector3 lastPos;
    private float lastRot;
    
    public UserStats user;

    public static GameObject spawnPlayer;

    public bool isConnected = false;

    public Socket clientSocket;
    public string strName;
    public EndPoint epServer;
    
    byte[] byteData = new byte[1024];
    
    private static List<Data> processQueue = new List<Data>();

    private new void SendMessage(string p)
    {
        if (p != "")
        {
            p = WWW.UnEscapeURL(p, System.Text.Encoding.UTF8);
            swSender.WriteLine(p);
            swSender.Flush();
        }
    }

    void Awake()
    {
        spawnPlayer = Resources.Load("ThirdPersonUMAOther") as GameObject;
    }
    
    void Update()
    {
            if (processQueue.Count > 0)
            {
                //Debug.Log("Process queue isn't empty: " + processQueue.Count);
                for(int i = 0; i < processQueue.Count; i++)
                {
                Data data = processQueue[i];
                    switch (data.cmdCommand)
                    {
                        case Command.Login:
                        //Debug.Log("Spawning " + data.strName + " Message: " + data.strMessage);
                        SpawnPlayer(data.strName, data.strMessage);
                            
                            break;
                        case Command.Transform:
                        Debug.Log(data.strMessage);
                            MovePlayer(data.strName, data.strMessage);
                            break;
                        case Command.Message:
                            //Debug.Log("Process Message: " + data.strMessage + " From: " + data.strName);
                            break;
                        case Command.ClientList:
                        Debug.Log(data.strMessage);
                        string[] temp = data.strMessage.Split('!'); //Array of all player names and positions
                        for(int i2 = 0; i2 < temp.Length - 1; i2++)
                        {
                            string[] player = temp[i2].Split(':');
                            SpawnPlayer(player[0], player[1]);
                        }
                        
                        break;
                    }
                processQueue.RemoveAt(i);
                }
            }
        
    }

    public void ScheduleTask(Data newData)
    {
        processQueue.Add(newData);
        
    }

    public void MainMethod(UserStats u)
    {
        this.user = u;
        string name = user.currentCharacter;
        strName = name;
        Data msgToSend = new Data();
            //Using UDP sockets
            clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            //IP address of the server machine
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //Server is listening on port 1000
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);

            epServer = (EndPoint)ipEndPoint;


            msgToSend.cmdCommand = Command.Login;
            msgToSend.strMessage = user.currentPos.x +"/"+user.currentPos.y+"/"+user.currentPos.z+"/"+user.currentRot.x+"/"+user.currentRot.y+"/"+user.currentRot.z;
            msgToSend.strName = strName;

            byteData = msgToSend.ToByte();

            //Login to the server
            clientSocket.BeginSendTo(byteData, 0, byteData.Length,
                SocketFlags.None, epServer, new AsyncCallback(OnSend), null);


        byteData = new byte[1024];
        //The user has logged into the system so we now request the server to send
        //the names of all users who are in the chat room


        //Start listening to the data asynchronously
        clientSocket.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref epServer,
                                       new AsyncCallback(OnReceive), null);
        Debug.Log("Started Receiving");
    }
    
    public void OnReceive(IAsyncResult ar)
    {
        try
        {
            Debug.Log("Received Message");
        clientSocket.EndReceive(ar);
        //Convert the bytes received into an object of type Data
        Data msgReceived = new Data(byteData);
            Debug.Log(msgReceived.cmdCommand);
            switch (msgReceived.cmdCommand)
            {
                case Command.ClientList:
                    ScheduleTask(msgReceived);
                    break;
                case Command.Login:
                    ScheduleTask(msgReceived);
                   break;
                case Command.Logout:
                                        
                    break;
                case Command.Message:
                    ScheduleTask(msgReceived);
                    break;
                case Command.Transform:
                    ScheduleTask(msgReceived);
                    break;
                    
            }

        byteData = new byte[1024];

        //Start listening to receive more data from the user
        clientSocket.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref epServer,
                                       new AsyncCallback(OnReceive), null);
        }
        catch (ObjectDisposedException)
        {
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void OnSend(IAsyncResult ar)
    {
        try
        {
            clientSocket.EndSend(ar);
        }
        catch (ObjectDisposedException)
        { }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void SendPosition(Vector3 pos, float rot)
    {
        if (user != null && (lastPos != pos || lastRot != rot))
        {
            lastRot = rot;
            lastPos = pos;
            SendMessage(new ChatMessage(pos.x.ToString("F2") + "/" + pos.y.ToString("F2") + "/" + pos.z.ToString("F2") + "/" + rot.ToString("F2") ));
        }
    }

    private void SendMessage(ChatMessage msg)
    {
        Data msgToSend = new Data();
        try
        {
            //Using UDP sockets
            clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            //IP address of the server machine
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //Server is listening on port 1000
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);

            epServer = (EndPoint)ipEndPoint;


            msgToSend.cmdCommand = Command.Transform;
            msgToSend.strMessage = msg.ToString();
            msgToSend.strName = user.currentCharacter;

            byte[] byteData = msgToSend.ToByte();

            //Login to the server
            clientSocket.BeginSendTo(byteData, 0, byteData.Length,
                SocketFlags.None, epServer, new AsyncCallback(OnSend), null);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

        }

    void OnApplicationQuit()
    {
        Data msgToSend = new Data();
        try
        {
            //Using UDP sockets
            clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

            //IP address of the server machine
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //Server is listening on port 1000
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);

            epServer = (EndPoint)ipEndPoint;


            msgToSend.cmdCommand = Command.Logout;
            msgToSend.strMessage = "";
            msgToSend.strName = user.currentCharacter;

            byte[] byteData = msgToSend.ToByte();

            //Login to the server
            clientSocket.BeginSendTo(byteData, 0, byteData.Length,
                SocketFlags.None, epServer, new AsyncCallback(OnSend), null);
            Debug.Log("Logout");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

    }

    public void SpawnPlayer(string name, string message)
    {
        user.SetUMAKit();

        if (name != user.currentCharacter)
        {
            Debug.Log("Spawn player " + name);
            GameObject go = Instantiate(spawnPlayer);

            object[] array = parseMessage(message);
            go.transform.position = (Vector3)array[0];
            go.transform.eulerAngles = (Vector3)array[1];
            go.name = name;
            LoadCharacterInWorld loadChar = (LoadCharacterInWorld)go.GetComponent<LoadCharacterInWorld>();
            loadChar.slotLibrary = user.UMAKit.GetComponentInChildren<SlotLibrary>();
            loadChar.generator = user.UMAKit.GetComponentInChildren<UMAGenerator>();
            loadChar.overlayLibrary = user.UMAKit.GetComponentInChildren<OverlayLibrary>();
            loadChar.raceLibrary = user.UMAKit.GetComponentInChildren<RaceLibrary>();

            loadChar.LoadOther(name);
            Debug.Log("Spawned " + name);
        }
    }

    private void MovePlayer(string name, string message)
    {
        object[] array = parseMessage(message);
        GameObject go = GameObject.Find(name);
        go.transform.position = (Vector3)array[0];
        go.transform.eulerAngles = (Vector3)array[1];

    }

    private object[] parseMessage(string message)
    {
        string[] temp = message.Split('/');
        object[] array = new object[3];
        array[0] = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
        array[1] = new Vector3(0, float.Parse(temp[3]), 0);

        return array;

    }

}

[Serializable()]
public class ChatMessage
{
    public string message;

    public ChatMessage(string message)
    {
        this.message = message;
    }

    public override string ToString()
    {
        return message;
    }
}

public enum Command
{
    Login,      //Log into the server
    Logout,     //Logout of the server
    Message,    //Send a text message to all the chat clients
    List,       //Get a list of users in the chat room from the server
    Transform,  //Client position/rotation in world update
    Null,
    ClientList//No command
}

public class Data
{
    //Default constructor
    public Data()
    {
        this.cmdCommand = Command.Null;
        this.strMessage = null;
        this.strName = null;
    }

    //Converts the bytes into an object of type Data
    public Data(byte[] data)
    {
        //The first four bytes are for the Command
        this.cmdCommand = (Command)BitConverter.ToInt32(data, 0);

        //The next four store the length of the name
        int nameLen = BitConverter.ToInt32(data, 4);

        //The next four store the length of the message
        int msgLen = BitConverter.ToInt32(data, 8);

        //This check makes sure that strName has been passed in the array of bytes
        if (nameLen > 0)
            this.strName = Encoding.UTF8.GetString(data, 12, nameLen);
        else
            this.strName = null;

        //This checks for a null message field
        if (msgLen > 0)
            this.strMessage = Encoding.UTF8.GetString(data, 12 + nameLen, msgLen);
        else
            this.strMessage = null;
    }

    //Converts the Data structure into an array of bytes
    public byte[] ToByte()
    {
        List<byte> result = new List<byte>();

        //First four are for the Command
        result.AddRange(BitConverter.GetBytes((int)cmdCommand));

        //Add the length of the name
        if (strName != null)
            result.AddRange(BitConverter.GetBytes(strName.Length));
        else
            result.AddRange(BitConverter.GetBytes(0));

        //Length of the message
        if (strMessage != null)
            result.AddRange(BitConverter.GetBytes(strMessage.Length));
        else
            result.AddRange(BitConverter.GetBytes(0));

        //Add the name
        if (strName != null)
            result.AddRange(Encoding.UTF8.GetBytes(strName));

        //And, lastly we add the message text to our array of bytes
        if (strMessage != null)
            result.AddRange(Encoding.UTF8.GetBytes(strMessage));

        return result.ToArray();
    }

    public string strName;      //Name by which the client logs into the room
    public string strMessage;   //Message text
    public Command cmdCommand;  //Command type (login, logout, send message, etcetera)
                                //Command type (login, logout, send message, etcetera)
    public String position;
    public float rotation;
}
