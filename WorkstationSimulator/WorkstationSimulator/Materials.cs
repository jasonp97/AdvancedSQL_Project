/*
* FILE: Materials.cs
* PROJECT: PROG3070 - Project Milestone 02
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the functionalities that involve in instanting
*              the materials that make up a fog lamp. It holds the approriate 
*              information about every materials along with their capacities within a bin.  
*/

using System;
using System.Data.SqlClient;
using System.Text;

namespace WorkstationSimulator
{
    class Materials
    {
        private const int ALERT_LEVEL = 5;
        private int currentHarness;
        private int currentReflector;
        private int currentHousing;
        private int currentLens;
        private int currentBulb;
        private int currentBezel;
        public Materials()
        {
            // Default init
            currentHarness = 0;
            currentReflector = 0;
            currentHousing = 0;
            currentLens = 0;
            currentBulb = 0;
            currentBezel = 0;
        }
        public Materials(Configuration config)
        {
            // Config init
            currentHarness = config.HarnessQty;
            currentReflector = config.ReflectorQty;
            currentHousing = config.HousingQty;
            currentLens = config.LensQty;
            currentBulb = config.BulbQty;
            currentBezel = config.BezelQty;
        }

        // FUNCTION NAME : TakeParts()
        // DESCRIPTION: 
        //		This function deducts 1 unit in each bin for all parts
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void TakeParts()
        {
            // Deduct 1 unit in each bin for all parts
            currentHarness -= 1;
            currentReflector -= 1;
            currentHousing -= 1;
            currentLens -= 1;
            currentBulb -= 1;
            currentBezel -= 1;
            //CheckParts();
        }

        // FUNCTION NAME : ShowParts()
        // DESCRIPTION: 
        //		This function shows all parts available (in console app)
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void ShowParts()
        {
            Console.WriteLine("Harness bin: {0}", currentHarness);
            Console.WriteLine("Reflector bin: {0}", currentReflector);
            Console.WriteLine("Housing bin: {0}", currentHousing);
            Console.WriteLine("Lens bin: {0}", currentLens);
            Console.WriteLine("Bulb bin: {0}", currentBulb);
            Console.WriteLine("Bezel bin: {0}", currentBezel);
        }

        // FUNCTION NAME : ShowPartsUI()
        // DESCRIPTION: 
        //		This function shows all parts available (in GUI app)
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void DisplayAssemblyStatus()
        {
            byte[] data = PacketBuilder();
            Workstation.displaySkt.Send(data);
        }

        private byte[] PacketBuilder()
        {
            ReadPartsCount();   // Update new parts count
            string packetString = "";
            packetString += currentHarness.ToString() +
                            "|" +
                            currentReflector.ToString() +
                            "|" +
                            currentHousing.ToString() +
                            "|" +
                            currentLens.ToString() +
                            "|" +
                            currentBulb.ToString() +
                            "|" +
                            currentBezel.ToString() +
                            "|" +
                            Workstation.workstationStatus.ToString();
            return Encoding.ASCII.GetBytes(packetString);
        }

        private void ReadPartsCount()
        {
            // Read part count
            using (SqlConnection conn = new SqlConnection(Workstation.connectionString))
            {
                string cmdText = $@"SELECT HarnessQty, ReflectorQty, HousingQty, LensQty, BulbQty, BezelQty
                                    FROM WorkStation
                                    WHERE WorkStationID = {Workstation.wID}";
                
                SqlCommand cmd = new SqlCommand(cmdText, conn);

                conn.Open();
                try
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        // Read all the retrieved records
                        while (rd.Read())
                        {
                            currentHarness = Convert.ToInt32(rd["HarnessQty"]);
                            currentReflector = Convert.ToInt32(rd["ReflectorQty"]);
                            currentHousing = Convert.ToInt32(rd["HousingQty"]);
                            currentLens = Convert.ToInt32(rd["LensQty"]);
                            currentBulb = Convert.ToInt32(rd["BulbQty"]);
                            currentBezel = Convert.ToInt32(rd["BezelQty"]);
                        }
                    }
                    else
                    {
                        // Display error if no record found
                        Console.WriteLine("No Data Found!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot read parts count from database!");
                }

                conn.Close();
            }
        }


        // FUNCTION NAME : CheckParts()
        // DESCRIPTION: 
        //		This function raises alert if any part amount is less than 5 
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        //private void CheckParts()
        //{
        //    if(currentHarness <= ALERT_LEVEL)
        //    {
        //        Console.WriteLine("Harness card is removed");
        //        currentHarness += Workstation.Refill("Harness");
        //    }
        //    if(currentReflector <= ALERT_LEVEL)
        //    {
        //        Console.WriteLine("Reflector card is removed");
        //        currentReflector += Workstation.Refill("Reflector");
        //    }
        //    if(currentHousing <= ALERT_LEVEL)
        //    {
        //        Console.WriteLine("Housing card is removed");
        //        currentHousing += Workstation.Refill("Housing");
        //    }
        //    if (currentLens <= ALERT_LEVEL)
        //    {
        //        Console.WriteLine("Lens card is removed");
        //        currentLens += Workstation.Refill("Lens");
        //    }
        //    if (currentBulb <= ALERT_LEVEL)
        //    {
        //        Console.WriteLine("Bulb card is removed");
        //        currentBulb += Workstation.Refill("Bulb");
        //    }
        //    if (currentBezel <= ALERT_LEVEL)
        //    {
        //        Console.WriteLine("Bezel card is removed");
        //        currentBezel += Workstation.Refill("Bezel");
        //    }
        //}
        
    }
}
