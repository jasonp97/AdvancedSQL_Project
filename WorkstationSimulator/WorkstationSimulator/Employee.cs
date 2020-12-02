using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WorkstationSimulator
{
    class Employee
    {
        private Timer timer;
        private int interval;          // This is simulated time a worker need to assemble a product
        public int Timescale { get; set; }   // This is the time simulated in real world
        public string EmployeeID { get; set; }
        public string EmployeeType { get; set; }
        public Employee(string type, int scale)
        {           
            EmployeeType = type;
            Timescale = scale;
            GenerateTimeInterval();
            timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();            
        }
        
        ~Employee()
        {
            Console.WriteLine("{0} Done!", EmployeeID);
            timer.Stop();
            timer.Dispose();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Use SignalTime.
            DateTime time = e.SignalTime;
            Console.WriteLine("A Fog lamp has been done at: {0}, by {1}, for {2}s", time, EmployeeID, (interval*Timescale)/1000);

            // Update new interval time for the next fog lamp assemble
            timer.Stop();
            GenerateTimeInterval();
            timer.Interval = interval;
            timer.Start();
        }

        private void GenerateTimeInterval()
        {
            Random rnd = new Random();
            if (EmployeeType == "NE")
            {
                interval = (1000 * rnd.Next(81, 100))/Timescale;    //New Employee (81s -> 99s)
            }
            else if (EmployeeType == "EE")
            {
                interval = (1000 * rnd.Next(54, 67))/Timescale;     //Experienced Employee (54s -> 66s)
            }
            else if (EmployeeType == "VEE")
            {
                interval = (1000 * rnd.Next(46, 57))/Timescale;     //Very Experienced Employee (46s -> 56s)
            }
            
        }
    }
}
