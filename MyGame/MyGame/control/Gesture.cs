using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Helper;

namespace control
{
    /// <summary>
    /// Abstract class that every gesture should implement to evaluate some core function
    /// </summary>
    abstract class Gesture
    {
        /// <summary>
        /// Indicate either the gesture is active or not
        /// </summary>
        public bool active { get; set; }
        /// <summary>
        /// A quick description for this gesture
        /// </summary>
        public string description { get; protected set; }
        /// <summary>
        /// Abstact method that should be implemented to evaluate this specific gesture.
        /// </summary>
        public abstract void eval();
    }

    /// <summary>
    /// class to detect the lean left gesture
    /// </summary>
    class LeanRight : Gesture
    {
        
        public LeanRight()
        {
            description = "lean right";
        }

        public override void eval()
        {
            float angle = util.Xangle(Kinect.skeleton.spine, Kinect.skeleton.shoulder_center);
            active = angle > 0 && angle < 78;
        }

        
    }

    /// <summary>
    /// class to detect the lean right gesture
    /// </summary>
    class LeanLeft : Gesture
    {
        public LeanLeft()
        {
            description = "lean left";
        }

        public override void eval()
        {
            float angle = util.Xangle(Kinect.skeleton.spine, Kinect.skeleton.shoulder_center);
            active = angle < 0 && angle > -78;
        }
    }

    /// <summary>
    /// class to detect when the right leg is advanced forward.
    /// </summary>
    class RightLegForward : Gesture
    {
        public RightLegForward()
        {
            description = "Right leg forward";
        }

        public override void eval()
        {
            active = Kinect.skeleton.rAnkle.Z - Kinect.skeleton.lAnkle.Z < -0.3f;
        }
    }

    /// <summary>
    /// class to detect when the right leg is backward.
    /// </summary>
    class RightLegBackward : Gesture
    {
        public RightLegBackward()
        {
            description = "Right leg backward";
        }

        public override void eval()
        {
            active = Kinect.skeleton.rAnkle.Z - Kinect.skeleton.lAnkle.Z > 0.3f;
        }
    }

    /// <summary>
    /// class to detect when a specific hand is stretched forward.
    /// </summary>
    class HandStretchForward : Gesture
    {
        int hand;
        public HandStretchForward(int hand)
        {
            this.hand = hand;
            description = hand==Constants.RIGHT_HAND?"Right":"Left"+" hand stretch forward";
        }

        public override void eval()
        {
            if (hand == Constants.RIGHT_HAND)
                active = util.GetAngle(Kinect.skeleton.rWrist, Kinect.skeleton.rShoulder, Kinect.skeleton.rElbow) > 120 &&
                    Math.Abs(Kinect.skeleton.rWrist.X - Kinect.skeleton.rShoulder.X) < .15f &&
                    Math.Abs(Kinect.skeleton.rWrist.Y - Kinect.skeleton.rShoulder.Y) < .15f;
            else
                active = util.GetAngle(Kinect.skeleton.lWrist, Kinect.skeleton.lShoulder, Kinect.skeleton.lElbow) > 120 &&
                    Math.Abs(Kinect.skeleton.lWrist.X - Kinect.skeleton.lShoulder.X) < .15f &&
                    Math.Abs(Kinect.skeleton.lWrist.Y - Kinect.skeleton.lShoulder.Y) < .15f;

        }
    }

    /// <summary>
    /// class to get the z difference between the 2 shoulders.
    /// </summary>
    class shoulderDifference : Gesture
    {
        public float diff { get; private set; }
        public bool left = false;
        public bool right = false;

        public shoulderDifference()
        {
            diff = 0;
        }

        public override void eval()
        {
            diff = Kinect.skeleton.rShoulder.Z - Kinect.skeleton.lShoulder.Z;
            if (Math.Abs(diff) < 0.3)
                left = right = false;
            else
            {
                left = (diff < 0);
                right = !left;
            }
        }
    }

    /// <summary>
    /// class to get the angle between the kinect space z axis and a specific hand.
    /// </summary>
    class HandPointer : Gesture
    {
        private int hand;

        public Vector2 theta { get; private set; }
        private Vector2 currentAngle;
        private Vector3[] currentPos;

        private const float HORZ_LIMIT = 60;
        /// <summary>
        /// constructor of HandPointer class
        /// </summary>
        /// <param name="hand">A specific hand</param>
        public HandPointer(int hand)
        {
            this.hand = hand;
            theta = new Vector2();
        }


        public override void eval()
        {

            if (hand == Constants.RIGHT_HAND)
                active = Math.Abs(Kinect.skeleton.rWrist.Z - Kinect.skeleton.rShoulder.Z) > .17f;
            else
                active = Math.Abs(Kinect.skeleton.lWrist.Z - Kinect.skeleton.lShoulder.Z) > .17f;

            
            currentPos = getPos();
            currentAngle = getAngle(currentPos[0], currentPos[1]); // currentPos[0] => hand 
                                                                   // currentPos[1] => shoulder

            if (float.IsNaN(currentAngle.X) || float.IsNaN(currentAngle.Y))
                return;

            theta = new Vector2(currentAngle.X, currentAngle.Y);

        }

        /// <summary>
        /// get the difference between shoulder and wrist of a specific arm.
        /// </summary>
        private Vector3[] getPos()
        {
            Vector3 hand_pos = new Vector3();
            Vector3 shoulder_pos = new Vector3();
            if (hand == Constants.RIGHT_HAND)
            {
                hand_pos = Kinect.skeleton.rWrist;
                shoulder_pos = Kinect.skeleton.rShoulder;
            }
            else
            {
                hand_pos = Kinect.skeleton.lWrist;
                shoulder_pos = Kinect.skeleton.lShoulder;
            }
            return new Vector3[2] { hand_pos, shoulder_pos };

        }

        /// <summary>
        /// get the angle between the kinect space z axis and a specific hand.
        /// </summary>
        private Vector2 getAngle(Vector3 v1, Vector3 v2)
        {
            Vector2 angle = new Vector2();
            double dx = v1.X - v2.X; //diff X
            double dy = v1.Y - v2.Y; //diff Y
            double dz = v2.Z - v1.Z; //diff z

            angle.X = (float)Math.Atan(dx / dz);
            angle.Y = (float)Math.Atan(dy / dz);
            angle.X = MathHelper.ToDegrees(angle.X);
            angle.Y = MathHelper.ToDegrees(angle.Y);

            return angle;

        }
    }

}

