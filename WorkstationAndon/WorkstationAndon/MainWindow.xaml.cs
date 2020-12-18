/*
* FILE: MainWindow.xaml.cs
* PROJECT: PROG3070 - Final Project
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/17/2020
* DESCRIPTION: This file includes the functionalities that involve in instantiating
*              an Andon display that shows the connected workstation information
*/

using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Net.Sockets;
using System.Net;

namespace WorkstationAndon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Buffer for reading data.
        private static byte[] data_bytes = new byte[1024];
        private string wIP;         // Workstation IP
        private int wID;            // Workstation ID
        private static int wStatus; // Workstation status   (0: Disconnected/Paused;    1: Connected/Active)
        private static Socket ClientSocket;            //Client socket of the current client
        private const Int32 baseport = 15000;          // Set the base port is 15000.               
        private const int ALERT_LEVEL = 5;        
        private static Workstation workstation = new Workstation();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = workstation;
            
        }

        // FUNCTION NAME : BtnConnect_Click()
        // DESCRIPTION: 
        //		This function connects the Andon to the desired workstation
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            wIP = workstationIP.Text;
            wID = Int32.Parse(workstationID.Text);

            // Connect Andon to workstation
            await Task.Run(() => ConnectToWorkstation(wIP));
        }

        // FUNCTION NAME : ConnectToWorkstation()
        // DESCRIPTION: 
        //		This function connects the Andon to the desired workstation
        // INPUTS :
        //	    workstation_IP: string
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private void ConnectToWorkstation(string workstation_IP)
        {
            try
            {
                //Create a new socket
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                int port = baseport + wID;

                // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                ClientSocket.Connect(IPAddress.Parse(workstation_IP), port);                
            }
            catch (SocketException e)
            {
                MessageBox.Show("Cannot connect to this workstation!");
                workstation.BgColorStatus = Brushes.Red;             
            }

            if (ClientSocket.Connected)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    workstationTitle.Content = "Workstation #" + wID.ToString();                   
                });

                workstation.BgColorStatus = Brushes.GreenYellow;
                workstation.Status = "Connected";

                ClientSocket.BeginReceive(data_bytes, 0, data_bytes.Length, SocketFlags.None, new AsyncCallback(ReceiveData), ClientSocket);
            }
        }

        // FUNCTION NAME : ReceiveData()
        // DESCRIPTION: 
        //		This function receives the incoming data from the workstation
        // INPUTS :
        //	    NONE
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private static void ReceiveData(IAsyncResult status_result)
        {
            try
            {
                int received_bytes = 0;

                //Get the number of bytes received and put them in a temporary buffer.
                received_bytes = ClientSocket.EndReceive(status_result);
                string data_packet = null;
                data_packet = Encoding.ASCII.GetString(data_bytes, 0, received_bytes); // Store the received data as a string.                        

                // Parse the received data_packet into meaningful components
                DisplayData(data_packet);

                //Continue to receive data
                ClientSocket.BeginReceive(data_bytes, 0, data_bytes.Length, SocketFlags.None, ReceiveData, ClientSocket);
            }
            catch (Exception e)
            {
                MessageBox.Show("Workstation is closed");

                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close();                
            }
        }

        // FUNCTION NAME : DisplayData()
        // DESCRIPTION: 
        //		This function displays the incoming data from the workstation
        // INPUTS :
        //	    data: string
        // OUTPUTS: 
        //      NONE
        // RETURNS:
        //	    NONE
        private static void DisplayData(string data)
        {
            // Parse raw data into meaningfil data 
            char[] separator = {'|'};
            string[] dataBuffer = data.Split(separator);
            workstation.CurrentHarness = Int32.Parse(dataBuffer[0].Trim());     // Harness
            workstation.CurrentReflector = Int32.Parse(dataBuffer[1].Trim());   // Reflector
            workstation.CurrentHousing = Int32.Parse(dataBuffer[2].Trim());     // Housing
            workstation.CurrentLens = Int32.Parse(dataBuffer[3].Trim());        // Lens
            workstation.CurrentBulb = Int32.Parse(dataBuffer[4].Trim());        // Bulb
            workstation.CurrentBezel = Int32.Parse(dataBuffer[5].Trim());       // Bezel
            wStatus = Int32.Parse(dataBuffer[6].Trim());                        // Workstation status (Active, Paused)
            
            if (wStatus == 0)
            {
                // Paused status
                workstation.Status = "Paused";
                workstation.BgColorStatus = Brushes.Red;
            }
            else if (wStatus == 1)
            {
                // Active status
                workstation.Status = "Active";
                workstation.BgColorStatus = Brushes.GreenYellow;
            }            

            // Check if any part count is less than 5
            /********** HARNESS **********/
            if (workstation.CurrentHarness <= ALERT_LEVEL)
            {
                workstation.BgColorHarness = Brushes.Red;
            }
            else
            {
                workstation.BgColorHarness = Brushes.Transparent;
            }

            /********** REFLECTOR **********/
            if (workstation.CurrentReflector <= ALERT_LEVEL)
            {
                workstation.BgColorReflector = Brushes.Red;
            }
            else
            {
                workstation.BgColorReflector = Brushes.Transparent;
            }

            /********** HOUSING **********/
            if (workstation.CurrentHousing <= ALERT_LEVEL)
            {
                workstation.BgColorHousing = Brushes.Red;
            }
            else
            {
                workstation.BgColorHousing = Brushes.Transparent;
            }

            /********** LENS **********/
            if (workstation.CurrentLens <= ALERT_LEVEL)
            {
                workstation.BgColorLens = Brushes.Red;
            }
            else
            {
                workstation.BgColorLens = Brushes.Transparent;
            }

            /********** BULB **********/
            if (workstation.CurrentBulb <= ALERT_LEVEL)
            {
                workstation.BgColorBulb = Brushes.Red;
            }
            else
            {
                workstation.BgColorBulb = Brushes.Transparent;
            }

            /********** BEZEL **********/
            if (workstation.CurrentBezel <= ALERT_LEVEL)
            {
                workstation.BgColorBezel = Brushes.Red;
            }
            else
            {
                workstation.BgColorBezel = Brushes.Transparent;
            }
        }
    }
}
