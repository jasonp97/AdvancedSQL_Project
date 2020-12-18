/*
* FILE: Materials.cs
* PROJECT: PROG3070 - Final Project
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

        // FUNCTION NAME : DisplayAssemblyStatus()
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

            // Send data packet to Andon display
            Workstation.displaySkt.Send(data);
        }

        // FUNCTION NAME : PacketBuilder()
        // DESCRIPTION: 
        //		This function contructs a data packet of the workstation to send to Andon Display
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    byte[]
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

        // FUNCTION NAME : ReadPartsCount()
        // DESCRIPTION: 
        //		This function reads the part counts in all the material bins
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    byte[]
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
                
    }
}
