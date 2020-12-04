/*
* FILE: Workstation.cs
* PROJECT: PROG3070 - Project Milestone 02
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the functionalities that involve in instantiating
*              a workstation object to hold the appropriate information related to a workstation.
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WorkstationSimulator
{
    class Workstation
    {
        private static Configuration config;        // Configuration of this workstation
        private static int numOfWorkers = 0;        // Number of workers
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
            Console.Write("Specify workstation ID: ");
            wID = Int32.Parse(Console.ReadLine());
                     
            System.Console.WriteLine("------------------------------ Workstation {0} Simulator ------------------------------", wID);
            
            // We need to assign ID for the workstation

            config = LoadCurrentConfigs();

            RegisterWorkstation(config);

            // Initialize test tray
            currentTestTray = 0;
            positionInTray = 0;
            CreateTestTray();
            
            materialsBins = new Materials(config);

            InstantiatingEmployees(config);

            StartAssemblyLine();

            System.Console.ReadLine();           
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
        private static void InstantiatingEmployees(Configuration config)
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
        private void RegisterWorkstation(Configuration config)
        {
            numOfWorkers = config.NoOfRookie + config.NoOfExperienced + config.NoOfSuper;
            using (SqlConnection conn = new SqlConnection(Workstation.connectionString))
            {               
                string cmdText = $@"INSERT INTO WorkStation
                                    VALUES ({wID}, {numOfWorkers});";

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
            Console.WriteLine("Workers start working ...");
            foreach(Employee e in workersList)
            {
                e.StartWorking();
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
        public static int Refill(string material)
        {
            int value = 0;
            if(material == "Harness")
            {
                value = config.HarnessQty; 
            }
            else if(material == "Reflector")
            {
                value = config.ReflectorQty;
            }
            else if(material == "Housing")
            {
                value = config.HousingQty;
            }
            else if(material == "Lens")
            {
                value = config.LensQty;
            }
            else if(material == "Bulb")
            {
                value = config.BulbQty;
            }
            else if (material == "Bezel")
            {
                value = config.BezelQty;
            }
            return value;
        } 

    }
}
