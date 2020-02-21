using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CIRLABURControl
{
    public class URServerAction : IURServerActionAsync,IURServerAction
    {
        public NetworkStream Stream { get; set; }
        public URServerAction(NetworkStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("別讓進來的NetworkStream是null阿!");
            Stream = stream;
        }

        public void FreeDrive()
        {
            Stream.Write(StatusCode.StartFreedrive, 0, 1);
            StreamRead(100, "StartFreedrive");
            StreamRead(100, "DoneStartFreedrive");
        }
        public void EndFreeDrive()
        {
            Stream.Write(StatusCode.EndFreedrive, 0, 1);
            StreamRead(100, "EndFreedrive");
            StreamRead(100, "DoneEndFreedrive");
        }
        public void Move(float[] Poses)
        {
            if (Poses.Length != 6)
                throw new ArgumentException("給的pose應該要是6個才對。");
            foreach (float Pose in Poses)
                if (Pose == 0)
                    throw new ArgumentException("數值不能有0，UR3很任性的。");

            byte[] data = Encoding.UTF8.GetBytes(URHandler.FloatArrayToURPose(Poses));

            Stream.Write(StatusCode.MovePoseWithCMD, 0, 1);

            StreamRead(100,"Move");
            Stream.Write(data, 0, data.Length);
            StreamRead(100, "DoneMove");
        }
        public void MoveJoint(int Joint, float Angle)
        {
            byte[] moveStr = Encoding.UTF8.GetBytes($"({Joint.ToString()},{Angle.ToString()})");
            Stream.Write(StatusCode.MoveJointWithCMD, 0, 1);
            StreamRead(100, "MoveJoint");
            Stream.Write(moveStr, 0, moveStr.Length);
            StreamRead(100, "DoneMoveJoint");
        }
        public void TurnJoint(int Turns)
        {
            int joint = 5;
            float clockwise = 3.1416f;
            float counterclockwise = -3.1416f;

            bool isReadyToOpen = Turns > 0;
            Turns *= 2;

            if (!isReadyToOpen)
                Turns = -Turns;

            for (int i = 0; i < Turns; i++)
            {
                if (i % 2 == 0)
                {
                    if (isReadyToOpen)
                        MoveJoint(joint, clockwise);
                    else
                        if (i != 0)
                            MoveJoint(joint, counterclockwise);
                    GripperClose();
                }
                else
                {
                    if (!isReadyToOpen)
                        MoveJoint(joint, clockwise);
                    else
                        MoveJoint(joint, counterclockwise);
                    GripperOpen();
                }
            }

        }
        public void TurnJoint(int Turns, float force, int forceJoint)
        {
            int joint = 5;
            float clockwise = 3.1416f;
            float counterclockwise = -3.1416f;

            bool isReadyToOpen = Turns > 0;
            Turns *= 2;
            if (!isReadyToOpen)
                Turns = -Turns;

            for (int i = 0; i < Turns; i++)
            {
                if (i % 2 == 0)
                {
                    if (isReadyToOpen)
                        MoveJoint(joint, clockwise);
                    else
                        MoveJoint(joint, counterclockwise);
                    GripperClose();
                    ForceMode(forceJoint, force);
                }
                else
                {
                    if (!isReadyToOpen)
                        MoveJoint(joint, clockwise);
                    else
                        MoveJoint(joint, counterclockwise);
                    EndForceMode();
                    GripperOpen();
                }
            }
        }
        public void GripperOpen()
        {
            Stream.Write(StatusCode.GripperOpen, 0, 1);
            StreamRead(100, "GripperOpen");
            StreamRead(100, "DoneGripperOpen");
        }
        public void GripperClose()
        {
            Stream.Write(StatusCode.StartFreedrive, 0, 1);
            StreamRead(100, "GripperClose");
            StreamRead(100, "DoneGripperClose");
        }
        public void GripperCloseMAX()
        {
            Stream.Write(StatusCode.GripperCloseMAX, 0, 1);
            StreamRead(100, "GripperCloseMAX");
            StreamRead(100, "DoneGripperCloseMAX");
        }
        public void ForceMode(int JointNumber, float JointForce)
        {
            byte[] forceStr = Encoding.UTF8.GetBytes($"({JointNumber.ToString()},{JointForce.ToString()})");
            Stream.Write(StatusCode.ForceMode, 0, 1);
            StreamRead(100, "ForceMode");
            Stream.Write(forceStr, 0, forceStr.Length);
            StreamRead(100, "DoneForceMode");
        }
        public void EndForceMode()
        {
            Stream.Write(StatusCode.EndForceMode, 0, 1);
            StreamRead(100, "EndForceMode");
            StreamRead(100, "DoneEndForceMode");
        }
        
        string StreamRead(int bufferNumber, string target)
        {
            if (Stream.CanRead){
                byte[] myReadBuffer = new byte[bufferNumber];
                int numberOfBytesRead = Stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                string actual = Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead);
                if (target != actual)
                    ThrowSomthing(target);
                return actual;
            }else{
                return "";
            }
        }
        

        // Async...
        
        
        public async Task FreeDriveAsync()
        {
            await Stream.WriteAsync(StatusCode.StartFreedrive, 0, 1).ConfigureAwait(false);
            await StreamReadAsync(100, "startFreedrive").ConfigureAwait(false);
        }
        public async Task EndFreeDriveAsync()
        {
            await Stream.WriteAsync(StatusCode.EndFreedrive, 0, 1).ConfigureAwait(false);
            await StreamReadAsync(100, "endFreedrive").ConfigureAwait(false);
        }
        public async Task FreeDriveAsync(int time)
        {
            await FreeDriveAsync().ConfigureAwait(false);
            System.Threading.Thread.Sleep(time);
            await EndFreeDriveAsync().ConfigureAwait(false);
        }
        public async Task MoveAsync(float[] Poses)
        {
            if (Poses.Length != 6)
                throw new ArgumentException("給的pose應該要是6個才對。");
            foreach (float Pose in Poses)
                if (Pose == 0)
                    throw new ArgumentException("數值不能有0，UR3很任性的。");

            byte[] data = Encoding.UTF8.GetBytes(URHandler.FloatArrayToURPose(Poses));

            await Stream.WriteAsync(StatusCode.MovePoseWithCMD, 0, 1).ConfigureAwait(false);

            string target = "move";
            await StreamReadAsync(100, target).ConfigureAwait(false);
            await Stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
        }
        public async Task MoveJointAsync(int Joint, float Angle)
        {
            byte[] moveStr = Encoding.UTF8.GetBytes($"({Joint.ToString()},{Angle.ToString()})");
            await Stream.WriteAsync(StatusCode.MoveJointWithCMD, 0, 1).ConfigureAwait(false);
            string target = "moveJoint";
            await StreamReadAsync(100, target).ConfigureAwait(false);
            await Stream.WriteAsync(moveStr, 0, moveStr.Length).ConfigureAwait(false);
        }
        public async Task TurnJointAsync(int Turns, float force, int joint)
        {
            byte[] clockwise = Encoding.UTF8.GetBytes("(5,3.1416)");
            byte[] counterclockwise = Encoding.UTF8.GetBytes("(5,-3.1416)");

            bool isReadyToOpen = Turns > 0;
            Turns *= 2;
            if (!isReadyToOpen)
                Turns = -Turns;

            for (int i = 0; i < Turns; i++)
            {
                await Stream.WriteAsync(StatusCode.MoveJointWithCMD, 0, 1).ConfigureAwait(false);
                string target = "moveJoint";
                await StreamReadAsync(100, target).ConfigureAwait(false);
                if (i % 2 == 0)
                {
                    if (isReadyToOpen)
                        await Stream.WriteAsync(clockwise, 0, clockwise.Length).ConfigureAwait(false);
                    else
                        await Stream.WriteAsync(counterclockwise, 0, counterclockwise.Length).ConfigureAwait(false);
                    await GripperCloseAsync().ConfigureAwait(false);
                    await ForceModeAsync(joint, force).ConfigureAwait(false);
                }
                else
                {
                    if (!isReadyToOpen)
                        await Stream.WriteAsync(clockwise, 0, clockwise.Length).ConfigureAwait(false);
                    else
                        await Stream.WriteAsync(counterclockwise, 0, counterclockwise.Length).ConfigureAwait(false);
                    await EndForceModeAsync().ConfigureAwait(false);
                    await GripperOpenAsync().ConfigureAwait(false);
                }
            }
        }
        public async Task GripperOpenAsync()
        {
            await Stream.WriteAsync(StatusCode.GripperOpen, 0, 1).ConfigureAwait(false);
            await StreamReadAsync(100, "GripperOpen").ConfigureAwait(false);
        }
        public async Task GripperCloseAsync()
        {
            await Stream.WriteAsync(StatusCode.GripperClose, 0, 1).ConfigureAwait(false);
        }
        public async Task GripperCloseMAXAsync()
        {
            await Stream.WriteAsync(StatusCode.GripperCloseMAX, 0, 1).ConfigureAwait(false);
        }
        public async Task ForceModeAsync(int JointNumber, float JointForce)
        {
            byte[] forceStr = Encoding.UTF8.GetBytes($"({JointNumber.ToString()},{JointForce.ToString()})");
            await Stream.WriteAsync(StatusCode.ForceMode, 0, 1).ConfigureAwait(false);
            string target = "ForceMode";

            await StreamReadAsync(100, target).ConfigureAwait(false);
            await Stream.WriteAsync(forceStr, 0, forceStr.Length).ConfigureAwait(false);
        }
        public async Task EndForceModeAsync()
        {
            await Stream.WriteAsync(StatusCode.EndForceMode, 0, 1).ConfigureAwait(false);
        }
        async Task<string> StreamReadAsync(int bufferNumber, string target)
        {
            if (Stream.CanRead)
            {
                byte[] myReadBuffer = new byte[bufferNumber];
                int numberOfBytesRead = await Stream.ReadAsync(myReadBuffer, 0, myReadBuffer.Length).ConfigureAwait(false);
                string actual = Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead);
                if (target != actual)
                    ThrowSomthing(target);
                return actual;
            }
            else
            {
                return "";
            }
        }



        void ThrowSomthing(string e)
        {
            throw new System.InvalidProgramException($"完了．．UR3沒有正常回覆{e}字串喔");
        }
    }
}
