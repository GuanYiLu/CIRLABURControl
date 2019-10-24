using System;
using System.Net.Sockets;
using System.Text;

namespace URControler2
{
    public class URServerAction : IURServerAction
    {
        public URServerAction(NetworkStream stream)
        {
            if (stream == null)
            {
                throw new  ArgumentNullException("別讓進來的NetworkStream是null阿!");
            }
            Stream = stream;
        }
        public NetworkStream Stream { get; set; }

        public void EndForceMode()
        {
            Stream.Write(new byte[] { 44 },0,1);
        }
        public void ForceMode(int JointNumber, float JointForce)
        {
            byte[] forceStr = Encoding.UTF8.GetBytes($"({JointNumber.ToString()},{JointForce.ToString()})");
            Stream.Write(new byte[] { 4 },0,1);
            string want = "ForceMode";
            if (Read(100) == want)
            {
                Stream.Write(forceStr,0, forceStr.Length);
            }
            else
            {
                ThrowSomthing(want);
            }
        }
        public void FreeDrive()
        {
            Stream.Write(new byte[] { 1 },0,1);
            if (Stream.CanRead)
            {
                var returnStr = Read(100);
                if (returnStr == "freedrive_mode")
                {
                    Stream.Write(new byte[] { 2 },0,1);
                }
                else
                {
                    ThrowSomthing("freedrive_mode");
                }
                System.Threading.Thread.Sleep(1000);
                Stream.Write(new byte[] { 1 },0,1);
                _ = Read(100);
            }

        }
        public void FreeDrive(int time)
        {
            Stream.Write(new byte[] { 1 },0,1);
            if (Stream.CanRead)
            {
                var returnStr = Read(100);
                if (returnStr == "freedrive_mode")
                {
                    Stream.Write(new byte[] { 2 },0,1);
                }
                else
                {
                    ThrowSomthing("freedrive_mode");
                }
                System.Threading.Thread.Sleep(time);
                Stream.Write(new byte[] { 1 },0,1);
                _ = Read(100);
            }

        }
        public void GripperClose()
        {
            Stream.Write(new byte[]{ 3 },0,1);
        }
        public void GripperOpen()
        {
            Stream.Write(new byte[] { 33 },0,1);
        }
        public void Move(float[] Poses)
        {
            if (Poses.Length != 6)
            {
                throw new ArgumentException("給的pose應該要是6個才對。");
            }
            foreach (float Pose in Poses)
            {
                if (Pose == 0)
                {
                    throw new ArgumentException("數值不能有0，UR3很任性的。");
                }
            }
            IURHandler uRHandler = new URHandler();
            byte[] data = Encoding.UTF8.GetBytes(uRHandler.FloatArrayToURPose(Poses));

            Stream.Write(new byte[] { 11 },0,1);
            var returnStr = Read(100);
            if (returnStr == "move")
            {
                Stream.Write(data,0, data.Length);
            }
            else
            {
                ThrowSomthing("move");
            }
            
        }
        public void MoveJoint(int Joint, float Angle)
        {
            byte[] moveStr = Encoding.UTF8.GetBytes($"({Joint.ToString()},{Angle.ToString()})");
            Stream.Write(new byte[] { 2 }, 0, 1);
            string data = Read(100);
            string want = "moveJoint";
            if (data != want)
            {
                ThrowSomthing(want);
            }
            else
            {
                Stream.Write(moveStr, 0, moveStr.Length);
            }
        }
        public void TurnJoint(int Turns)
        {

            byte[] clockwise = Encoding.UTF8.GetBytes("(5,3.14)");
            byte[] counterclockwise = Encoding.UTF8.GetBytes("(5,-3.14)");

            bool isReadyToOpen = Turns > 0;
            Turns *= 2;
            if (!isReadyToOpen)
            {
                Turns = -Turns;
            }

            for (int i = 0; i < Turns; i++)
            {
                Stream.Write(new byte[] { 2 }, 0, 1);
                string data = Read(100);
                string want = "moveJoint";
                if (data != want)
                {
                    ThrowSomthing(want);
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        if (isReadyToOpen)
                        {
                            Stream.Write(clockwise, 0, clockwise.Length);
                        }
                        else
                        {
                            if (i != 0)
                                Stream.Write(counterclockwise, 0, counterclockwise.Length);
                        }
                        Stream.Write(new byte[] { 3 }, 0, 1);

                        
                    }
                    else
                    {
                        if (!isReadyToOpen)
                        {
                            Stream.Write(clockwise, 0, clockwise.Length);
                        }
                        else
                        {
                            Stream.Write(counterclockwise, 0, counterclockwise.Length);
                        }
                        EndForceMode();
                        Stream.Write(new byte[] { 33 }, 0, 1);
                    }
                }
            }

        }
        public void TurnJoint(int Turns,float force,int joint)
        {

            byte[] clockwise = Encoding.UTF8.GetBytes("(5,3.1416)");
            byte[] counterclockwise = Encoding.UTF8.GetBytes("(5,-3.1416)");
            
            bool isReadyToOpen = Turns > 0;
            Turns *= 2;
            if (  !  isReadyToOpen)
            {
                Turns = - Turns;
            }

            for (int i = 0; i < Turns; i++)
            {
                Stream.Write(new byte[] { 2 }, 0, 1);
                string data = Read(100);
                string want = "moveJoint";
                if (data != want)
                {
                    ThrowSomthing(want);
                }
                else
                {
                    if (i%2 == 0)
                    {
                        if (isReadyToOpen)
                        {
                            Stream.Write(clockwise, 0, clockwise.Length);
                        }
                        else
                        {
                            Stream.Write(counterclockwise, 0, counterclockwise.Length);   
                        }
                        Stream.Write(new byte[] { 3 }, 0, 1);
                        
                        ForceMode(joint, force);
                    }
                    else
                    {
                        if ( ! isReadyToOpen)
                        {
                            Stream.Write(clockwise, 0, clockwise.Length);
                        }
                        else
                        {
                            Stream.Write(counterclockwise, 0, counterclockwise.Length);
                        }
                        EndForceMode();
                        Stream.Write(new byte[] { 33 }, 0, 1);
                    }
                }
            }
           
        }
        string Read(int b)
        {
            byte[] myReadBuffer = new byte[b];
            int numberOfBytesRead = Stream.Read(myReadBuffer, 0, myReadBuffer.Length);
            string returnStr = Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead);
            return returnStr;
        }
        void ThrowSomthing(string e)
        {
            throw new System.InvalidProgramException($"完了．．UR3沒有正常回覆{e}字串喔");
        }
    }
}
