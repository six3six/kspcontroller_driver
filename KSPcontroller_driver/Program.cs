using System;
using System.IO.Ports;
using System.Net;
using KRPC.Client;
using KRPC.Client.Services.KRPC;

namespace KSPcontroller_driver
{
    class Program
    {
        static void Main(string[] args)
        {
            //string port = GetPort();
            string port = "COM4";
            GameManager gameManager = new GameManager();
            ControllerManager controllerManager = new ControllerManager(port, ref gameManager);
            
        }

        private static string GetPort()
        {
            while (true)
            {
                Console.Write("Entrez le port de la manette : ");
                string port = Console.ReadLine();
                try
                {
                    SerialPort serialPort = new SerialPort(port);
                    serialPort.Open();
                    serialPort.Close();
                    return port;
                }
                catch (Exception e)
                { 
                    Console.WriteLine();
                    Console.WriteLine(e);
                }
            }
        }
    }
}