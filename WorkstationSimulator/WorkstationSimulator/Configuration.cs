/*
* FILE: Configuration.cs
* PROJECT: PROG3070 - Project Milestone 02
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the functionalities that involve in modifying
*              the configuration settings (from the configuration table) for the simulation. 
*/

using System;

namespace WorkstationSimulator
{
    class Configuration
    {       
        public int HarnessQty { get; set; }
        public int ReflectorQty { get; set; }
        public int HousingQty { get; set; }
        public int LensQty { get; set; }
        public int BulbQty { get; set; }
        public int BezelQty { get; set; }
        public string TimeScale { get; set; }
        public int AssemblyStationQty { get; set; }
        public int TestTrayQty { get; set; }
        public int NoOfRookie { get; set; }
        public int NoOfExperienced { get; set; }
        public int NoOfSuper { get; set; }
        public Configuration()
        {

        }

        // FUNCTION NAME : ShowConfig()
        // DESCRIPTION: 
        //		This function shows all configuration of the workstation
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        public void ShowConfig()
        {
            Console.WriteLine("Harness Quantity: {0}", HarnessQty);
            Console.WriteLine("Reflector Quantity: {0}", ReflectorQty);
            Console.WriteLine("Housing Quantity: {0}", HousingQty);
            Console.WriteLine("Lens Quantity: {0}", LensQty);
            Console.WriteLine("Bulb Quantity: {0}", BulbQty);
            Console.WriteLine("Bezel Quantity: {0}", BezelQty);
            Console.WriteLine("Timescale: {0}", TimeScale);
            Console.WriteLine("Assembly Station Quantity: {0}", AssemblyStationQty);
            Console.WriteLine("Test Tray Quantity: {0}", TestTrayQty);
            Console.WriteLine("# of New Employees: {0}", NoOfRookie);
            Console.WriteLine("# of Experienced Employees: {0}", NoOfExperienced);
            Console.WriteLine("# of Very Experienced Employees: {0}", NoOfSuper);
        }
    }
}
