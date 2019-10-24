using System;
using System.Collections.Generic;

namespace URControler2
{
    public class URHandler : IURHandler
    {


        public float[] URposeToFloatArray(string pose)

        {
            var poseArray = pose.Split(',');
            try
            {
                poseArray[0] = poseArray[0].Substring(2, poseArray[0].Length - 2);
                poseArray[poseArray.Length - 1] = poseArray[poseArray.Length - 1].Substring(0, poseArray[poseArray.Length - 1].Length - 1);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                return new float[] { };
            }

            List<float> poseList = new List<float>();
            foreach (string p in poseArray)
            {
                try
                {
                    poseList.Add(float.Parse(p));
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                    return new float[] { };
                }
            }
            return poseList.ToArray();
        }

        public string FloatArrayToURPose(float[] poseArray)
        {
            string pose = "(";
            foreach (float p in poseArray)
            {
                pose += p.ToString() + ",";
            }

            pose = pose.Substring(0, pose.Length - 2);
            pose += ")";
            return pose;

        }

        //public float[] URposeToFloatArray(string pose)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
