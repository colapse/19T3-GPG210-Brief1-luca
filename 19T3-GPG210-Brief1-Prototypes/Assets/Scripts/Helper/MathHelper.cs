using System;
using UnityEngine;

namespace Helper
{
    public static class MathHelper
    {
        
        /** AngleDir: Determines if a target is on the left or right of an object.
         * @return: -1 if target is on the left, 0 if infron/behind, 1 if on the right
         * Copied from: https://forum.unity.com/threads/left-right-test-function.31420/
         */
        public static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);
		
            if (dir > 0f) {
                return 1;
            } else if (dir < 0f) {
                return -1;
            } else {
                return 0;
            }
        }

        public static bool Vector3ContainsNaN(Vector3 vector)
        {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
        }
        
        // original from https://answers.unity.com/questions/756538/mathfapproximately-with-a-threshold.html
        public static bool FApproximately(float a, float b, float threshold = -1)
        {
            //return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
            return threshold <= 0 ? Mathf.Approximately(a,b) : (((a < b)?(b - a):(a - b)) <= threshold);
        }

        public static bool Equals(this Vector3 v3, Vector3 other, float treshold)
        {
            return Vector3.SqrMagnitude(v3 - other) < treshold;
            //const double epsilon = 0.005;
            //Debug.Log(v3.magnitude+" "+other.magnitude+" < "+treshold);
            //return (Mathf.Abs(v3.magnitude - other.magnitude) < treshold);
        }
    
        // https://stackoverflow.com/questions/412019/math-optimization-in-c-sharp
        public static float Sigmoid(float value) {
            var k = (float)Math.Exp((double)value);
            return k / (1.0f + k);
        }
    }
}