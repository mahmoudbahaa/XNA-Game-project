using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using SkinnedModelPipeline;
using Microsoft.Xna.Framework;

namespace MyGame
{
    public class SkinnedAnimationPlayer
    {
        SkinningData skinningData;

        // The currently playing clip, if there is one
        public AnimationClip CurrentClip { get; private set; }

        // Whether the current animation has finished
        public bool Done { get; private set; }

        // Timing values
        private TimeSpan startTime, endTime, currentTime;
        private int currentKeyframe;
        public bool loop;

        // Transforms
        public Matrix[] BoneTransforms { get; private set; }
        public Matrix[] WorldTransforms { get; private set; }
        public Matrix[] SkinTransforms { get; private set; }

        public SkinnedAnimationPlayer(SkinningData skinningData)
        {
            this.skinningData = skinningData;

            BoneTransforms = new Matrix[skinningData.BindPose.Count];
            WorldTransforms = new Matrix[skinningData.BindPose.Count];
            SkinTransforms = new Matrix[skinningData.BindPose.Count];
        }

        // Starts playing the entirety of the given clip
        public void StartClip(string clip, bool loop)
        {
            AnimationClip clipVal = skinningData.AnimationClips[clip];
            StartClip(clip, TimeSpan.FromSeconds(0), clipVal.Duration, loop);
        }

        // Plays a specific portion of the given clip, from one frame
        // index to another
        public void StartClip(string clip, int startFrame, int endFrame, bool loop)
        {
            AnimationClip clipVal = skinningData.AnimationClips[clip];

            StartClip(clip, clipVal.Keyframes[startFrame].Time,
                clipVal.Keyframes[endFrame].Time, loop);
        }

        // Plays a specific portion of the given clip, from one time
        // to another
        public void StartClip(string clip, TimeSpan StartTime, TimeSpan EndTime, bool loop)
        {
            CurrentClip = skinningData.AnimationClips[clip];
            currentTime = TimeSpan.FromSeconds(0);
            currentKeyframe = 0;
            Done = false;
            this.startTime = StartTime;
            this.endTime = EndTime;
            this.loop = loop;

            // Copy the bind pose to the bone transforms array to reset the animation
            skinningData.BindPose.CopyTo(BoneTransforms, 0);
        }

        public void Update(TimeSpan time, Matrix rootTransform)
        {
            if (CurrentClip == null || Done)
                return;

            currentTime += time;

            updateBoneTransforms();
            updateWorldTransforms(rootTransform);
            updateSkinTransforms();
        }

        // Helper used by the Update method to refresh the BoneTransforms data
        private void updateBoneTransforms()
        {
            // If the current time has passed the end of the animation...
            while (currentTime >= (endTime - startTime))
            {
                // If we are looping, reduce the time until we are
                // back in the animation's time frame
                if (loop)
                {
                    currentTime -= (endTime - startTime);
                    currentKeyframe = 0;
                    skinningData.BindPose.CopyTo(BoneTransforms, 0);
                }
                // Otherwise, clamp to the end of the animation
                else
                {
                    Done = true;
                    currentTime = endTime;
                    break;
                }
            }

            // Read keyframe matrices
            IList<Keyframe> keyframes = CurrentClip.Keyframes;

            // Read keyframes until we have found the latest frame before
            // the current time
            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > currentTime + startTime)
                    break;

                // Use this keyframe.
                BoneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }

        // Helper used by the Update method to refresh the WorldTransforms data.
        private void updateWorldTransforms(Matrix rootTransform)
        {
            // Root bone
            WorldTransforms[0] = BoneTransforms[0] * rootTransform;

            // For each child bone...
            for (int bone = 1; bone < WorldTransforms.Length; bone++)
            {
                // Add the transform of the parent bone
                int parentBone = skinningData.SkeletonHierarchy[bone];

                WorldTransforms[bone] = BoneTransforms[bone] *
                                                WorldTransforms[parentBone];
            }
        }

        // Helper used by the Update method to refresh the SkinTransforms data
       public void updateSkinTransforms()
        {
            for (int bone = 0; bone < SkinTransforms.Length; bone++)
                SkinTransforms[bone] = skinningData.InverseBindPose[bone] *
                                            WorldTransforms[bone];
        }
    }
}
