using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;


namespace control
{
    class Kinect
    {
        //initilize skeleton data holder
        public static skeleton skeleton{ get; private set;}

        private KinectSensor kinectSensor;
        //Runtime nui;

        public Kinect()
        {
            skeleton = new skeleton(0);

            //getting first kinect
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    kinectSensor = sensor;
                    break;
                }
            }

            Console.WriteLine(kinectSensor.Status);

            //starting kinect and enabling skeleton data stream
            kinectSensor.Start();
            kinectSensor.SkeletonStream.Enable();

            //register event handler on skeleton fram ready event
            kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(skeletonFrameReady); 
        }

        //skeleton fram ready event handler
        void skeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            // Have we already been "shut down" by the user of this viewer, 
            // or has the SkeletonStream been disabled since this event was posted?
            if ((this.kinectSensor == null) || !((KinectSensor)sender).SkeletonStream.IsEnabled)
            {
                return;
            }

            Skeleton[] s = new Skeleton[6];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;
                skeletonFrame.CopySkeletonDataTo(s);
                foreach (Skeleton sk in s)
                {
                    if (sk.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        foreach (Joint joint in sk.Joints)
                        {
                            switch (joint.JointType)
                            {
                                case JointType.Head:
                                    copyVect(ref skeleton.head, joint.Position);
                                    break;
                                case JointType.ShoulderCenter:
                                    copyVect(ref skeleton.shoulder_center, joint.Position);
                                    break;
                                case JointType.Spine:
                                    copyVect(ref skeleton.spine, joint.Position);
                                    break;
                                case JointType.HipCenter:
                                    copyVect(ref skeleton.hip_center, joint.Position);
                                    break;

                                case JointType.ShoulderRight:
                                    copyVect(ref skeleton.rShoulder, joint.Position);
                                    break;
                                case JointType.ElbowRight:
                                    copyVect(ref skeleton.rElbow, joint.Position);
                                    break;
                                case JointType.WristRight:
                                    copyVect(ref skeleton.rWrist, joint.Position);
                                    break;
                                case JointType.HandRight:
                                    copyVect(ref skeleton.rHand, joint.Position);
                                    break;

                                case JointType.ShoulderLeft:
                                    copyVect(ref skeleton.lShoulder, joint.Position);
                                    break;
                                case JointType.ElbowLeft:
                                    copyVect(ref skeleton.lElbow, joint.Position);
                                    break;
                                case JointType.WristLeft:
                                    copyVect(ref skeleton.lWrist, joint.Position);
                                    break;
                                case JointType.HandLeft:
                                    copyVect(ref skeleton.lHand, joint.Position);
                                    break;

                                case JointType.HipRight:
                                    copyVect(ref skeleton.rHip, joint.Position);
                                    break;
                                case JointType.KneeRight:
                                    copyVect(ref skeleton.rKnee, joint.Position);
                                    break;
                                case JointType.AnkleRight:
                                    copyVect(ref skeleton.rAnkle, joint.Position);
                                    break;
                                case JointType.FootRight:
                                    copyVect(ref skeleton.rFoot, joint.Position);
                                    break;

                                case JointType.HipLeft:
                                    copyVect(ref skeleton.lHip, joint.Position);
                                    break;
                                case JointType.KneeLeft:
                                    copyVect(ref skeleton.lKnee, joint.Position);
                                    break;
                                case JointType.AnkleLeft:
                                    copyVect(ref skeleton.lAnkle, joint.Position);
                                    break;
                                case JointType.FootLeft:
                                    copyVect(ref skeleton.lFoot, joint.Position);
                                    break;
                            }

                        }
                    }
                }
            }
        }

        // copy skeletonPoint to a vector v1 
        private void copyVect(ref Vector3 v1, SkeletonPoint v2)
        {
            v1.X = v2.X;
            v1.Y = v2.Y;
            v1.Z = v2.Z;
        }



    }
}
