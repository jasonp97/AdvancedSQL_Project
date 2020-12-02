using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void showConfig()
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
