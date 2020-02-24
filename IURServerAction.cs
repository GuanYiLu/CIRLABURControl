using System;
using System.Threading.Tasks;

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
        void FreeDrive();
        void EndFreeDrive();
        void ForceMode(int JointNumber, float JointForce);
        void EndForceMode();
        void GripperOpen();
        void GripperClose();
        void TurnJoint(int Turns, float force, int joint);

    }
    interface IURServerActionAsync
    {
        System.Net.Sockets.NetworkStream Stream
        {
            get;
            set;
        }
        Task MoveAsync(float[] Poses);
        Task MoveJointAsync(int Joint, float Angle);
        Task FreeDriveAsync();
        Task EndFreeDriveAsync();
        Task ForceModeAsync(int JointNumber, float JointForce);
        Task EndForceModeAsync();
        Task GripperOpenAsync();
        Task GripperCloseAsync();
        Task TurnJointAsync(int Turns);
        Task TurnJointAsync(int Turns, float force, int joint);

    }
}