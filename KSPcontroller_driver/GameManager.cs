using System;
using System.Net;
using KRPC.Client;
using KRPC.Client.Services.Drawing;
using KRPC.Client.Services.KRPC;
using KRPC.Client.Services.RemoteTech;
using KRPC.Client.Services.SpaceCenter;

namespace KSPcontroller_driver
{
    public class GameManager
    {
        private Connection _connection;

        public GameManager()
        {
            _connection = new Connection(
                name: "KSP Controller",
                address: IPAddress.Parse("127.0.0.1"));
        }

        public double GetAltitude()
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            return vessel.Flight().SurfaceAltitude;
        }
        
        public double GetSpeed()
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            return vessel.Orbit.Speed;
        }
        
        public double GetApoapsis()
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            return vessel.Orbit.ApoapsisAltitude;
        }
        
        public double GetPeriapsis()
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            return vessel.Orbit.PeriapsisAltitude;
        }
        
        public double GetTargetDistance()
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            try
            {
                var position =  _connection.SpaceCenter().TargetBody.Position(vessel.ReferenceFrame);
                return Math.Sqrt(Math.Pow(position.Item1, 2) + Math.Pow(position.Item2, 2) + Math.Pow(position.Item3, 2));
            }
            catch (Exception e)
            {
                return 0;
            }
            
        }

        public void SetSAS(bool value)
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            vessel.Control.SAS = value;
        }
        
        public void SetRCS(bool value)
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            vessel.Control.RCS = value;
        }
        
        public void SetLight(bool value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Lights = value;
        }
        
        public void SetBreak(bool value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Brakes = value;
        }
        
        public void SetAbort(bool value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Abort = value;
        }
        
        public void SetGear(bool value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Gear = value;
        }
        
        
        
        public void SetThrottle(float value)
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            vessel.Control.Throttle = value;
        }
        
        public void SetPitch(float value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Pitch = value;
            control.InputMode = ControlInputMode.Additive;
        }
        
        public void SetRoll(float value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Roll = value;
            control.InputMode = ControlInputMode.Additive;
        }
        
        public void SetUp(float value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Up = value;
            control.InputMode = ControlInputMode.Additive;
        }
        
        public void SetForward(float value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Forward = value;
            control.InputMode = ControlInputMode.Additive;
        }
        
        public void SetRight(float value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Right = value;
            control.InputMode = ControlInputMode.Additive;
        }
        
        public void SetYaw(float value)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.Yaw = value;
            control.InputMode = ControlInputMode.Additive;
        }
        
        public void SetActionGroup(uint group, bool state)
        {
            var control = _connection.SpaceCenter().ActiveVessel.Control;
            control.SetActionGroup(group, state);
        }
        
        
        
        

        public void ActivateNextStage()
        {
            var vessel = _connection.SpaceCenter().ActiveVessel;
            vessel.Control.ActivateNextStage();
        }
    }
}