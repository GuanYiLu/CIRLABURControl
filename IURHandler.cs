using System;
namespace CIRLABURControl
{
    interface IURHandler
    {
        float[] URposeToFloatArray(string pose);
        string FloatArrayToURPose(float[] poseArray);
    }
}
