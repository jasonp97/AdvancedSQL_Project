/*
* FILE: Workstation.cs
* PROJECT: PROG3070 - Final Project
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the functionalities that involve in instantiating
*              a workstation object to hold the appropriate information related to a workstation.
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace WorkstationSimulator
{
    class Workstation
    {
        // Timer settings
        private Timer timer;

        public static int workstationStatus = 0;    // Workstation status (Paused or Active)

        //Set up a global socket for the workstation.
        private static Socket workstationSkt;
        public static Socket displaySkt;

        //Set the TcpListener base port is 15000.
        private const Int32 baseport = 15000;

        private static Configuration config;        // Configuration of this workstation        
        public static int wID { get; set; }         // Workstation ID
        private static int currentTestTray;         // The current test tray ID
        private static int positionInTray;          // Indicates the next available position to place into tray
        public static Materials materialsBins;      // Set of materials bins
        private static string[] employeeType = { "NE", "EE", "VEE" };   //NE: New Employee; EE: Experienced Employee; VEE: Very Experienced Employee
        private static string[] timeScaleDesc = { "1:1", "1:5", "1:10" };
        private static List<Employee> workersList;          // List of workers
        public static string connectionString = ConfigurationManager.ConnectionStrings["KanbanConnection"].ConnectionString;

        // FUNCTION NAME : StartWorkstation()
        // DESCRIPTION: 
        //		This function instantiates all components in a workstation
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void StartWorkstation()
        {
            // We need to assign ID for the workstation
            Console.Write("Specify workstation ID: ");
            wID = Int32.Parse(Console.ReadLine());
                     
            System.Console.WriteLine("------------------------------ Workstation {0} Simulator ------------------------------", wID);

            // Set up socket connection to Andon display           
            workstationSkt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Set up socket listener for the ground terminal software.                
                //workstationSkt.Bind(localEPoint);

                int port = baseport + wID;
                workstationSkt.Bind(new IPEndPoint(IPAddress.Any, port));
                workstationSkt.Listen(3);

                // Accept incoming connection attempt
                workstationSkt.BeginAccept(new AsyncCallback(AcceptConnection), null);

            }
            catch (Exception)
            {
                Console.Write("Failed to connect to display!");
            }

            config = LoadCurrentConfigs();
            RegisterWorkstation();

            // Initializes test tray
            currentTestTray = 0;
            positionInTray = 0;
            CreateTestTray();

            // Creates materials bins
            materialsBins = new Materials(config);

            // Creates workers
            InstantiatingEmployees(config);
          
            string keystroke;
            do
            {
                Console.WriteLine("Press Enter to start assembly line ...");
                keystroke = Console.ReadLine();
                
            } while (keystroke.Trim() != "");
            
            StartAssemblyLine();
            StartTiming();

            System.Console.ReadLine();           
        }

        // FUNCTION NAME : StartTiming()
        // DESCRIPTION: 
        //		This function starts the timers
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void StartTiming()
        {
            int timescale = 1;
            if(config.TimeScale == timeScaleDesc[0])
            {
                timescale = 1;
            }
            else if (config.TimeScale == timeScaleDesc[1])
            {
                timescale = 5;
            }
            else if (config.TimeScale == timeScaleDesc[2])
            {
                timescale = 10;
            }
            timer = new Timer();
            timer.Interval = 300000/timescale;  // The runner refills the bins every 5 minutes
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        // FUNCTION NAME : Timer_Elapsed()
        // DESCRIPTION: 
        //		This function is triggered each time the timer is due
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Time is up, the runner picks up all Kanban card and refills the bins
            Console.WriteLine("Time to refill!");
            Refill();
            
            // Check if the assembly line is paused, then reactive it
            if(workstationStatus == 0)
            {
                workstationStatus = 1;
                UpdateWorkstaionStatus(workstationStatus);
                ContinueAssemblyLine();
            }
        }

        // FUNCTION NAME : AcceptConnection()
        // DESCRIPTION: 
        //		This function accepts the incoming connection to Andon display
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private static void AcceptConnection(IAsyncResult status_result)
        {
            // Assigns the displaySkt to the received socket from the display
            displaySkt = workstationSkt.EndAccept(status_result);

        }

        // FUNCTION NAME : LoadCurrentConfigs()
        // DESCRIPTION: 
        //		This function reads the list of current configuration settings and applies to the system.
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    Configuration
        private static Configuration LoadCurrentConfigs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                Configuration config = new Configuration();
                string cmdText = @"SELECT * FROM  Configuration";

                SqlCommand cmd = new SqlCommand(cmdText, conn);

                conn.Open();

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    config.HarnessQty = Convert.ToInt32(rd["HarnessQty"]);
                    config.ReflectorQty = Convert.ToInt32(rd["ReflectorQty"]);
                    config.HousingQty = Convert.ToInt32(rd["HousingQty"]);
                    config.LensQty = Convert.ToInt32(rd["LensQty"]);
                    config.BulbQty = Convert.ToInt32(rd["BulbQty"]);
                    config.BezelQty = Convert.ToInt32(rd["BezelQty"]);
                    config.TimeScale = timeScaleDesc[Convert.ToInt32(rd["TimeScale"])];
                    config.AssemblyStationQty = Convert.ToInt32(rd["AssemblyStationQty"]);
                    config.TestTrayQty = Convert.ToInt32(rd["TestTrayQty"]);
                    config.NoOfRookie = Convert.ToInt32(rd["NoOfRookie"]);
                    config.NoOfExperienced = Convert.ToInt32(rd["NoOfExperienced"]);
                    config.NoOfSuper = Convert.ToInt32(rd["NoOfSuper"]);
                }

                conn.Close();

                return config;
            }
        }

        // FUNCTION NAME : InstantiatingEmployees()
        // DESCRIPTION: 
        //		This function instantiates all workers in this assembly line
        // INPUTS :
        //	    config: Configuration
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void InstantiatingEmployees(Configuration config)
        {
            workersList = new List<Employee>();
            int scale;
            switch (config.TimeScale)
            {
                case "1:1": scale = 1; break;
                case "1:5": scale = 5; break;
                case "1:10": scale = 10; break;
                default: scale = 0; break;
            }
            // Instantiating workers in assembly line
            for (int i = 1; i <= config.NoOfRookie; i++)
            {
                // New Employees
                Employee e = new Employee("NE", scale);               
                e.EmployeeID = wID.ToString() + "NE" + i.ToString();
                e.RegisterEmployee();
                workersList.Add(e);               
            }
            for (int i = 1; i <= config.NoOfExperienced; i++)
            {
                // Experienced Employees
                Employee e = new Employee("EE", scale);               
                e.EmployeeID = wID.ToString() + "EE" + i.ToString();
                e.RegisterEmployee();
                workersList.Add(e);                
            }
            for (int i = 1; i <= config.NoOfSuper; i++)
            {
                // Very Experienced Employees
                Employee e = new Employee("VEE", scale);                
                e.EmployeeID = wID.ToString() + "VEE" + i.ToString();
                e.RegisterEmployee();
                workersList.Add(e);               
            }            
        }

        // FUNCTION NAME : GenerateLampNumber()
        // DESCRIPTION: 
        //		This function generates LampID
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    String
        public static string GenerateLampNumber()
        {
            string lampNo = "";
            lampNo += "FL" + currentTestTray.ToString("D6") + positionInTray.ToString("D2");    // FLxxxxxxyy
            UpdateTray();
            return lampNo;           
        }

        // FUNCTION NAME : UpdateTray()
        // DESCRIPTION: 
        //		This function updates the current tray info
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private static void UpdateTray()
        {
            positionInTray++;
            if (positionInTray > 60)
            {
                CreateTestTray();
            }
        }

        // FUNCTION NAME : GenerateTestTrayID()
        // DESCRIPTION: 
        //		This function generates test tray ID
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    String
        public static string GenerateTestTrayID()
        {
            string testTrayID = "";
            testTrayID += wID + "FL" + currentTestTray.ToString("D6") + "yy";   // 1FLxxxxxxyy
            return testTrayID;
        }

        // FUNCTION NAME : CreateTestTray()
        // DESCRIPTION: 
        //		This function creates test tray
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private static void CreateTestTray()
        {
            positionInTray = 1;
            currentTestTray++;
            string testTrayID = GenerateTestTrayID();

            // Save new tray to database
            using (SqlConnection conn = new SqlConnection(Workstation.connectionString))
            {
                string testTrayUnitNo = "FL" + currentTestTray.ToString("D6") + "yy";
                string cmdText = $@"INSERT INTO Test_Tray
                                    VALUES ('{testTrayID}', '{testTrayUnitNo}');";

                SqlCommand cmd = new SqlCommand(cmdText, conn);

                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }
        }

        // FUNCTION NAME : CreateTestTray()
        // DESCRIPTION: 
        //		This function saves this workstation to database
        // INPUTS :
        //	    Configuration config
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void RegisterWorkstation()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string cmdText = $@"EXECUTE Workstation_Modifier {wID}, 0";     //Flag 0 for CREATING workstation

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                

                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                conn.Close();
            }
        }

        // FUNCTION NAME : StartAssemblyLine()
        // DESCRIPTION: 
        //		This function starts off all workers
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void StartAssemblyLine()
        {
            workstationStatus = 1;  // Active status
            UpdateWorkstaionStatus(workstationStatus);
            Console.WriteLine("Workers start working ...");
            foreach(Employee e in workersList)
            {
                e.StartWorking();
            }
        }

        // FUNCTION NAME : stopAssemblyLine()
        // DESCRIPTION: 
        //		This function stops assembly line
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public static void StopAssemblyLine()
        {
            workstationStatus = 0;  // Paused status
            UpdateWorkstaionStatus(workstationStatus);
            foreach (Employee e in workersList)
            {
                e.StopWorking();
            }           
        }

        // FUNCTION NAME : UpdateWorkstaionStatus()
        // DESCRIPTION: 
        //		This function updates workstation status (Active or Paused)
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private static void UpdateWorkstaionStatus(int status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string cmdTxt = $@"UPDATE WorkStation
                                   SET WorkstationStatus = {status}
                                   WHERE WorkStationID = {wID}";
                SqlCommand cmd = new SqlCommand(cmdTxt, conn);
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Failed to update status");
                }

                conn.Close();
            }
        }

        // FUNCTION NAME : ContinueAssemblyLine()
        // DESCRIPTION: 
        //		This function continues the stopped assembly line
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void ContinueAssemblyLine()
        {
            foreach (Employee e in workersList)
            {
                e.ContinueWorking();
            }
        }

        // FUNCTION NAME : Refill()
        // DESCRIPTION: 
        //		This function refills the almost empty bin with its initial amount of parts
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    Int
        public static void Refill()
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                string cmdText = $@"EXECUTE Materials_Refill {wID}";     // Procedure to refill the needed bins

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                conn.Close();
            }
        } 
    }
}
