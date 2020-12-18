/*
* FILE: MainWindow.xaml.cs
* PROJECT: PROG3070 - Final Project
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/17/2020
* DESCRIPTION: This file includes the functionalities that involve in instantiating
*              a Kanban display, which contains information of all active workstations.
*/

using System;
using System.Windows;
using System.Windows.Media;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;

namespace AssemblyLineKanban
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool running;       // Flag to indicate if the Kanban is running or not
        private static string connectionString = ConfigurationManager.ConnectionStrings["KanbanConnection"].ConnectionString;
        private static Workstation workstation1, workstation2, workstation3;    // 3 desire workstations
        private ThreadStart workingThread;   //Working thread (reading data from database)
        private Thread thread;
        public MainWindow()
        {
            InitializeComponent();
            workstation1 = new Workstation();
            workstation2 = new Workstation();
            workstation3 = new Workstation();
            SetDataContext();

            running = true;

            //Set up background thread to establish socket connection
            workingThread = new ThreadStart(ReadingLoop);
            thread = new Thread(workingThread);
            thread.Start();
        }

        // FUNCTION NAME : StartWorkstation()
        // DESCRIPTION: 
        //		This function constantly reads data of all workstation in the database
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void ReadingLoop()
        {            
            while (running)
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
                {
                    const string cmdGetWorksationInfo = @"SELECT * FROM [Get_Quantity]";    // Return a table containing info of all workstation
                    SqlCommand cmd = new SqlCommand(cmdGetWorksationInfo, conn);

                    conn.Open();

                    try
                    {
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.HasRows)
                        {
                            // Read all the retrieved records
                            while (rd.Read())
                            {
                                // Update data to a corresponding workstation
                                ReadDataFromStation(rd["WorkStationID"].ToString(), rd);
                            }
                        }                        
                    }
                    catch
                    {
                        MessageBox.Show("Cannot read from database");
                    }

                    conn.Close();
                }
                Thread.Sleep(1000);     // Repeat the loop every 1 second                
            }           
        }

        // FUNCTION NAME : SetDataContext()
        // DESCRIPTION: 
        //		This function set data context of all the instances of workstations
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void SetDataContext()
        {
            // For station 1
            w1_Label.DataContext = workstation1;
            w1_status_Label.DataContext = workstation1;
            w1_statusLight.DataContext = workstation1;
            w1_order.DataContext = workstation1;
            w1_produced.DataContext = workstation1;
            w1_passed.DataContext = workstation1;
            w1_failed.DataContext = workstation1;
            w1_yield.DataContext = workstation1;

            // For station 2
            w2_Label.DataContext = workstation2;
            w2_status_Label.DataContext = workstation2;
            w2_statusLight.DataContext = workstation2;
            w2_order.DataContext = workstation2;
            w2_produced.DataContext = workstation2;
            w2_passed.DataContext = workstation2;
            w2_failed.DataContext = workstation2;
            w2_yield.DataContext = workstation2;

            // For station 3
            w3_Label.DataContext = workstation3;
            w3_status_Label.DataContext = workstation3;
            w3_statusLight.DataContext = workstation3;
            w3_order.DataContext = workstation3;
            w3_produced.DataContext = workstation3;
            w3_passed.DataContext = workstation3;
            w3_failed.DataContext = workstation3;
            w3_yield.DataContext = workstation3;
        }

        // FUNCTION NAME : ReadDataFromStation()
        // DESCRIPTION: 
        //		This function reads data of a specific workstation from the database
        // INPUTS :
        //	    id: string
        //      rd: SqlDataReader
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void ReadDataFromStation(string id, SqlDataReader rd)
        {
            if (id == "1")
            {
                // Update workstation 1
                workstation1.WorkstationLabel = "Workstation " + rd["WorkStationID"].ToString();
                if (rd["WorkstationStatus"].ToString() == "0")
                {
                    workstation1.WorkstationStatus = "Paused";
                    workstation1.BgColorStatus = Brushes.Red;
                }
                else
                {
                    workstation1.WorkstationStatus = "Active";
                    workstation1.BgColorStatus = Brushes.GreenYellow;
                }
                workstation1.OrderTarget = Convert.ToInt32(rd["OrderQty"]);
                workstation1.Produced = Convert.ToInt32(rd["LampQty"]);
                workstation1.Passed = Convert.ToInt32(rd["NumPassed"]);
                workstation1.Failed = Convert.ToInt32(rd["NumFailed"]);
                workstation1.Yield = Convert.ToDouble(rd["Yield"]);
            }
            else if (id == "2")
            {
                // Update workstation 2
                workstation2.WorkstationLabel = "Workstation " + rd["WorkStationID"].ToString();
                if (rd["WorkstationStatus"].ToString() == "0")
                {
                    workstation2.WorkstationStatus = "Paused";
                    workstation2.BgColorStatus = Brushes.Red;
                }
                else
                {
                    workstation2.WorkstationStatus = "Active";
                    workstation2.BgColorStatus = Brushes.GreenYellow;
                }
                workstation2.OrderTarget = Convert.ToInt32(rd["OrderQty"]);
                workstation2.Produced = Convert.ToInt32(rd["LampQty"]);
                workstation2.Passed = Convert.ToInt32(rd["NumPassed"]);
                workstation2.Failed = Convert.ToInt32(rd["NumFailed"]);
                workstation2.Yield = Convert.ToDouble(rd["Yield"]);
            }
            else if (id == "3")
            {
                // Update workstation 3
                workstation3.WorkstationLabel = "Workstation " + rd["WorkStationID"].ToString();
                if (rd["WorkstationStatus"].ToString() == "0")
                {
                    workstation3.WorkstationStatus = "Paused";
                    workstation3.BgColorStatus = Brushes.Red;
                }
                else
                {
                    workstation3.WorkstationStatus = "Active";
                    workstation3.BgColorStatus = Brushes.GreenYellow;
                }
                workstation3.OrderTarget = Convert.ToInt32(rd["OrderQty"]);
                workstation3.Produced = Convert.ToInt32(rd["LampQty"]);
                workstation3.Passed = Convert.ToInt32(rd["NumPassed"]);
                workstation3.Failed = Convert.ToInt32(rd["NumFailed"]);
                workstation3.Yield = Convert.ToDouble(rd["Yield"]);
            }
        }

        // FUNCTION NAME : OnWindowClosing()
        // DESCRIPTION: 
        //		This function stops the reading loop and terminates the background thread 
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            running = false;
            thread.Join();
        }        
    }
}
