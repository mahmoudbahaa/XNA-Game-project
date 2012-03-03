using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Helper;
using MyGame;

namespace control
{
    abstract class Gesture
    {
        public bool active { get; protected set; }
        public bool hold;
        public string description { get; protected set; }
        public abstract void eval();

        public virtual Object[] getArgs() {
            return null;
        }
    }


    class handBackHead : Gesture
    {

        int hand;

        public handBackHead(int hand)
        {
            description = "hand behind head";
            this.hand = hand;
        }
        public override void eval()
        {
            //Console.WriteLine("----------------------------------------");
            //Console.WriteLine("rhand.x =" + (kinect.skeleton.rHand.X - kinect.skeleton.head.X));
            //Console.WriteLine("rhand.z =" + (kinect.skeleton.rHand.Z - kinect.skeleton.head.Z));
            //Console.WriteLine("rhand.y =" + (kinect.skeleton.rHand.Y - kinect.skeleton.rShoulder.Y));
            if (Kinect.skeleton.rHand.Z == 0 && Kinect.skeleton.rHand.X == 0 && Kinect.skeleton.head.X == 0)
                active = false;
            if(hand == Constants.RIGHT_HAND)
                active = Math.Abs(Kinect.skeleton.rHand.X - Kinect.skeleton.head.X) < 0.25f && (Kinect.skeleton.rHand.Y - Kinect.skeleton.rShoulder.Y) < 0.25f && (Kinect.skeleton.rHand.Y - Kinect.skeleton.rShoulder.Y) > 0 && Math.Abs(Kinect.skeleton.rHand.Z - Kinect.skeleton.head.Z) < 0.25f;
            else
                active =  Math.Abs(Kinect.skeleton.lHand.X - Kinect.skeleton.head.X) < 0.25f && (Kinect.skeleton.lHand.Y - Kinect.skeleton.lShoulder.Y) < 0.25f && (Kinect.skeleton.lHand.Y - Kinect.skeleton.lShoulder.Y) > 0 && Math.Abs(Kinect.skeleton.lHand.Z - Kinect.skeleton.head.Z) < 0.25f;
        }

    }

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

    class RightLegForward : Gesture
    {
        public RightLegForward()
        {
            description = "Right leg forward";
        }

        public override void eval()
        {
           // Console.WriteLine(kinect.skeleton.rAnkle.Z + "," + kinect.skeleton.lAnkle.Z);
            active = Kinect.skeleton.rAnkle.Z - Kinect.skeleton.lAnkle.Z < -0.3f;
            //if (active)
            //    this.fireEvent(game, "", "x", 394);
        }
    }

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

    class HandPointer : Gesture
    {
        private int hand;

        public Vector2 theta { get; private set; }
        private Vector2 currentAngle;
        private Vector3[] currentPos;

        private const float HORZ_LIMIT = 60;

        public HandPointer(int hand)
        {
            this.hand = hand;
            theta = new Vector2();
        }


        public override void eval()
        {
            
            currentPos = getPos();
            currentAngle = getAngle(currentPos[0], currentPos[1]); // currentPos[0] => hand 
                                                                   // currentPos[1] => shoulder

            if (float.IsNaN(currentAngle.X) || float.IsNaN(currentAngle.Y))
                return;

            //Console.WriteLine("=====(" + (tmpX) + "," + (tmpY) + ")");

            //if (current.X > 45)
            //{
            //    tmpX = HORZ_LIMIT;
            //}
            //else if (current.X < -45)
            //{
            //    tmpX = -HORZ_LIMIT;
            //}

            // TODO: calibarate vertical angle limit

            theta = new Vector2(currentAngle.X, currentAngle.Y);

        }

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

        public override Object[] getArgs()
        {
            return new Object[]{"theta",theta};
        }
    }

}

