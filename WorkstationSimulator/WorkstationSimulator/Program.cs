/*
* FILE: Program.cs
* PROJECT: PROG3070 - Project Milestone 02
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/03/2020
* DESCRIPTION: This file includes the functionalities that involve in creating
*              a workstation simulation. This simulation is made as a Console app
*              that provides the simulated timing surrounding the creation of a fog lamp.
*/

namespace WorkstationSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Workstation ws = new Workstation();
            ws.StartWorkstation();
           
        }
    }
}
