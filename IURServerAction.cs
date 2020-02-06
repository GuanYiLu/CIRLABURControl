using System;
namespace CIRLABURControl
{
    interface IURServerAction
    {
        System.Net.Sockets.NetworkStream Stream
        {
            get;
            set;
        }
        void Move(float[] Poses);
        void MoveJoint(int Joint, float Angle);
        void FreeDrive(int time);
        void ForceMode(int JointNumber, float JointForce);
        void EndForceMode();
        void GripperOpen();
        void GripperClose();
        void TurnJoint(int Turns, float force, int joint);

    }
}