/*
* FILE: Employee.cs
* PROJECT: PROG3070 - Project Milestone 02
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the functionalities that involve in instanting
*              an employee / worker for a workstation. It holds the approriate 
*              information to create a unique worker.  
*/

using System;
using System.Timers;
using System.Data.SqlClient;

namespace WorkstationSimulator
{
    class Employee
    {
        private static string[] workerTypeDesc = { "New Employee", "Experienced Employee", "Very Experience Employee" };
        private Timer timer;
        private int interval;          // This is simulated time a worker need to assemble a product
        public int Timescale { get; set; }   // This is the time simulated in real world
        public string EmployeeID { get; set; }
        public int WorkstationID { get; set; }
        public string EmployeeType { get; set; }
        public Employee(string type, int scale)
        {
            switch (type){
                case "NE":
                    EmployeeType = workerTypeDesc[0]; break;
                case "EE": 
                    EmployeeType = workerTypeDesc[1]; break;
                case "VEE":
                    EmployeeType = workerTypeDesc[2]; break;
                default: break;
            }            
            WorkstationID = Workstation.wID;
            Timescale = scale;            
        }
        
        ~Employee()
        {
            Console.WriteLine("{0} Done!", EmployeeID);
            timer.Stop();
            timer.Dispose();
        }


        // FUNCTION NAME : StartWorking()
        // DESCRIPTION: 
        //		This function starts the timer within a worker
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void StartWorking()
        {
            GenerateTimeInterval();
            timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }


        // FUNCTION NAME : Timer_Elapsed()
        // DESCRIPTION: 
        //		This function is triggered each time a worker finishes assemble a fog lamp
        // INPUTS :
        //	    object sender
        //      ElapsedEventArgs e
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Use SignalTime.
            DateTime time = e.SignalTime;

            // Create a fog lamp
            FogLamp fl = new FogLamp(Workstation.GenerateLampNumber(), Workstation.wID, Workstation.GenerateTestTrayID(), EmployeeID, "Pass");           
            Console.WriteLine("Fog lamp {0} was successfully assembled at: {1}, by {2}, for {3}s", fl.LampNumber , time, EmployeeID, (interval*Timescale)/1000);
           
            fl.SaveToDb();

            // Take parts for the next assembly
            Workstation.materialsBins.TakeParts();            
            
            // Update new interval time for the next fog lamp assemble
            timer.Stop();
            GenerateTimeInterval();
            timer.Interval = interval;
            timer.Start();
        }

        // FUNCTION NAME : GenerateTimeInterval()
        // DESCRIPTION: 
        //		This function generates a random time to simulate working time of an employee
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void GenerateTimeInterval()
        {
            Random rnd = new Random();
            if (EmployeeType == workerTypeDesc[0])
            {
                interval = (1000 * rnd.Next(81, 100))/Timescale;    //New Employee (81s -> 99s)
            }
            else if (EmployeeType == workerTypeDesc[1])
            {
                interval = (1000 * rnd.Next(54, 67))/Timescale;     //Experienced Employee (54s -> 66s)
            }
            else if (EmployeeType == workerTypeDesc[2])
            {
                interval = (1000 * rnd.Next(46, 57))/Timescale;     //Very Experienced Employee (46s -> 56s)
            }           
        }

        // FUNCTION NAME : GenerateTimeInterval()
        // DESCRIPTION: 
        //		This function saves worker into database
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void RegisterEmployee()
        {
            using (SqlConnection conn = new SqlConnection(Workstation.connectionString))
            {
                
                string cmdText = $@"INSERT INTO Worker
                                    VALUES ('{EmployeeID}', {WorkstationID} , '{EmployeeType}');";

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
