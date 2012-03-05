using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace control
{
    class util
    {
        //calculate angle between 2 point and the x-axis and return its degree value
        public static float Xangle(Vector3 v1, Vector3 v2)
        {
            return toDegree((float)Math.Atan((v1.Y-v2.Y)/(v1.X-v2.X)));
        }

        //calculate angle between 2 point and the z-axis and return its degree value
        public static float Zangle(Vector3 v1, Vector3 v2)
        {
            return toDegree((float)Math.Atan((v1.Y - v2.Y) / (v1.X - v2.X)));
        }

        //get angle between 3 points
        public static float GetAngle(Vector3 a, Vector3 b, Vector3 c)
        {
            float a_ = Vector3.Distance(c,a);
            float b_ = Vector3.Distance(c,b);
            float a_2 = a_ * a_;
            float b_2 = b_ * b_;
            float c_2 = Vector3.Distance(b, a) * Vector3.Distance(b, a);

            float r = (a_2 + b_2 - c_2)/(2 * a_ * b_);
            return toDegree((float)Math.Acos(r));
        }

      

        public static float toDegree(float rad)
        {
            return (float)(rad * (180 / Math.PI));
        }
        

    }
}
