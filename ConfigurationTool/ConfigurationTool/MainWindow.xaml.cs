/*
* FILE: MainWindow.xaml.cs
* PROJECT: PROG3070 - Project Milestone 01
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 11/13/2020
* DESCRIPTION: This file includes a function that connects to the 
*              system database and updates all the settings / attributes
*              of the configuration table based on user inputs.
*              These configuration shall be used for the project simulation.
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigurationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["KanbanConnection"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
            LoadCurrentConfigs();
        }

        // FUNCTION NAME : UpdateConfig()
        // DESCRIPTION: 
        //		This function takes a connection string to the system database
        //      and update the settings / attributes of the configuration table,
        //      according to user inputs.
        // INPUTS :
        //	    string connectionString : connection string to the system database.
        // OUTPUTS: 
        //      Displays error messages if applicable.
        // RETURNS:
        //	    NONE
        public void UpdateConfig(string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Get timescale
                int timescale = 0;
                if (oneOneRatio.IsChecked == true)
                {
                    timescale = 0;
                }
                else if (oneFiveRatio.IsChecked == true)
                {
                    timescale = 1;
                }
                else if(oneTenRatio.IsChecked == true)
                {
                    timescale = 2;
                }
                // Sql query used for updating the configuration table.
                const string updateQuery = @"UPDATE Configuration
                                            SET HarnessQty=@Harness,ReflectorQty=@Reflector,HousingQty=@Housing,LensQty=@Lens,BulbQty=@Bulb,BezelQty=@Bezel,
                                            TimeScale=@TimeScale,AssemblyStationQty=@AssemblyStation,TestTrayQty=@TestTray,NoOfRookie=@NoOfRookie,
                                            NoOfExperienced=@NoOfExperience,NoOfSuper=@NoOfSuper
                                            WHERE ConfigID=1;";

                // Sql command for updating the configuration table
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Harness", HarnessQty.Text);
                updateCmd.Parameters.AddWithValue("@Reflector", ReflectorsQty.Text);
                updateCmd.Parameters.AddWithValue("@Housing", HousingQty.Text);
                updateCmd.Parameters.AddWithValue("@Lens", LensQty.Text);
                updateCmd.Parameters.AddWithValue("@Bulb", BulbQty.Text);
                updateCmd.Parameters.AddWithValue("@Bezel", BezelQty.Text);
                updateCmd.Parameters.AddWithValue("@TimeScale", timescale);
                updateCmd.Parameters.AddWithValue("@AssemblyStation", AssemblyStationQty.Text);
                updateCmd.Parameters.AddWithValue("@TestTray", TestTrayQty.Text);
                updateCmd.Parameters.AddWithValue("@NoOfRookie", NewWorkers.Text);
                updateCmd.Parameters.AddWithValue("@NoOfExperience", ExperiencedWorkers.Text);
                updateCmd.Parameters.AddWithValue("@NoOfSuper", SuperExpWorkers.Text);

                try
                {
                    // Open connection to database and execute updating command.

                    conn.Open
                    ();
                    updateCmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show
                    (ex.ToString());
                }
            }
        }

        // FUNCTION NAME : GetCurrentConfigs()
        // DESCRIPTION: 
        //		This function returns a list of current configuration settings.
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    List<int>: List of all the current configuration settings.
        public List<int> GetCurrentConfigs()
        {
            // Sql query to retrieve the current configuration settings.
            const string selectQuery = @"SELECT * FROM  Configuration";

            using (SqlConnection conn = new SqlConnection(connectionString)) // Establish connection to the system database.
            {
                SqlCommand selectCmd = new SqlCommand(selectQuery, conn);

                //For offline connection we will use  SqlDataAdapter class.  
                var adapter = new SqlDataAdapter
                {
                    SelectCommand = selectCmd
                };

                var dataTable = new DataTable();
                adapter.Fill(dataTable); // Retrieve a list of configuration settings as data table type.
                var configsList = DataTableToIntList(dataTable); // Convert data table to return a list of configuration settings.

                return configsList;
            }

        }


        // FUNCTION NAME : DataTableToIntList()
        // DESCRIPTION: 
        //		This function takes a datatable-type object, retrieves
        //      each row in the table as a specific configuration setting. 
        //      The entire data table will be returned as a list of configuration settings.
        // INPUTS :
        //	    DataTable mytable: The input data table to be converted.
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    List<int>: List of all the current configuration settings.
        private List<int> DataTableToIntList(DataTable mytable)
        {
            var configsList = new List<int>();

            // Convert a row into a list of configuration settings.
            foreach (DataRow row in mytable.Rows)
            {
                configsList.Add(Convert.ToInt32(row["ConfigID"]));
                configsList.Add(Convert.ToInt32(row["HarnessQty"]));
                configsList.Add(Convert.ToInt32(row["ReflectorQty"]));
                configsList.Add(Convert.ToInt32(row["HousingQty"]));
                configsList.Add(Convert.ToInt32(row["LensQty"]));
                configsList.Add(Convert.ToInt32(row["BulbQty"]));
                configsList.Add(Convert.ToInt32(row["BezelQty"]));
                configsList.Add(Convert.ToInt32(row["TimeScale"]));
                configsList.Add(Convert.ToInt32(row["AssemblyStationQty"]));
                configsList.Add(Convert.ToInt32(row["TestTrayQty"]));
                configsList.Add(Convert.ToInt32(row["NoOfRookie"]));
                configsList.Add(Convert.ToInt32(row["NoOfExperienced"]));
                configsList.Add(Convert.ToInt32(row["NoOfSuper"]));
            }
            return configsList;
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
        private void LoadCurrentConfigs()
        {
            var listOfConfigs = GetCurrentConfigs();
          
            //Extract configs data from list to display           
            HarnessQty.Text = listOfConfigs[1].ToString();
            ReflectorsQty.Text = listOfConfigs[2].ToString();
            HousingQty.Text = listOfConfigs[3].ToString();
            LensQty.Text = listOfConfigs[4].ToString();
            BulbQty.Text = listOfConfigs[5].ToString();
            BezelQty.Text = listOfConfigs[6].ToString();
            //Get timescale value
            int timescale = listOfConfigs[7];
            switch (timescale)
            {
                case 0:
                    oneOneRatio.IsChecked = true;
                    break;
                case 1:
                    oneFiveRatio.IsChecked = true;
                    break;
                case 2:
                    oneTenRatio.IsChecked = true;
                    break;
                default: break;
            }
            AssemblyStationQty.Text = listOfConfigs[8].ToString();
            TestTrayQty.Text = listOfConfigs[9].ToString();
            NewWorkers.Text = listOfConfigs[10].ToString();
            ExperiencedWorkers.Text = listOfConfigs[11].ToString();
            SuperExpWorkers.Text = listOfConfigs[12].ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            UpdateConfig(connectionString);
            MessageBox.Show("Setting is saved!");
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
