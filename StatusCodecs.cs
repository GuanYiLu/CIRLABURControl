using System;

namespace CIRLABURControl
{
    internal class StatusCode
    {
        internal static byte[] StartFreedrive { get => new byte[]{ 11 }; }
        internal static byte[] EndFreedrive { get => new byte[] { 12 }; }
        
        internal static byte[] MoveJointWithCMD { get => new byte[] { 21 }; }
        /*  MoveJointWithCMD
         *  後續要先給URScript格式的List
         *  第一個值為角度
         *  第二個值為第幾軸數
         *  
         *  *Example* 角度:3.14, 軸數: 5
         *      (3.14,5)
        */
        internal static byte[] MovePoseWithCMD { get => new byte[] { 22 }; }
        /*  MoveJointWithCMD
         *  後續要先給URScript格式的Pose
         *  可以用URHandler.FloatArrayToURPose從C# Array to URScript Pose
        */
        internal static byte[] GripperOpen { get => new byte[] { 31 }; }
        internal static byte[] GripperClose { get => new byte[] { 32 }; }
        internal static byte[] GripperCloseMAX { get => new byte[] { 33 }; }
        internal static byte[] ForceMode { get => new byte[] { 41 }; }
        /*  ForceMode
         *  後續要先給URScript格式的List
         *  第一個值為第幾軸數
         *  第二個值為力量值(牛頓)
         *  
         *  *Example* 力度:10, 軸數: 5
         *      (5,10)
         */
        internal static byte[] EndForceMode { get => new byte[] { 42 }; }
    }
}
