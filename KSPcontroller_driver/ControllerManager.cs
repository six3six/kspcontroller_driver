using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace KSPcontroller_driver
{
    public class ControllerManager
    {
        private SerialPort _serialPort;
        private GameManager _gameManager;

        public ControllerManager(string port, ref GameManager gameManager)
        {
            _serialPort = new SerialPort(port, 115200);
            _serialPort.DtrEnable = true;
            _serialPort.RtsEnable = true;
            _serialPort.Open();

            _gameManager = gameManager;

            Console.WriteLine("Waiting for handshake");
            bool hand = false;
            while (!hand)
            {
                var line = _serialPort.ReadLine();
                hand = line.Contains(Command.INT.ToString());
                Console.WriteLine(line);
            }

            Console.WriteLine("Controller said hello so we can play");
            SetAltitude(35383773);

            var updateGui = new Thread(UpdateGUI);
            var receiveCommand = new Thread(ReceiveCommand);
            updateGui.Start();
            receiveCommand.Start();
        }

        private void UpdateGUI()
        {
            while (true)
            {
                try
                {
                    SetAltitude((int) _gameManager.GetAltitude());
                    SetSpeed((int) _gameManager.GetSpeed());
                    SetApoapsis((int) _gameManager.GetApoapsis());
                    SetPeriapsis((int) _gameManager.GetPeriapsis());
                    SetTargetDistance((int) _gameManager.GetTargetDistance());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


                Thread.Sleep(200);
            }
        }

        public void SetAltitude(int value)
        {
            SendCommand(Command.ALT, value.ToString());
        }

        public void SetSpeed(int value)
        {
            SendCommand(Command.SPD, value.ToString());
        }

        public void SetApoapsis(int value)
        {
            SendCommand(Command.APO, value.ToString());
        }

        public void SetPeriapsis(int value)
        {
            SendCommand(Command.PER, value.ToString());
        }

        public void SetTargetDistance(int value)
        {
            SendCommand(Command.TDIS, value.ToString());
        }

        public void RefreshAll()
        {
            SendCommand(Command.FRC);
        }

        private void SendCommand(Command command, params string[] args)
        {
            string strArgs = "";
            foreach (var arg in args)
            {
                strArgs += ";" + arg;
            }

            _serialPort.WriteLine(command.ToString() + strArgs);
        }

        private void ReceiveCommand()
        {
            while (true)
            {
                Command command;
                var rawRequest = _serialPort.ReadLine();
                var textCommand = rawRequest.Split(";")[0];
                if (Enum.TryParse(textCommand, true, out command))
                {
                    if (!Enum.IsDefined(typeof(Command), command))
                    {
                        Console.WriteLine("Unknown command");
                        continue;
                    }
                }

                var args = rawRequest.Substring(rawRequest.IndexOf(";") + 1).Split(";");
                RunCommand(command, args);
            }
        }

        private void RunCommand(Command command, params string[] args)
        {
            Console.WriteLine(command + " run " + args[0]);

            switch (command)
            {
                case Command.ABT:
                    _gameManager.SetAbort(args[0].Contains("1"));
                    break;
                case Command.STG:
                    if (args[0] == "1") _gameManager.ActivateNextStage();
                    break;
                case Command.SAS:
                    _gameManager.SetSAS(args[0].Contains("1"));
                    break;
                case Command.RCS:
                    _gameManager.SetRCS(args[0].Contains("1"));
                    break;
                case Command.EQP:
                    _gameManager.SetGear(args[0].Contains("1"));
                    break;
                case Command.LGT:
                    _gameManager.SetLight(args[0].Contains("1"));
                    break;
                case Command.PBK:
                    _gameManager.SetBreak(args[0].Contains("1"));
                    break;
                case Command.THT:
                    _gameManager.SetThrottle(float.Parse(args[0].Replace('.', ',')));
                    break;
                case Command.YAW:
                    _gameManager.SetYaw(float.Parse(args[0].Replace('.', ',')));
                    break;
                case Command.ROL:
                    _gameManager.SetRoll(float.Parse(args[0].Replace('.', ',')));
                    break;
                case Command.PIT:
                    _gameManager.SetPitch(float.Parse(args[0].Replace('.', ',')));
                    break;
                case Command.FOR:
                    _gameManager.SetForward(float.Parse(args[0].Replace('.', ',')));
                    break;
                case Command.TUR:
                    _gameManager.SetRight(float.Parse(args[0].Replace('.', ',')));
                    break;
                case Command.ELV:
                    _gameManager.SetUp(float.Parse(args[0].Replace('.', ',')));
                    break;
                case Command.PSE:
                    RefreshAll();
                    break;
                case Command.CUS0:
                    _gameManager.SetActionGroup(0, args[0].Contains("1"));
                    break;
                case Command.CUS1:
                    _gameManager.SetActionGroup(1, args[0].Contains("1"));
                    break;
                case Command.CUS2:
                    _gameManager.SetActionGroup(2, args[0].Contains("1"));
                    break;
                case Command.CUS3:
                    _gameManager.SetActionGroup(3, args[0].Contains("1"));
                    break;
                case Command.CUS4:
                    _gameManager.SetActionGroup(4, args[0].Contains("1"));
                    break;
                case Command.CUS5:
                    _gameManager.SetActionGroup(1, args[0].Contains("1"));
                    break;
                case Command.CUS6:
                    _gameManager.SetActionGroup(6, args[0].Contains("1"));
                    break;
                case Command.CUS7:
                    _gameManager.SetActionGroup(7, args[0].Contains("1"));
                    break;
                case Command.CUS8:
                    _gameManager.SetActionGroup(8, args[0].Contains("1"));
                    break;
                case Command.CUS9:
                    _gameManager.SetActionGroup(9, args[0].Contains("1"));
                    break;
                    
            }
        }


        enum Command
        {
            INT,
            TEST,
            MEM,
            BRK,
            PSE,
            STG,
            TSPD,
            TSLW,
            ABT,
            LGT,
            EQP,
            PBK,
            RCS,
            SAS,
            THT,
            ROL,
            YAW,
            PIT,
            FOR,
            TUR,
            ELV,
            FRC,
            SPD,
            APO,
            PER,
            TDIS,
            BAR,
            ALT,
            CUS0,
            CUS1,
            CUS2,
            CUS3,
            CUS4,
            CUS5,
            CUS6,
            CUS7,
            CUS8,
            CUS9
        }
    }
}