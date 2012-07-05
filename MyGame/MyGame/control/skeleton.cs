using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace control
{
    /// <summary>
    /// entity class that holds the skeleton Frame joints data from kinect sensor stream.
    /// </summary>
    class skeleton
    {
        /// <summary>Head joint.</summary>
        public Vector3 head = new Vector3();
        /// <summary> ShoulderCenter joint.</summary>
        public Vector3 shoulder_center = new Vector3();
        /// <summary> Spine joint.</summary>
        public Vector3 spine = new Vector3();
        /// <summary> HipCenter joint.</summary>
        public Vector3 hip_center = new Vector3();

        /// <summary> ShoulderRight joint.</summary>
        public Vector3 rShoulder = new Vector3 ();
        /// <summary> ElbowRight joint.</summary>
        public Vector3 rElbow = new Vector3();
        /// <summary> WristRight joint.</summary>
        public Vector3 rWrist = new Vector3();
        /// <summary> HandRight joint.</summary>
        public Vector3 rHand = new Vector3();

        /// <summary> ShoulderLeft joint.</summary>
        public Vector3 lShoulder = new Vector3();
        /// <summary> ElbowLeft joint.</summary>
        public Vector3 lElbow = new Vector3();
        /// <summary> WristLeft joint.</summary>
        public Vector3 lWrist = new Vector3();
        /// <summary>HandLeft joint.</summary>
        public Vector3 lHand = new Vector3();

        /// <summary>HipRight joint.</summary>
        public Vector3 rHip = new Vector3();
        /// <summary>KneeRight joint.</summary>
        public Vector3 rKnee = new Vector3();
        /// <summary>AnkleRight joint.</summary>
        public Vector3 rAnkle = new Vector3();
        /// <summary>FootRight joint.</summary>
        public Vector3 rFoot = new Vector3();

        /// <summary>HipLeft joint.</summary>
        public Vector3 lHip = new Vector3();
        /// <summary>KneeLeft joint.</summary>
        public Vector3 lKnee = new Vector3();
        /// <summary>AnkleLeft joint.</summary>
        public Vector3 lAnkle = new Vector3();
        /// <summary>FootLeft joint.</summary>
        public Vector3 lFoot = new Vector3();

        /// <summary> skeleton id.</summary>
        private int id;

        /// <summary>
        /// Constructor of Skeleton class.
        /// </summary>
        /// <param name="id">of this skeleton</param>
        public skeleton (int id){
            this.id = id;
        }
    }
}
