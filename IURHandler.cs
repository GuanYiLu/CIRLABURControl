using System;
namespace URControler2
{
    interface IURHandler
    {
        float[] URposeToFloatArray(string pose);
        string FloatArrayToURPose(float[] poseArray);
    }
}
