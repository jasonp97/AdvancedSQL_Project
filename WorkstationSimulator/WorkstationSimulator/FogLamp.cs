/*
* FILE: FogLamp.cs
* PROJECT: PROG3070 - Final Project
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the functionalities that involve in instantiating
*              a foglamp object to hold the appropriate information related to a foglamp.
*/

using System;
using System.Data.SqlClient;

namespace WorkstationSimulator
{
    class FogLamp
    {
        public string LampNumber { get; set; }      //Ex: FL00000101
        public int WorkstationID { get; set; }
        public string TestUnitNo { get; set; }      //Ex: 1FLxxxxxxyy
        public string WorkerID { get; set; }        // string or int
        public string CompletedStatus { get; set; }
        public FogLamp(string lampNo, int wID, string testTrayID, string workerID, string status)
        {
            LampNumber = lampNo;
            WorkstationID = wID;
            TestUnitNo = testTrayID;
            WorkerID = workerID;
            CompletedStatus = status;
        }

        // FUNCTION NAME : SaveToDb()
        // DESCRIPTION: 
        //		This function saves the current fog lamp to database
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void SaveToDb()
        {
            using(SqlConnection conn = new SqlConnection(Workstation.connectionString))
            {               
                string cmdText = $@"INSERT INTO Test_Lamp
                                    VALUES ('{LampNumber}', {WorkstationID} , '{TestUnitNo}', '{WorkerID}', '{CompletedStatus}');";

                SqlCommand cmd = new SqlCommand(cmdText, conn);

                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
                conn.Close();
            }
        }
    }
}
