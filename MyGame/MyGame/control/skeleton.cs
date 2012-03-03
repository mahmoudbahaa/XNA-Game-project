using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace control
{
    class skeleton
    {
        public Vector3 head = new Vector3();
        public Vector3 shoulder_center = new Vector3();
        public Vector3 spine = new Vector3();
        public Vector3 hip_center = new Vector3();

        public Vector3 rShoulder = new Vector3 ();
        public Vector3 rElbow = new Vector3();
        public Vector3 rWrist = new Vector3();
        public Vector3 rHand = new Vector3();

        public Vector3 lShoulder = new Vector3();
        public Vector3 lElbow = new Vector3();
        public Vector3 lWrist = new Vector3();
        public Vector3 lHand = new Vector3();

        public Vector3 rHip = new Vector3();
        public Vector3 rKnee = new Vector3();
        public Vector3 rAnkle = new Vector3();
        public Vector3 rFoot = new Vector3();

        public Vector3 lHip = new Vector3();
        public Vector3 lKnee = new Vector3();
        public Vector3 lAnkle = new Vector3();
        public Vector3 lFoot = new Vector3();

        private int id;

        public skeleton (int id){
            this.id = id;
        }
    }
}
