﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworksApi.TCP.SERVER;
using System.Net;
using System.Net.Sockets;
using SmartOfficeServer.Model;
using Newtonsoft.Json;
using MySql.Data;
namespace SmartOfficeServer
{
    class TCPServer
    {
        Server server;
        string iPAddress;
        const string port = "8888";
        JSONObject obj;
        DBConnect MySQlConnection;
        List<List<String>> users = new List<List<string>>();
        List<List<String>> admins = new List<List<String>>();
        List<List<String>> robot_status = null;
        List<String> logged_in_users = new List<string>();
        System.Timers.Timer admin_data_timer = new System.Timers.Timer(10000);
        private const string unity_client = "unity";
        //1: Notifications(Any kind) 2: Coffee 3: Login request 4: Mail request 5: BLANK 6: Initial user Data 7: delivery history 8: notification history 10:recall robot
        private const int NOTIFICATION_REQUEST = 1;
        private const int COFFEE_REQUEST = 2;
        private const int LOGIN_REQUEST = 3;
        private const int MAIL_REQUEST = 4;
        private const int DELIVERY_REQUEST = 5;
        private const int INITIAL_USER_DATA = 6;
        private const int DELIVERY_HISTORY = 7;
        private const int NOTIFICATION_HISTORY = 8;
        private const int ADMIN_DATA = 9;
        private const int RECALL_ROBOT = 10;
        public static int Main(string[] args)
        {
            
            GetLocalIPAddress();
            TCPServer Init = new TCPServer();
            Console.WriteLine("Begin Server initialization..............");
            Init.initializeServer();
            Console.WriteLine("Begin database initialization............");
            Init.intializeDatabase();
            Console.WriteLine("Fetching list of users............");
            Init.initializeSettings();
            Console.WriteLine("Ending Initialization...... \nServer setup fully \n==================================");

            /*
            List<List<String>> data = new List<List<string>>();
            String name = "raghav";
            
            data = Init.MySQlConnection.SelectAll("user", "id", "username = '" + name + "'; ");

            Console.WriteLine(data.ElementAt(0).ElementAt(0));
            String id = data.ElementAt(0).ElementAt(0);

            data = Init.MySQlConnection.SelectAll("user", "*", "true");

           
            //Now we just need to serialize this chunk of data into a JSON object and pass it on to the client
            JSONObject obj = new JSONObject(DELIVERY_HISTORY, data);
            String jasonString =  JsonConvert.SerializeObject(obj);

            JSONObject obj2 = JsonConvert.DeserializeObject<JSONObject>(jasonString);
            List<List<String>> jsonList = JsonConvert.DeserializeObject<List<List<String>>>(obj2.info.ToString());

            foreach(List<String> list in jsonList)
            {
                foreach(String d in list)
                {
                    Console.Write(d + "  ");
                }
                Console.WriteLine();
            }
            */

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
            return 0;

        }

        public void initializeServer()
        {
            //dynamically gets the local IP address of the server
            iPAddress = "172.31.131.100";
            server = new Server(iPAddress, port);
            server.OnClientConnected += Server_OnClientConnected;
            server.OnClientDisconnected += Server_OnClientDisconnected;
            server.OnDataReceived += Server_OnDataReceived;
            server.OnServerError += Server_OnServerError;

            server.Start();
            Console.WriteLine("Server Connected! \n");

        }
        public void intializeDatabase()
        {
            //Open DB connection and display message
            MySQlConnection = new DBConnect();
            if(MySQlConnection.OpenConnection())
            {
                Console.WriteLine("Sucessfully connected to database\n");
            }
            else
            {
                Console.WriteLine("Error connecting to database\n");
            }
        }
        public void initializeSettings()
        {
            users  = MySQlConnection.SelectAll("user", "id,username", "true");
            admins = MySQlConnection.SelectAll("user", "id,username", "department='admin'");
            admin_data_timer.Elapsed += Admin_data_timer_Elapsed;
            admin_data_timer.Start();
        }

        /// <summary>
        /// Every 5 min, send a pulse to the admins regarding the robots vitals and logged in employees
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_data_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                robot_status = MySQlConnection.SelectAll("robot", "id_robot,battery_status", "true");
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                //occurs when connection to database has been lost
                Console.WriteLine("Error fetching robot vitals");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Sending robot vitals and logged in user data to admins.......");
            foreach(List<String> admin_users in admins)
            {
                try
                { 
                    server.SendTo(admin_users.ElementAt(1), JsonConvert.SerializeObject(new JSONObject(ADMIN_DATA, robot_status)));
                    server.SendTo(admin_users.ElementAt(1), JsonConvert.SerializeObject(new JSONObject(ADMIN_DATA, logged_in_users)));
                }
                catch(Exception ex)
                {
                    //occurs when unable to contact 1 of the admins. 
                }
            }

        }

        private void Server_OnServerError(object Sender, ErrorArguments R)
        {
            
            Console.WriteLine(R.ErrorMessage);
        }

        /// <summary>
        /// Executed when the server receives data from a client. based on requestType executes different method stubs
        /// </summary>
        /// <param name="requestType"> 
        /// 1: Notifications(Any kind) 2: Coffee 3: Login request 4: Mail request 5: delivery request 6: Initial user Data 7: delivery history 8: notification history 10: recall robot
        /// </param>
        /// <param name="obj">Desired data</param>
        private void Server_OnDataReceived(object Sender, ReceivedArguments R)
        {
            String response = "";
            Console.WriteLine("I got data from " + R.Name);
         //   Console.WriteLine(R.ReceivedData.ToString());
            obj = JsonConvert.DeserializeObject<JSONObject>(R.ReceivedData);
            //differentiate data based on the type of request received
            switch(obj.requestType)
            {
                case 1:
                    Console.WriteLine(DateTime.Now + ": Generating notification for " + R.Name);
                    Notification_Handler(R.Name, obj.info);
                    Console.WriteLine();
                    break;
                case 2:
                    Console.WriteLine(DateTime.Now + ": Coffee delivery requested by " + R.Name);
                    response = Coffee_Handler(R.Name);
                    Console.WriteLine(DateTime.Now + ": Coffee delivery response by " + R.Name + " delivered\n");
                    break;
                case 3:
                    //convert login string to object and pass it to login method to verify
                    Console.WriteLine(DateTime.Now + ": Trying to login " + R.Name);
                    response = Login_Handler(obj.info);
                    Console.WriteLine(DateTime.Now + ": Login attempt complete for " + R.Name + "\n");
                    break;
                case 4:
                    Console.WriteLine(DateTime.Now + ": " + R.Name + " is requesting their mail from mail room");
                    response = Mail_Request_Handler(R.Name);
                    Console.WriteLine(DateTime.Now + ": Mail dispatched for " + R.Name + "\n");
                    break;
                case 5:
                    Console.WriteLine(DateTime.Now + ": " + R.Name + " is requesting for mail delivery");
                    response = Delivery_Request_Handler(R.Name);
                    Notification_Handler(R.Name,obj.info);
                    Console.WriteLine(DateTime.Now + ": Robot dispatched for " + R.Name + " for mail delivery\n");
                    break;
                case 6:
                    Console.WriteLine(DateTime.Now + ": Initial user data requested by " + R.Name);
                    response = Initial_user_data_handler(R.Name);
                    Console.WriteLine(DateTime.Now + ": Initial user data requested by " + R.Name + " has been dispatched\n");
                    break;
                case 7:
                    Console.WriteLine(DateTime.Now + ": " + R.Name + " is requesting their delivery history");
                    response = Delivery_History_Handler(R.Name);
                    Console.WriteLine(DateTime.Now + ": delivery history data requested by " + R.Name + " has been dispatched\n");
                    break;
                case 8:
                    Console.WriteLine(DateTime.Now + ": " + R.Name + " is requesting their notification history");
                    response = Notification_History_Handler(R.Name);
                    Console.WriteLine(DateTime.Now + ": notification history data requested by " + R.Name + " has been dispatched\n");
                    break;
                case 10:
                    Console.WriteLine(DateTime.Now + ": ADMIN " + R.Name + " has recalled robot " + obj.info.ToString());
                    response = Recall_Robot_Handler(obj.info);
                    break;

            }//switch
            try
            {
                server.SendTo(R.Name, response);
            }
            catch(Exception e)
            {
                //occurs when server looses connection with client;
                //for future=> try sending the data after connection is regained. would need the server to keep some sort of record
                // for this
            }

        }

        private string Recall_Robot_Handler(object info)
        {
            //send data to unity
            server.SendTo(unity_client, obj.info.ToString());
            return JsonConvert.SerializeObject(new JSONObject(RECALL_ROBOT, "Robot will be recalled soon"));
        }

        /// <summary>
        /// Generates a notification for the reciver as well as inserts that data into the database
        /// </summary>
        /// <param name="sender_name"></param>
        /// <param name="info"></param>
        private void Notification_Handler(string sender_name, object info)
        {
            String json = JsonConvert.SerializeObject(info);
            Mail mail = JsonConvert.DeserializeObject<Mail>(json);
            if(mail.note==null)
            {
                mail.note = " "; //if sender did not add any note
            }
            // Notification notification = new Notification(name,)


            foreach (List<String> user in users)
            {   //match user ID to find name
                if (user.ElementAt(1) == mail.mailDestination)
                {
                    mail.mailDestination = user.ElementAt(0);   //replace with user id instead
                }
                if (user.ElementAt(1) == sender_name)
                {
                    sender_name = user.ElementAt(0);   //replace with user id instead
                    break;
                }
            }


            //add a notification to the database
            //create a list of the notification
            //send that list to the user 
            List<String> notification = new List<string>();
            List<String> delivery = new List<string>();
        //populate notification arguments
            notification.Add("'" + mail.mailDestination + "'");
            notification.Add("'" + sender_name + "'");
            notification.Add("'" + mail.subject + "'");
            notification.Add("'" + mail.note + "'");
            notification.Add("'" + mail.mailTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            //populate delivery arguments
            delivery.Add("'" + sender_name + "'");
            delivery.Add("'" + mail.mailDestination + "'");
            delivery.Add("'" + "2" + "'");//request type 2 is for deliveries
            delivery.Add("'" + mail.mailTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");

            /// query = insert into notification (id_user, id_sender, subject, description, time) values ();
            ///      = insert into delivery (id_user, id_reciver, request_type, start_time) values ();
            /// add records to both the notification and delivery tables
            try
            {
            //add to the notification table
                MySQlConnection.Insert("notification", "id_user, id_sender, subject, description, time",notification);
                //add to the delivery table
                MySQlConnection.Insert("delivery", "id_user,id_reciver,request_type,start_time", delivery);
            }
            catch(MySql.Data.MySqlClient.MySqlException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
            notification.Insert(0, " ");    //add dummy notification id. we don't need it to display notification to the user. It is stored in the database if needed
        //Send mail notification to the receiver of the mail so they know it's coming
            server.SendTo(mail.mailDestination, JsonConvert.SerializeObject(notification));

        }

        /// <summary>
        /// Handles delivery requests. Note: Initial database entries are handled by notification handler
        /// </summary>
        /// <param name="name">the sender of this request</param>
        /// <returns></returns>
        private string Delivery_Request_Handler(string name)
        {
            
            //Unity data here


            return JsonConvert.SerializeObject(new JSONObject(DELIVERY_REQUEST, "A robot will arrive for pickup soon!"));

        }

        /// <summary>
        /// Deliver mail from the mail room to an employee
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private String Mail_Request_Handler(string name)
        {
            //Unity data here

            return JsonConvert.SerializeObject(new JSONObject(MAIL_REQUEST, "A robot will deliver you your mail soon!"));
        }

        private String Notification_History_Handler(string name)
        {
            List<List<String>> notification;
            //          query = "Select id from user where username='" + name + "';";
            //          needed to get user's ID, will reuse to get notifications
            notification = MySQlConnection.SelectAll("user", "id", "username = '" + name + "'; ");
            //Should only return 1 value
            String id = notification.ElementAt(0).ElementAt(0);
            //            query = "Select * from user, notifications where user.id = 'id' and user.id=notification.id_user limit 50";
            //            get notification history of the user
            notification = MySQlConnection.SelectAll("user,notification n", "n.id_notification,n.id_user,n.id_sender,n.subject,n.description,n.time", "user.id = '" + id + "' and user.id=n.id_user order by n.time limit 50;");

            //Now we just need to serialize this chunk of data into a JSON object and pass it on to the client
            JSONObject obj = new JSONObject(NOTIFICATION_HISTORY, notification);
            return JsonConvert.SerializeObject(obj);
        }

        private String Delivery_History_Handler(string name)
        {
            List<List<String>> delivery;
            //          query = "Select id from user where username='" + name + "';";
            //          needed to get user's ID, will reuse to get notifications
            delivery = MySQlConnection.SelectAll("user", "id", "username = '" + name + "'; ");
            //Should only return 1 value
            String id = delivery.ElementAt(0).ElementAt(0);
            //          query = "Select * from delivery, user where user.id = id and user.id = delivery.id_user and status = 1 limit 50;"
            //          get delivery history of user
            delivery = MySQlConnection.SelectAll("delivery d,user", "d.id_delivery, d.id_reciver, d.request_type, d.start_time, d.end_time, d.status", "user.id = '" + id + "' and user.id = d.id_user and status = 1 limit 50;");

            //Now we just need to serialize this chunk of data into a JSON object and pass it on to the client
            JSONObject obj = new JSONObject(DELIVERY_HISTORY, delivery);
            return JsonConvert.SerializeObject(obj);
        }

        private String Initial_user_data_handler(string name)
        {
            List<List<String>> user;
            //          query = "Select id from user where username='" + name + "';";
            //          needed to get user's ID, will reuse to get all users
            user = MySQlConnection.SelectAll("user", "id", "username = '" + name + "'; ");
            //Should only return 1 value
            String id = user.ElementAt(0).ElementAt(0);
            //            query = "Select * from user where true"
            //            to fetch all the users in DB
            user = MySQlConnection.SelectAll("user", "*", "true");

            //Now we just need to serialize this chunk of data into a JSON object and pass it on to the client
            JSONObject obj = new JSONObject(INITIAL_USER_DATA, user);
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Checks for login of the user. returns false if entered details are incorrect. true otherwise
        /// Also populates the user object with other details like name, age, department etc
        /// </summary>
        /// <param name="login">User object with username and password initialized</param>
        /// <returns></returns>
        public String Login_Handler(Object login_object)
        {

            String loginString = JsonConvert.SerializeObject(login_object);
            User login = JsonConvert.DeserializeObject<User>(loginString);
            Boolean login_status = false;
            List<List<String>> returnData = null;   //perform binary comparison for case sensitivity
            returnData = MySQlConnection.SelectAll("user","*", "BINARY username='" + login.username + "' and password = '" + login.password + "';");

            /// if an empty set was returned meaning no matches, otherwise atleast 1 set was returned
            /// and the user is legit. We don't have to worry about more than 1 entry being returned
            /// because username field in the database is set to unique so there is no possibility of
            /// duplicates
            if (returnData == null)
            {
                login_status = false; //no matching login found
            }
            else
            {
                login_status = true; //some record was found
                                     //populate user object
                login.id = returnData.ElementAt(0).ElementAt(0);
                login.Name = returnData.ElementAt(0).ElementAt(3);
                login.age = returnData.ElementAt(0).ElementAt(4);
                login.email = returnData.ElementAt(0).ElementAt(5);
                login.phone = returnData.ElementAt(0).ElementAt(6);
                login.department = returnData.ElementAt(0).ElementAt(7);
                login.image = returnData.ElementAt(0).ElementAt(8);
            }
            //create a login object of this information
            loginObject userLogin = new loginObject(login_status, login);
            //convert to standerdized JSON object
            JSONObject obj = new JSONObject(LOGIN_REQUEST, userLogin);
            return JsonConvert.SerializeObject(obj);

        }//end login

        private String Coffee_Handler(string name)
        {
            
        //Send data to unity

        //Unity
            String response = "Your coffee will arrive soon!";
        //convert to standerdized JSON object for sending
            JSONObject obj = new JSONObject(COFFEE_REQUEST, response);
            return JsonConvert.SerializeObject(obj);

        }

        private void Server_OnClientDisconnected(object Sender, DisconnectedArguments R)
        {
            Console.WriteLine(R.Name + " has disconnected!");
            logged_in_users.Remove(R.Name);
        }

        private void Server_OnClientConnected(object Sender, ConnectedArguments R)
        {
            Console.WriteLine(R.Name + " has connected");
            logged_in_users.Add(R.Name);
        }
        public static string GetLocalIPAddress()
        {
            string IP4Address = String.Empty;
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            
            return IP4Address;
            
        }//get iP address




        /*      DATA PRINTING STUB, use as needed
                    foreach(List<string> list in returnData)
            {
               foreach(String data in list)
                {
                    Console.Write(data + " ");
                }
                Console.WriteLine("");
            }
        */
    }//class
}//namespace
