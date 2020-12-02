using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WorkstationSimulator
{
    class Workstation
    {
        private static string[] employeeType = { "NE", "EE", "VEE" };   //NE: New Employee; EE: Experienced Employee; VEE: Very Experienced Employee
        private static string[] timeScaleDesc = { "1:1", "1:5", "1:10" };
        private static List<Employee> workersList;
        public static string connectionString = ConfigurationManager.ConnectionStrings["KanbanConnection"].ConnectionString;
        static void Main(string[] args)
        {
            System.Console.WriteLine("------------------------------ Workstation Simulator ------------------------------");
            
            Configuration config = LoadCurrentConfigs();

            InstantiatingEmployees(config);                                           

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
        //	    NONE
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
                e.EmployeeID = e.EmployeeType + i.ToString();
                workersList.Add(e);
            }
            for (int i = 1; i <= config.NoOfExperienced; i++)
            {
                // Experienced Employees
                Employee e = new Employee("EE", scale);               
                e.EmployeeID = e.EmployeeType + i.ToString();
                workersList.Add(e);
            }
            for (int i = 1; i <= config.NoOfSuper; i++)
            {
                // Very Experienced Employees
                Employee e = new Employee("VEE", scale);                
                e.EmployeeID = e.EmployeeType + i.ToString();
                workersList.Add(e);
            }
            
        }
    }
}
