using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace Server
{
    //The commands for interaction between the server and the client
    enum Command
    {
        Login,      //Log into the server
        Logout,     //Logout of the server
        Message,    //Send a text message to all the chat clients
        List,       //Get a list of users in the chat room from the server
        Transform,
        Null,
        ClientList//No command
    }

    public partial class SGSserverForm : Form
    {
        //The ClientInfo structure holds the required information about every
        //client connected to the server
        struct ClientInfo
        {
            public ClientInfo(EndPoint ep, string Name, string pR)
            {
                endpoint = ep;
                strName = Name;
                posRot = pR;
            }

            public EndPoint endpoint;   //Socket of the client
            public string strName;
            public string posRot; //Name by which the user logged into the chat room
        }


        List<Data> processQueue = new List<Data>();

        List<GraphData> conUsersList = new List<GraphData>();
        List<GraphData> messageReceivedList = new List<GraphData>();
        List<GraphData> messageSentList = new List<GraphData>();

        //The collection of all clients logged into the room (an array of type ClientInfo)
        List<ClientInfo> clientList;

        //The main socket on which the server listens to the clients
        Socket serverSocket;

        int messagesReceived = 0;
        int messagesSent = 0;

        byte[] byteData = new byte[1024];

        public SGSserverForm()
        {
            clientList = new List<ClientInfo>();
            InitializeComponent();
        }

        int chartIndex = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            String now = DateTime.Now.ToString("HH:mm:ss");
            conUsersList.Add(new GraphData(clientList.Count, now));

            chtConnectedUsers.Series[2].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), messagesReceived);
            chtConnectedUsers.Series[1].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), messagesSent);
            messagesReceived = 0;
            messagesSent = 0;

            chartIndex++;
            chtConnectedUsers.Series[0].Points.Clear();
            chtConnectedUsers.Series[1].Points.Clear();
            chtConnectedUsers.Series[2].Points.Clear();

            if (conUsersList.Count < 30)
            {
                for (int i = 0; i < conUsersList.Count;i++)
                {
                    chtConnectedUsers.Series[0].Points.AddXY(conUsersList[i].time, conUsersList[i].clientCount);
                }
            }else
            {
                int conUsersSize = conUsersList.Count;
                for(int i = 30; i > 0; i--)
                {
                    chtConnectedUsers.Series[0].Points.AddXY(conUsersList[conUsersSize - i].time, conUsersList[conUsersSize - i].clientCount);
                }
            }
            
            lblCount.Text = clientList.Count.ToString();
        }
        
        private void Processing()
        {
            while (true)
            {
                if (processQueue.Count > 0)
                {

                    Data data = processQueue[0];
                    if (data != null)
                    {
                        switch (data.cmdCommand)
                        {
                            case Command.Transform:
                                byte[] message;
                                message = data.ToByte();

                                for (int j = 0; j < clientList.Count; j++)
                                {
                                    ClientInfo clientInfo = clientList[j];
                                    if (clientInfo.strName != data.strName)
                                    {
                                        if (data.cmdCommand == Command.Transform)
                                        {
                                            LogMessage("Sending " + data.strName + " position to " + clientInfo.strName);
                                        }

                                        serverSocket.BeginSendTo(message, 0, message.Length, SocketFlags.None, clientInfo.endpoint,
                                                                        new AsyncCallback(OnSend), clientInfo.endpoint);

                                    }

                                }
                                processQueue.RemoveAt(0);

                                break;
                        }
                    }
                }
                
            }
        }

    private void Form1_Load(object sender, EventArgs e)
    {

            Thread processQueue = new Thread(new ThreadStart(Processing));
            processQueue.Start();

            chtConnectedUsers.Titles.Add("Connected Users");
            chtConnectedUsers.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chtConnectedUsers.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chtConnectedUsers.Series.Clear();
            chtConnectedUsers.Series.Add("Connected Users");
            chtConnectedUsers.Series.Add("Messages Sent");
            chtConnectedUsers.Series.Add("Messages Received");
            chtConnectedUsers.Series[0].ChartType = SeriesChartType.Line;
            chtConnectedUsers.Series[1].ChartType = SeriesChartType.Line;
            chtConnectedUsers.Series[2].ChartType = SeriesChartType.Line;

            chtConnectedUsers.Series[1].Color = Color.Red;
            chtConnectedUsers.Series[2].Color = Color.Green;
            chtConnectedUsers.Series[0].XValueType = ChartValueType.DateTime;
            chtConnectedUsers.Series[1].XValueType = ChartValueType.DateTime;
            chtConnectedUsers.Series[2].XValueType = ChartValueType.DateTime;

            timer1.Tick += timer1_Tick;
            timer1.Start();

            try
        {
	    CheckForIllegalCrossThreadCalls = false;

            //We are using UDP sockets
            serverSocket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Dgram, ProtocolType.Udp);

            //Assign the any IP of the machine and listen on port number 1000
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 1000);

            //Bind this address to the server
            serverSocket.Bind(ipEndPoint);
            
            IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
            //The epSender identifies the incoming clients
            EndPoint epSender = (EndPoint) ipeSender;

            //Start receiving data
            serverSocket.BeginReceiveFrom (byteData, 0, byteData.Length, 
                SocketFlags.None, ref epSender, new AsyncCallback(OnReceive), epSender);                
        }
        catch (Exception ex) 
        { 
            MessageBox.Show(ex.Message, "SGSServerUDP", 
                MessageBoxButtons.OK, MessageBoxIcon.Error); 
        }            
    }

        private void OnReceive(IAsyncResult ar)
        {
            messagesReceived++;
            try
            {
                IPEndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint epSender = (EndPoint)ipeSender;

                serverSocket.EndReceiveFrom (ar, ref epSender);
                
                //Transform the array of bytes received from the user into an
                //intelligent form of object Data
                Data msgReceived = new Data(byteData);

                //We will send this object in response the users request
                Data msgToSend = new Data();

                byte [] message;
                
                //If the message is to login, logout, or simple text message
                //then when send to others the type of the message remains the same
                msgToSend.cmdCommand = msgReceived.cmdCommand;
                msgToSend.strName = msgReceived.strName;

                switch (msgReceived.cmdCommand)
                {
                    case Command.Login:
                        
                        //When a user logs in to the server then we add her to our
                        //list of clients

                        ClientInfo clientInfo = new ClientInfo();
                        clientInfo.endpoint = epSender;      
                        clientInfo.strName = msgReceived.strName;
                        clientInfo.posRot = "0/0/0/0";                       

                        clientList.Add(clientInfo);

                        Data clientListMsg = new Data();
                        clientListMsg.strName = msgReceived.strName;
                        clientListMsg.cmdCommand = Command.ClientList;
                       
                        foreach(ClientInfo client in clientList)
                        {
                            clientListMsg.strMessage += client.strName + ":" + client.posRot + "!";
                        }
                        message = clientListMsg.ToByte();
                        

                        serverSocket.BeginSendTo(message, 0, message.Length, SocketFlags.None, clientInfo.endpoint,
                                                            new AsyncCallback(OnSend), clientInfo.endpoint);

                        //Set the text of the message that we will broadcast to all users
                        msgToSend.strMessage = msgReceived.strMessage;
                        LogMessage("<<<" + msgReceived.strName + " has joined the room>>> " + msgReceived.strMessage);
                        break;

                    case Command.Logout:

                        //When a user wants to log out of the server then we search for her 
                        //in the list of clients and close the corresponding connection
                        foreach (ClientInfo client in clientList)
                        {
                            Console.WriteLine(client.strName + " : " + client.endpoint + " : " + epSender);
                            if (client.strName == msgReceived.strName)
                            {
                                
                                clientList.Remove(client);
                                //Console.WriteLine("Removed " + client.strName);
                                break;
                            }
                        }                                               
                        
                        msgToSend.strMessage = "<<<" + msgReceived.strName + " has left the room>>>";
                        break;

                    case Command.Message:

                        //Set the text of the message that we will broadcast to all users
                        msgToSend.strMessage = msgReceived.strName + ": " + msgReceived.strMessage;
                        break;
                    case Command.Transform:
                        msgToSend.strMessage = msgReceived.strName + ": " + msgReceived.strMessage;
                        int index = clientList.FindIndex(x => x.strName == msgReceived.strName);
                        clientList[index] = new ClientInfo(clientList[index].endpoint, clientList[index].strName, msgReceived.strMessage);
                        Data tempData = new Data();
                        tempData.cmdCommand = Command.Transform;
                        tempData.strName = msgReceived.strName;
                        tempData.strMessage = msgReceived.strMessage;
                        processQueue.Add(tempData);
                        break;
                        
                }
                if (msgToSend.cmdCommand != Command.List && msgToSend.cmdCommand != Command.Transform)   //List messages are not broadcasted
                {
                    message = msgToSend.ToByte();

                    foreach (ClientInfo clientInfo in clientList)
                    {
                        if (clientInfo.strName != msgToSend.strName)
                        {
                            if (msgToSend.cmdCommand == Command.Transform)
                            {
                                LogMessage("Sending " + msgReceived.strName + " position to " + clientInfo.strName);
                            }

                            serverSocket.BeginSendTo(message, 0, message.Length, SocketFlags.None, clientInfo.endpoint,
                                                            new AsyncCallback(OnSend), clientInfo.endpoint);

                            //Send the message to all users
                        }                          
                        
                    }
                    
                }
                
                LogMessage(msgToSend.strMessage);
                //If the user is logging out then we need not listen from her
               
                    //Start listening to the message send by the user
                    serverSocket.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref epSender,
                        new AsyncCallback(OnReceive), null);
                
                    
                
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "SGSServerUDP", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        public void OnSend(IAsyncResult ar)
        {
            messagesSent++;
            try
            {                
                serverSocket.EndSend(ar);
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "SGSServerUDP", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
        
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            Data msgToSend = new Data();

            byte[] message;

            //If the message is to login, logout, or simple text message
            //then when send to others the type of the message remains the same
            msgToSend.cmdCommand = Command.Message;
            msgToSend.strName = "Server";
            msgToSend.strMessage = "Test Message From Server";
            message = msgToSend.ToByte();

            foreach (ClientInfo clientInfo in clientList)
            {
                if (clientInfo.strName != msgToSend.strName)
                {
                    

                    serverSocket.BeginSendTo(message, 0, message.Length, SocketFlags.None, clientInfo.endpoint,
                                                    new AsyncCallback(OnSend), clientInfo.endpoint);
                    LogMessage(msgToSend.strMessage + " " + msgToSend.cmdCommand.ToString());
                    //Send the message to all users
                }

            }
        }

        private void LogMessage(string message)
        {
            
            txtLog.AppendText(message + "\r\n");
        }
        
    }

    class GraphData
    {
        public int clientCount;
        public String time;

        public GraphData(int clientCount, String time)
        {
            this.clientCount = clientCount;
            this.time = time;
        }
    }


    
    //The data structure by which the server and the client interact with 
    //each other
    class Data
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
    }
}