using UnityEngine;

namespace Helper
{
    public class MathHelper
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
    }
}